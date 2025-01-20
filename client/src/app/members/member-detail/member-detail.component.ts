import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { Member } from '../../_models/member';
import { MembersService } from '../../_services/members.service';
// import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css',
})
export class MemberDetailComponent implements OnInit {
  private membersService = inject(MembersService);
  private route = inject(ActivatedRoute); // laay router hien tai
  member?: Member;
  images: string[] = [];

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const username = this.route.snapshot.paramMap.get('username'); // lay bien username tren router
    if (!username) return;
    this.membersService.getMember(username).subscribe({
      next: (member) => {
        this.member = member;
        member.photos.map((el) => {
          this.images.push(el.url);
        });
      },
    });
  }
}
