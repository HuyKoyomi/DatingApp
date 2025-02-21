import { Directive, inject, Input, input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AccountService } from '../_services/account.service';

// kiểm soát hiển thị các phần tử HTML dựa trên quyền (role) của người dùng
/*
  Directive: Đánh dấu đây là một directive tùy chỉnh
  
  
  
*/
@Directive({
  selector: '[appHasRole]',
  standalone: true
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[] = [] // Input: Cho phép directive nhận giá trị đầu vào từ component cha.
  private accountSvc = inject(AccountService);
  private viewContainerRef = inject(ViewContainerRef); // ViewContainerRef: Dùng để thao tác với View (thêm/xóa nội dung động).
  private templateRef = inject(TemplateRef); // TemplateRef: Đại diện cho nội dung HTML nằm trong directive.

  ngOnInit(): void {
    // Kiểm tra xem người dùng có ít nhất một quyền trong danh sách 
    if (this.accountSvc.roles()?.some((r: string) => this.appHasRole.includes(r))) {
      this.viewContainerRef.createEmbeddedView(this.templateRef); // Nếu có quyền → Hiển thị nội dung (createEmbeddedView).
    } else {
      this.viewContainerRef.clear(); // Nếu không có quyền → Xóa nội dung (clear()).
    }
  }

}
