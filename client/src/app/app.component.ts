import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NgxSpinnerComponent } from 'ngx-spinner';
import { AccountService } from './_services/account.service';
import { HomeComponent } from './home/home.component';
import { NavComponent } from './nav/nav.component';

// Định nghĩa decorator @Component
@Component({
  selector: 'app-root', // Định nghĩa tên thẻ HTML mà component này sẽ gắn vào
  standalone: true, // Chỉ định rằng đây là một thành phần độc lập (standalone component), không cần khai báo trong một module
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  imports: [NavComponent, RouterOutlet, NgxSpinnerComponent, HomeComponent], // Khai báo các dependencies mà component này cần => component sử dụng RouterOutlet để xử lý điều hướng.
})
export class AppComponent implements OnInit {
  private accountService = inject(AccountService);

  // ham khoi tao khi chay vao component
  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }
}
