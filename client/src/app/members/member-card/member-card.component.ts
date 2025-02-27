import {
  Component,
  computed,
  inject,
  input,
  ViewEncapsulation,
} from '@angular/core';
import { Member } from '../../_models/member';
import { RouterLink } from '@angular/router';
import { LikesService } from '../../_services/likes.service';
import { PresenceService } from '../../_services/presence.service';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css',
  encapsulation: ViewEncapsulation.None, // CSS của component này sẽ áp dụng toàn cục lên toàn bộ ứng dụng (giống như CSS thông thường). -Không bị đóng gói vào riêng component.
})
export class MemberCardComponent {
  private likeSvc = inject(LikesService);
  private presenceService = inject(PresenceService);

  member = input.required<Member>();
  hasLiked = computed(() => this.likeSvc.likeIds().includes(this.member().id));
  isOnline = computed(() => this.presenceService.onlineUsers().includes(this.member().userName));

  toggleLike() {
    this.likeSvc.toggleLike(this.member().id).subscribe({
      next: () => {
        if (this.hasLiked()) {
          // bỏ thích
          this.likeSvc.likeIds.update((ids) =>
            ids.filter((x) => x !== this.member().id)
          );
        } else {
          // yêu thích
          this.likeSvc.likeIds.update((ids) => [...ids, this.member().id]);
        }
      },
    });
  }
}
