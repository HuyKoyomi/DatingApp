import { NgIf } from '@angular/common';
import { Component, input, Self } from '@angular/core';
import {
  ControlValueAccessor,
  FormControl,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';

@Component({
  selector: 'app-text-input',
  standalone: true,
  imports: [NgIf, ReactiveFormsModule],
  templateUrl: './text-input.component.html',
  styleUrl: './text-input.component.css',
})
export class TextInputComponent implements ControlValueAccessor {
  label = input<string>('');
  type = input<string>('text');

  // tạo custom form components mà vẫn hoạt động hoàn toàn như các input tiêu chuẩn của Angular Forms.
  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  writeValue(obj: any): void {}
  registerOnChange(fn: any): void {}
  registerOnTouched(fn: any): void {}

  /*
  // tạo getter control
    + Dễ dàng truy cập trạng thái của form control (valid, invalid, touched, pristine, v.v.).
    + Tránh việc phải truyền FormControl vào component.
    + Tích hợp tốt với Angular Forms, cho phép component tùy chỉnh hoạt động như một input thông thường.
  => Getter control giúp bạn dễ dàng truy cập vào FormControl bên trong một component tùy chỉnh khi làm việc với Angular Forms.
  */
  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }
}
