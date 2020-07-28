import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-generic-success',
  templateUrl: './dialog-generic-success.component.html',
  styleUrls: ['./dialog-generic-success.component.css']
})
export class DialogGenericSuccessComponent {
  message: any;
  html: string;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
    this.message = data;
    this.html = data.detail;
  }
}
