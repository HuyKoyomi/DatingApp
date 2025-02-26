import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { PaginatedResult } from '../_models/pagination';
import { Message } from '../_models/message';
import {
  setPaginationHeaders,
  setPaginationResponse,
} from './paginationHelper';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  paginatedResult = signal<PaginatedResult<Message[]> | null>(null);

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
