import { Component } from '@angular/core';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  standalone: true,
  imports: [],
  templateUrl: './server-error.component.html',
  styleUrl: './server-error.component.css',
})
export class ServerErrorComponent {
  error: any;

  // private router: Router: Inject service Router để truy cập thông tin điều hướng hiện tại.
  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation(); // Lấy thông tin điều hướng hiện tại.
    this.error = navigation?.extras?.state?.['error']; // Truy cập vào extras.state của thông tin điều hướng để lấy dữ liệu lỗi (error) được truyền qua state.
  }
}
