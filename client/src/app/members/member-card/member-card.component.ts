import { Component, input, ViewEncapsulation } from '@angular/core';
import { Member } from '../../_models/member';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css',
  encapsulation: ViewEncapsulation.None, // CSS của component này sẽ áp dụng toàn cục lên toàn bộ ứng dụng (giống như CSS thông thường). -Không bị đóng gói vào riêng component.
})
export class MemberCardComponent {
  member = input.required<Member>();
}
