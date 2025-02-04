import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, output, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/photo';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from './../_models/userParams';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  // members = signal<Member[]>([]);
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);

  getMembers(userParams: UserParams) {
    // cach su dung param trong angular
    let params = new HttpParams();
    if (userParams.pageNumber && userParams.pageSize) {
      params = params.append('pageNumber', userParams.pageNumber);
      params = params.append('pageSize', userParams.pageSize);
    }
    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);


    return this.http
      .get<Member[]>(this.baseUrl + 'users', { observe: 'response', params })
      .subscribe({
        next: (response) => {
          this.paginatedResult.set({
            items: response.body as Member[],
            pagination: JSON.parse(response.headers.get('Pagination')!), //  Dấu ! (non-null assertion operator) trong TypeScript sẽ bỏ qua kiểm tra null, nhưng nếu get('Pagination') trả về null, JSON.parse(null) sẽ gây lỗi.
          });
        },
      });
  }

  getMember(username: string) {
    // //  kiểm tra xem member có trong list hiện tại hay không
    // const member = this.members().find((x) => x.username == username);
    // if (member) return of(member); // of() là một phương thức của rxjs, dùng để tạo một Observable từ một giá trị đơn lẻ
    // nếu không call api
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
    // => dữ liệu trả ra là 1 Observable
  }

  // pipe() dùng để xử lý dữ liệu trước khi nó đến component
  // tap(): Toán tử RxJS giúp thực hiện một thao tác phụ mà không làm thay đổi dữ liệu chính.
  updateMember(member: Member) {
    return this.http
      .put<Member>(this.baseUrl + 'users', member)
      .pipe
      // tap(() => {
      //   this.members.update((members) =>
      //     members.map((m) => (m.username == member.username ? member : m))
      //   );
      // })
      ();
  }

  setMainPhoto(photo: Photo) {
    return this.http
      .put(this.baseUrl + 'users/set-main-photo/' + photo.id, {})
      .pipe
      // tap(() => {
      //   this.members.update((members) =>
      //     members.map((m) => {
      //       if (m.photos.includes(photo)) {
      //         m.photoUrl = photo.url;
      //       }
      //       return m;
      //     })
      //   );
      // })
      ();
  }

  deletePhoto(photo: Photo) {
    return this.http
      .delete(this.baseUrl + 'users/delete-photo/' + photo.id)
      .pipe
      // tap(() => {
      //   this.members.update((members) =>
      //     members.map((m) => {
      //       if (m.photos.includes(photo)) {
      //         m.photos = m.photos.filter((x) => x.id !== photo.id);
      //       }
      //       return m;
      //     })
      //   );
      // })
      ();
  }
}
