import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { PaginatedResult } from '../_models/pagination';
import { Message } from '../_models/message';
import {
  setPaginationHeaders,
  setPaginationResponse,
} from './paginationHelper';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubsUrl;
  private hubConnection?: HubConnection;
  private http = inject(HttpClient);
  paginatedResult = signal<PaginatedResult<Message[]> | null>(null);
  messageThread = signal<Message[]>([]);

  createHubConnection(use: User, otherUserName: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + "messages?user=" + otherUserName, {
        accessTokenFactory: () => use.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(err => console.log(err))
    this.hubConnection.on('ReceiveMessageThread', message => {
      this.messageThread.set(message);
    })

  }

  getMessages(container: string, pageNumber: number, pageSize: number) {
    let params = setPaginationHeaders(pageNumber, pageSize);
    params = params.append('container', container);

    return this.http
      .get<Message[]>(`${this.baseUrl}messages`, {
        observe: 'response',
        params,
      })
      .subscribe({
        next: (response) =>
          setPaginationResponse(response, this.paginatedResult),
      });
  }

  getMessageThread(userName: string) {
    return this.http.get<Message[]>(
      `${this.baseUrl}messages/thread/${userName}`
    );
  }

  sendMessage(userName: string, content: string) {
    return this.http.post<Message>(this.baseUrl + 'messages', {
      recipientUsername: userName,
      content: content,
    });
  }

  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'messages/' + id);
  }
}
