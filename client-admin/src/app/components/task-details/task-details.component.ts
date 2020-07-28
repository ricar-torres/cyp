import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import {
  faArchive,
  faFileDownload,
  faCheckCircle,
  faThumbsUp,
  faInbox,
  faTrash,
  faSignature,
  faBookmark,
  faKey,
  faPaperPlane,
  faPen,
  faTasks,
  faAsterisk,
  faComment,
  faUndoAlt,
} from '@fortawesome/free-solid-svg-icons';
import { Task } from '@app/models/task';
import { TaskStatus } from '@app/models/enums';

@Component({
  selector: 'app-task-details',
  templateUrl: './task-details.component.html',
  styleUrls: ['./task-details.component.css'],
})
export class TaskDetailsComponent implements OnInit {
  @Output('onApprove') onApprove = new EventEmitter();
  @Output('onArchive') onArchive = new EventEmitter();
  @Output('onCancel') onCancel = new EventEmitter();
  @Output('onRecover') onRecover = new EventEmitter();
  @Output('onDownloadFile') onDownloadFile = new EventEmitter();
  @Output('onDownloadTemplate') onDownloadTemplate = new EventEmitter();

  @Input()
  task: Task;

  @Input()
  actionButtons: boolean = true;

  faArchive = faArchive;
  faFileDownload = faFileDownload;
  faCheckCircle = faCheckCircle;
  faThumbsUp = faThumbsUp;
  faInbox = faInbox;
  faTrash = faTrash;
  faSignature = faSignature;
  faBookmark = faBookmark;
  faKey = faKey;
  faPaperPlane = faPaperPlane;
  faPen = faPen;
  faTasks = faTasks;
  faAsterisk = faAsterisk;
  faComment = faComment;
  faUndoAlt = faUndoAlt;

  taskStatus = TaskStatus;

  constructor() {}

  ngOnInit(): void {}

  archive(task: Task) {
    this.onArchive.emit(task);
  }

  approve(task: Task) {
    this.onApprove.emit(task);
  }

  cancel(task: Task) {
    this.onCancel.emit(task);
  }

  recover(task: Task) {
    this.onRecover.emit(task);
  }

  downloadFile(task: Task) {
    this.onDownloadFile.emit(task);
  }

  downloadTemplate(task: Task) {
    this.onDownloadTemplate.emit(task);
  }
}
