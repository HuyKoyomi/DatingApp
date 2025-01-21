import { CanDeactivateFn } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

// Đây là một Route Guard trong Angular sử dụng CanDeactivateFn để ngăn người dùng rời khỏi trang nếu có thay đổi chưa được lưu trong biểu mẫu (editForm).
export const preventUnsavedChangesGuard: CanDeactivateFn<
  MemberEditComponent
> = (component) => {
  if (component.editForm?.dirty) {
    return confirm(
      'Are you sure you want to continue? Any unsaved changes will be lost'
    );
  }
  return true;
};
