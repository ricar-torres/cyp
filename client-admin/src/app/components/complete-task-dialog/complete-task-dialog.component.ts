import { Component, OnInit, Inject } from '@angular/core';
import {
  FormGroup,
  FormControl,
  NgForm,
  Validators,
  FormBuilder,
} from '@angular/forms';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';
import {
  faThumbsUp,
  faArchive,
  faFileDownload,
  faCheckCircle,
  faTrash,
  faUndoAlt,
} from '@fortawesome/free-solid-svg-icons';
import { Task } from '@app/models/task';
import { TaskStatus } from '@app/models/enums';
import { ApiService } from '@app/shared/api.service';
import { AppService } from '@app/shared/app.service';

@Component({
  selector: 'app-complete-task-dialog',
  templateUrl: './complete-task-dialog.component.html',
  styleUrls: ['./complete-task-dialog.component.css'],
})
export class CompleteTaskDialogComponent implements OnInit {
  task: Task;
  faThumbsUp = faThumbsUp;
  faArchive = faArchive;
  faFileDownload = faFileDownload;
  faCheckCircle = faCheckCircle;
  faTrash = faTrash;
  faUndoAlt = faUndoAlt;

  loading: boolean = false;
  form: FormGroup;

  taskStatus = TaskStatus;

  constructor(
    public dialogRef: MatDialogRef<CompleteTaskDialogComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: any,
    private fb: FormBuilder,
    private api: ApiService,
    private app: AppService
  ) {
    this.task = this.data.task;
    this.setupForm();
  }

  ngOnInit(): void {}

  async onSubmit() {
    try {
      if (this.form.valid) {
        this.loading = true;
        this.task.status = this.form.controls.status.value;
        this.task.adminComments = this.form.controls.adminComments.value;
        const res: Task = await this.api.taskPatch(this.task.id, this.task);
        this.loading = false;
        this.dialogRef.close(true);
      }
    } catch (error) {
      this.loading = false;
      this.app.showErrorMessage('Error');
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  setupForm() {
    let status: string;

    if (this.data.action == 'APPROVE') status = TaskStatus.completed;
    else if (this.data.action == 'ARCHIVE') status = TaskStatus.archived;
    else if (this.data.action == 'CANCEL') status = TaskStatus.cancelled;
    else if (this.data.action == 'RECOVER') status = TaskStatus.inbox;

    this.form = this.fb.group({
      status: [status, Validators.required],
      adminComments: [null],
    });
  }
}
