import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { faSave } from '@fortawesome/free-solid-svg-icons';
import { MatSnackBar } from '@angular/material';
import { ApiService } from '../../shared/api.service';
import { AppService } from '../../shared/app.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-comment-dialog',
  templateUrl: './comment-dialog.component.html',
  styleUrls: ['./comment-dialog.component.css'],
})
export class CommentDialogComponent implements OnInit {
  id;
  fileId;
  key;
  loading: boolean = false;
  reactiveForm: FormGroup;
  faSave = faSave;
  form: FormGroup;
  durationInSeconds = 30;

  constructor(
    public dialogRef: MatDialogRef<CommentDialogComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: any,
    private fb: FormBuilder,
    private _snackBar: MatSnackBar,
    private api: ApiService,
    private app: AppService
  ) {
    this.setupForm(null);
  }

  async ngOnInit() {
    try {
      this.fileId = this.data.file.id;
      this.key = this.data.file.key;

      if (this.data.comment) {
        this.id = this.data.comment.id;
        this.setupForm(this.data.comment);
      }
    } catch (error) {}
  }

  setupForm(comment) {
    if (comment) {
      var datePipe = new DatePipe('en-US');

      this.form = this.fb.group({
        createDt: [
          {
            value: datePipe.transform(this.data.comment.createDt, 'MM/dd/yyyy'),
            disabled: true,
          },
          Validators.required,
        ],
        createdUser: [
          { value: this.data.comment.createdUser.fullName, disabled: true },
          Validators.required,
        ],
        text: [
          { value: this.data.comment.text, disabled: true },
          Validators.required,
        ],
      });
    } else {
      this.form = this.fb.group({
        text: ['', Validators.required],
      });
    }
  }

  async onSubmit() {
    try {
      if (this.form.valid) {
        this.loading = true;

        const res: any = await this.api.commentCreate(
          this.fileId,
          this.form.value
        );
        this.loading = false;
        this.dialogRef.close(true);
      }
    } catch (error) {
      this.loading = false;
      this.app.showErrorMessage('Error');
    }
  }

  onNoClick(): void {
    this._snackBar.dismiss();
    this.dialogRef.close(false);
  }
}
