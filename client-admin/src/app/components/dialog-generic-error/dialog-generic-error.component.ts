import { Component, Inject } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-generic-error',
  templateUrl: './dialog-generic-error.component.html',
  styleUrls: ['./dialog-generic-error.component.css']
})
export class DialogGenericErrorComponent {
  message: any;
  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
    this.message = data;
  }
}
