import { NgIf } from '@angular/common';
import { Component, input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from '@angular/forms';
import {
  BsDatepickerConfig,
  BsDatepickerModule,
} from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-date-picker',
  standalone: true,
  imports: [BsDatepickerModule, ReactiveFormsModule, NgIf],
  templateUrl: './date-picker.component.html',
  styleUrl: './date-picker.component.css',
})
export class DatePickerComponent implements ControlValueAccessor {
  label = input<string>('');
  maxDate = input<Date>();
  bsConfig?: Partial<BsDatepickerConfig>;
  /*  Partial<T> là một utility type của TypeScript.
      Nó biến tất cả các thuộc tính của T thành tùy chọn (?), tức là bạn không cần phải khai báo đầy đủ tất cả các thuộc tính của BsDatepickerConfig.
  */

  // tạo custom form components mà vẫn hoạt động hoàn toàn như các input tiêu chuẩn của Angular Forms.
  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
    this.bsConfig = {
      containerClass: 'theme-red',
      dateInputFormat: 'DD MMMM YYYY',
    };
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
