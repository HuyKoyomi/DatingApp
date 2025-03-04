import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { of } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';
import { Photo } from '../_models/photo';
import { UserParams } from './../_models/userParams';
import { AccountService } from './account.service';
import {
  setPaginationHeaders,
  setPaginationResponse,
} from './paginationHelper';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  baseUrl = environment.apiUrl;
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);
  memberCache = new Map(); // hàm cache
  user = this.accountService.currentUser();
  userParams = signal<UserParams>(new UserParams(this.user));

  restUserParams() {
    this.userParams.set(new UserParams(this.user));
  }

  getMembers() {
    const response = this.memberCache.get(
      Object.values(this.userParams()).join('-')
    ); // chuyển các giá trị về thành chuỗi duy nhất
    if (response) return setPaginationResponse(response, this.paginatedResult); // Kiểm tra xem dữ liệu có trong cache không để tránh gọi API ko cần thiết

    // cach su dung param trong angular
    // let params = new HttpParams();
    // if (this.userParams().pageNumber && this.userParams().pageSize) {
    //   params = params.append('pageNumber', this.userParams().pageNumber);
    //   params = params.append('pageSize', this.userParams().pageSize);
    // }
    let params = setPaginationHeaders(
      this.userParams().pageNumber,
      this.userParams().pageSize
    );
    params = params.append('minAge', this.userParams().minAge);
    params = params.append('maxAge', this.userParams().maxAge);
    params = params.append('gender', this.userParams().gender);
    params = params.append('orderBy', this.userParams().orderBy);

    return this.http
      .get<Member[]>(this.baseUrl + 'users', { observe: 'response', params })
      .subscribe({
        next: (res) => {
          setPaginationResponse(res, this.paginatedResult);
          this.memberCache.set(Object.values(this.userParams()).join('-'), res); // cập nhật cache
        },
      });
  }

  getMember(userName: string) {
    const member: Member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.body), [])
      .find((m: Member) => m.userName === userName);
    if (member) return of(member);
    // //  kiểm tra xem member có trong list hiện tại hay không
    // const member = this.members().find((x) => x.userName == userName);
    // if (member) return of(member); // of() là một phương thức của rxjs, dùng để tạo một Observable từ một giá trị đơn lẻ
    // nếu không call api
    return this.http.get<Member>(this.baseUrl + 'users/' + userName);
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
      //     members.map((m) => (m.userName == member.userName ? member : m))
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
