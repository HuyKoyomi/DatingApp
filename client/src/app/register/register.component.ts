import { JsonPipe, NgIf } from '@angular/common';
import { Component, inject, OnInit, output } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';
import { DatePickerComponent } from '../_forms/date-picker/date-picker.component';
import { TextInputComponent } from '../_forms/text-input/text-input.component';
import { AccountService } from './../_services/account.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    TextInputComponent,
    BsDatepickerModule,
    DatePickerComponent,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  private accountService = inject(AccountService);
  private toasrt = inject(ToastrService);
  private router = inject(Router);
  private fb = inject(FormBuilder); // sử dụng FormBuilder - Code ngắn gọn hơn, dễ đọc. // Dễ mở rộng với form phức tạp. // Hỗ trợ Dependency Injection tốt hơn.
  cancelRegister = output<boolean>();
  model: any = {};
  form: FormGroup = new FormGroup({});
  maxDate = new Date();
  validationErrors: string[] | undefined;

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm() {
    this.form = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
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
    const dob = this.getDateOnly(this.form.get('dateOfBirth')?.value);
    this.form.patchValue({ dateOfBirth: dob });
    this.accountService.register(this.form.value).subscribe({
      next: (_) => {
        this.router.navigateByUrl('/members');
      },
      error: (error) => (this.validationErrors = error),
    });
  }

  cancel() {
    this, this.cancelRegister.emit(false);
  }

  private getDateOnly(dob: string | undefined) {
    if (!dob) return;
    return new Date(dob).toISOString().slice(0, 10);
  }
}
