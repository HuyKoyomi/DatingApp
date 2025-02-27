import {
  Component,
  inject,
  input,
  ViewChild
} from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';
import { MessageService } from '../../_services/message.service';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css',
})
export class MemberMessagesComponent {
  @ViewChild('messageForm') messageForm?: NgForm;
  messageService = inject(MessageService);
  userName = input.required<string>();
  messageContent = '';

  sendMessage() {
    this.messageService.sendMessage(this.userName(), this.messageContent).subscribe({
      next: (message) => {
        this.messageForm?.reset();
      },
    });
  }
}
