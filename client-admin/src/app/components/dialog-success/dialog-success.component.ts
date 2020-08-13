import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-dialog-success',
  templateUrl: './dialog-success.component.html',
  styleUrls: ['./dialog-success.component.css'],
})
export class DialogSuccessComponent {
  title: string;
  message: string;
  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
    this.title = data.header;
    this.message = data.detail;
  }
}
