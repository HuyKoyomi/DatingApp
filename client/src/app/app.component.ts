import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { HomeComponent } from './home/home.component';
import { NavComponent } from './nav/nav.component';

// Định nghĩa decorator @Component
@Component({
  selector: 'app-root', // Định nghĩa tên thẻ HTML mà component này sẽ gắn vào
  standalone: true, // Chỉ định rằng đây là một thành phần độc lập (standalone component), không cần khai báo trong một module
  imports: [NavComponent, HomeComponent], // Khai báo các dependencies mà component này cần => component sử dụng RouterOutlet để xử lý điều hướng.
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  http = inject(HttpClient);
  title = 'DatingApp';
  users: any;
  private accountService = inject(AccountService);

  // ham khoi tao khi chay vao component
  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);
  }
}
