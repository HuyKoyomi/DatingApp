import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { Member } from '../../_models/member';
import { MembersService } from '../../_services/members.service';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import { Message } from '../../_models/message';
import { MessageService } from '../../_services/message.service';
import { DatePipe } from '@angular/common';
import { TimeagoModule } from 'ngx-timeago';
import { PresenceService } from '../../_services/presence.service';
// import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule, MemberMessagesComponent, DatePipe, TimeagoModule],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css',
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs', { static: true }) memberTabs?: TabsetComponent;
  presenceService = inject(PresenceService);
  private messSvc = inject(MessageService);
  private route = inject(ActivatedRoute); // laay router hien tai
  member: Member = {} as Member;
  images: string[] = [];
  activeTab?: TabDirective;
  messages: Message[] = [];

  ngOnInit(): void {
    this.route.data.subscribe({
      next: (data) => {
        this.member = data['member'];
        this.member &&
          this.member.photos.map((el) => {
            this.images.push(el.url);
          });
      },
    });

    this.route.queryParams.subscribe({
      next: (params) => params['tab'] && this.selectTab(params['tab']),
    });
  }

  onUpdateMessages(event: Message) {
    this.messages.push(event);
  }

  onTabActived(data: TabDirective) {
    this.activeTab = data;
    if (
      this.activeTab.heading === 'Messages' &&
      this.messages.length === 0 &&
      this.member
    ) {
      this.messSvc.getMessageThread(this.member.userName).subscribe({
        next: (messages) => (this.messages = messages),
      });
    }
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      const messTab = this.memberTabs.tabs.find((x) => x.heading === heading);
      if (messTab) messTab.active = true;
    }
  }

  // loadMember() {
  //   const userName = this.route.snapshot.paramMap.get('userName'); // lay bien userName tren router
  //   if (!userName) return;
  //   this.membersService.getMember(userName).subscribe({
  //     next: (member) => {
  //       this.member = member;
  //       member.photos.map((el) => {
  //         this.images.push(el.url);
  //       });
  //     },
  //   });
  // }
}
