import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

// Định nghĩa decorator @Component
@Component({
  selector: 'app-root', // Định nghĩa tên thẻ HTML mà component này sẽ gắn vào
  standalone: true, // Chỉ định rằng đây là một thành phần độc lập (standalone component), không cần khai báo trong một module
  imports: [RouterOutlet], // Khai báo các dependencies mà component này cần => component sử dụng RouterOutlet để xử lý điều hướng.
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  http = inject(HttpClient);
  title = 'DatingApp';
  users: any;

  ngOnInit(): void {
    // throw new Error('Method not implemented.');
    this.http.get('https://localhost:5000/api/users').subscribe({
      next: (response) => (this.users = response),
      error: (error) => console.error(error),
      complete: () => console.log('Request has completed'),
    });
  }

  // constructor(private httpClient: HttpClient) {}
}
