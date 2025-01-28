import { Component, inject, output, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from './../_services/account.service';
import { JsonPipe, NgIf } from '@angular/common';
import { TextInputComponent } from '../_forms/text-input/text-input.component';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    JsonPipe,
    NgIf,
    TextInputComponent,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  private accountService = inject(AccountService);
  private toasrt = inject(ToastrService);
  private fb = inject(FormBuilder); // sử dụng FormBuilder - Code ngắn gọn hơn, dễ đọc. // Dễ mở rộng với form phức tạp. // Hỗ trợ Dependency Injection tốt hơn.
  cancelRegister = output<boolean>();
  model: any = {};
  form: FormGroup = new FormGroup({});

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: [
        '',
        [Validators.required, Validators.minLength(4), Validators.maxLength(8)],
      ],
      confirmPassword: [
        '',
        [Validators.required, this.matchValues('password')],
      ],
    });

    this.form.controls['password'].valueChanges.subscribe({
      next: () =>
        this.form.controls['confirmPassword'].updateValueAndValidity(),
    });
  }

  matchValues(mathTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(mathTo)?.value
        ? null
        : { isMatching: true };
    };
  }
  register() {
    console.log(this.form.value);
    // this.accountService.register(this.model).subscribe({
    //   next: (response) => {
    //     console.log(response);
    //     this.cancel();
    //   },
    //   error: (error) => this.toasrt.error(error.error),
    // });
  }

  cancel() {
    this, this.cancelRegister.emit(false);
  }
}
