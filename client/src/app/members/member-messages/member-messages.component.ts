import {
  Component,
  inject,
  input,
  OnInit,
  output,
  ViewChild,
} from '@angular/core';
import { MessageService } from '../../_services/message.service';
import { Message } from './../../_models/message';
import { TimeagoModule } from 'ngx-timeago';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css',
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm?: NgForm;
  private messSVC = inject(MessageService);
  userName = input.required<string>();
  messages = input.required<Message[]>();
  messageContent = '';
  updateMessages = output<Message>();

  ngOnInit(): void {}
  sendMessage() {
    this.messSVC.sendMessage(this.userName(), this.messageContent).subscribe({
      next: (message) => {
        this.updateMessages.emit(message);
        this.messageForm?.reset();
      },
    });
  }
}
