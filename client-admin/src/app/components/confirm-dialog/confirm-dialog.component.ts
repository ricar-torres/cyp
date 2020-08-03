import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Component, OnInit, Inject, Input } from '@angular/core';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css'],
})
export class ConfirmDialogComponent implements OnInit {
  title: string;
  message: string;
  noButton: boolean;
  yesButton: boolean;
  okButton: boolean;

  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmDialogModel
  ) {
    // Update view with given values
    this.title = data.title;
    this.message = data.message;
    this.noButton = data.noButton;
    this.yesButton = data.yesButton;
    this.okButton = data.okButton;
  }

  ngOnInit() {}

  onConfirm(): void {
    // Close the dialog, return true
    this.dialogRef.close(true);
  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }
}

/**
 * Class to represent confirm dialog model.
 *
 * It has been kept here to keep it as part of shared component.
 */
export class ConfirmDialogModel {
  constructor(
    public title: string,
    public message: string,
    public noButton: boolean,
    public yesButton: boolean,
    public okButton: boolean
  ) {}
}
