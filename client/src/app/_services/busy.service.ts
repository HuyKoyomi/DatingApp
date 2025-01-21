import { inject, Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  busyReqCount = 0;
  private spinnerSvc = inject(NgxSpinnerService);

  busy() {
    this.busyReqCount++;
    this.spinnerSvc.show(undefined, {
      type: "ball-scale-multiple",
      bdColor: 'rgba(255, 255, 255, 0)',
      color: '#333333',
    });
  }

  idle() {
    this.busyReqCount--;
    if (this.busyReqCount <= 0) {
      this.busyReqCount = 0;
      this.spinnerSvc.hide();
    }
  }
}
