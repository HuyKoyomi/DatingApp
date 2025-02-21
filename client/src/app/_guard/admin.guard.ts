import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountSvc = inject(AccountService);
  const toastr = inject(ToastrService);
  console.log(accountSvc.roles())
  if (
    accountSvc.roles().includes('Admin') ||
    accountSvc.roles().includes('Moderator')
  ) {
    return true;
  } else {
    toastr.error('You cannot enter this area');
    return false;
  }
};
