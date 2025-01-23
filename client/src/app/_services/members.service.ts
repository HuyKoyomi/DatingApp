import { HttpClient } from '@angular/common/http';
import { inject, Injectable, output, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/photo';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  members = signal<Member[]>([]);

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'users').subscribe({
      next: (members) => this.members.set(members),
    });
  }

  getMember(username: string) {
    //  kiểm tra xem member có trong list hiện tại hay không
    const member = this.members().find((x) => x.username == username);
    if (member) return of(member); // of() là một phương thức của rxjs, dùng để tạo một Observable từ một giá trị đơn lẻ
    // nếu không call api
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
    // => dữ liệu trả ra là 1 Observable
  }

  // pipe() dùng để xử lý dữ liệu trước khi nó đến component
  // tap(): Toán tử RxJS giúp thực hiện một thao tác phụ mà không làm thay đổi dữ liệu chính.
  updateMember(member: Member) {
    return this.http.put<Member>(this.baseUrl + 'users', member).pipe(
      tap(() => {
        this.members.update((members) =>
          members.map((m) => (m.username == member.username ? member : m))
        );
      })
    );
  }

  setMainPhoto(photo: Photo) {
    return this.http
      .put(this.baseUrl + 'users/set-main-photo/' + photo.id, {})
      .pipe(
        tap(() => {
          this.members.update((members) =>
            members.map((m) => {
              if (m.photos.includes(photo)) {
                m.photoUrl = photo.url;
              }
              return m;
            })
          );
        })
      );
  }
}
