import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import {
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';
import { saveAs } from 'file-saver';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { NewTaskDialogComponent } from '../../components/new-task-dialog/new-task-dialog.component';
import { CommentDialogComponent } from '../../components/comment-dialog/comment-dialog.component';
import { DocumentDialogComponent } from '../../components/document-dialog/document-dialog.component';

import {
  faInbox,
  faPaperPlane,
  faTasks,
  faFile,
  faComments,
  faPaperclip,
  faThumbsUp,
  faArchive,
  faFileDownload,
  faCheckCircle,
  faFilePdf,
  faFileImage,
  faGripHorizontal,
  faList,
  faLink,
  faDownload,
  faEdit,
} from '@fortawesome/free-solid-svg-icons';
import { ApiService } from '../../shared/api.service';
import { AppService } from '../../shared/app.service';
import { PERMISSION, MenuRoles } from '@app/models/enums';
import { Task } from '@app/models/task';
import { CompleteTaskDialogComponent } from '@app/components/complete-task-dialog/complete-task-dialog.component';
import { MatSlideToggleChange } from '@angular/material';
import { Download } from '@app/models/document';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-file',
  templateUrl: './file.component.html',
  styleUrls: ['./file.component.css'],
  animations: [
    trigger('detailExpand', [
      state(
        'collapsed',
        style({ height: '0px', minHeight: '0', visibility: 'hidden' })
      ),
      state('expanded', style({ height: '*', visibility: 'visible' })),
      transition(
        'expanded <=> collapsed',
        animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')
      ),
    ]),
  ],
})
export class FileComponent implements OnInit {
  @ViewChild('tasksTable', { read: MatSort, static: true })
  sortTasks: MatSort;
  @ViewChild('paginatorTasks') paginatorTasks: MatPaginator;

  @ViewChild('docsTable', { read: MatSort, static: true })
  sortDocuments: MatSort;
  @ViewChild('paginatorDocuments') paginatorDocuments: MatPaginator;

  @ViewChild('linkedFilesTable', { read: MatSort, static: true })
  sortLinkedFiles: MatSort;
  @ViewChild('paginatorLinkedFiles') paginatorLinkedFiles: MatPaginator;

  @ViewChild('commentsTable', { read: MatSort, static: true })
  sortComments: MatSort;
  @ViewChild('paginatorComments') paginatorComments: MatPaginator;

  download$: Observable<Download>;
  faTasks = faTasks;
  faFile = faFile;
  faComments = faComments;
  faPaperclip = faPaperclip;
  faThumbsUp = faThumbsUp;
  faArchive = faArchive;
  faFileDownload = faFileDownload;
  faCheckCircle = faCheckCircle;
  faInbox = faInbox;
  faPaperPlane = faPaperPlane;
  faFilePdf = faFilePdf;
  faFileImage = faFileImage;
  faGripHorizontal = faGripHorizontal;
  faList = faList;
  faLink = faLink;
  faDownload = faDownload;
  faEdit = faEdit;

  id: string;
  loading: boolean = false;

  pageSizeTasks = 25;
  pageSizeOptionsTasks: number[] = [5, 10, 25, 100];
  pageEventTasks: PageEvent;

  pageSizeDocuments = 25;
  pageSizeLinkedFiles = 25;
  pageSizeComments = 25;
  pageSizeOptionsDocuments: number[] = [5, 10, 25, 100];
  pageSizeOptionsLinkedFiles: number[] = [5, 10, 25, 100];
  pageSizeOptionsComments: number[] = [5, 10, 25, 100];
  pageEventDocuments: PageEvent;
  pageEventLinkedFiles: PageEvent;
  pageEventComments: PageEvent;

  dcTasks: string[] = [
    'documentId',
    'name',
    'details',
    'createDt',
    'updDt',
    'status',
  ];
  dcDocuments: string[] = [
    'tag',
    'createdUser',
    'updateUser',
    'createDt',
    'updDt',
    'origin',
    'delFlag',
    'download',
    'edit',
  ];
  dcLinkedFiles: string[] = ['relationKey', 'reference1', 'fullName', 'type'];
  dcComments: string[] = ['text', 'createdUser', 'createDt'];

  expandedElement: any;

  dsTasks: any;
  dsDocuments: any;
  dsLinkedFiles: any;
  dsComments: any;

  nextDay: Date;

  FILE: any = {};

  DOCS: any = [];

  docPermissions: PERMISSION = {
    read: this.app.checkMenuRoleAccess(MenuRoles.DOCUMENTS),
    create: this.app.checkMenuRoleAccess(MenuRoles.DOCUMENTS_CREATE),
    update: this.app.checkMenuRoleAccess(MenuRoles.DOCUMENTS_UPDATE),
    delete: this.app.checkMenuRoleAccess(MenuRoles.DOCUMENTS_DELETE),
    upload: false,
  };
  taskPermissions: PERMISSION = {
    read: this.app.checkMenuRoleAccess(MenuRoles.TASKS),
    create: this.app.checkMenuRoleAccess(MenuRoles.TASKS_CREATE),
    update: this.app.checkMenuRoleAccess(MenuRoles.TASKS_UPDATE),
    delete: this.app.checkMenuRoleAccess(MenuRoles.TASKS_DELETE),
    upload: false,
  };
  commentPermissions: PERMISSION = {
    read: this.app.checkMenuRoleAccess(MenuRoles.COMMENTS),
    create: this.app.checkMenuRoleAccess(MenuRoles.COMMENTS_CREATE),
    update: this.app.checkMenuRoleAccess(MenuRoles.COMMENTS_UPDATE),
    delete: this.app.checkMenuRoleAccess(MenuRoles.COMMENTS_DELETE),
    upload: false,
  };

  fabMenuButtons = {
    visible: false,
    buttons: [],
  };

  documentsView = 'list';

  onlyActiveDocument = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog,
    private api: ApiService,
    private app: AppService,
    private cdRef: ChangeDetectorRef
  ) {
    this.router.routeReuseStrategy.shouldReuseRoute = function () {
      return false;
    };
  }

  async ngOnInit() {
    try {
      this.loading = true;

      this.id = this.route.snapshot.paramMap.get('id');
      this.FILE = await this.api.file(this.id);

      this.loading = false;

      this.loadLinkedFiles(this.FILE.relations);
      await this.loadDocuments(true);
      await this.loadComments();
      await this.loadTasks();

      this.setupFabButton();

      this.cdRef.detectChanges();
    } catch (error) {
      console.error(error);
      this.app.showErrorMessage('Error');
    }
  }

  //#region "Documents"

  async loadDocuments(showLoading: boolean = true) {
    try {
      if (showLoading) {
        this.loading = true;
      }

      const res: any = await this.api.fileDocuments(
        this.id,
        this.onlyActiveDocument
      );
      this.DOCS = res.map((e) => {
        return {
          id: e.id,
          tag: e.documentType ? e.documentType.name : 'N/A',
          createdUser: e.createdUser ? e.createdUser.fullName : null,
          updateUser: e.updateUser ? e.updateUser.fullName : null,
          createDt: e.createDt,
          updDt: e.updDt,
          ext: e.document ? e.document.ext : null,
          name: e.document ? e.document.name : null,
          delFlag: e.delFlag,
          document: e.document,
          origin: e.origin,
        };
      });

      this.dsDocuments = new MatTableDataSource();
      this.dsDocuments.data = this.DOCS;
      this.dsDocuments.paginator = this.paginatorDocuments;
      this.dsDocuments.sort = this.sortDocuments;
      this.dsDocuments.paginator._intl.itemsPerPageLabel = '';

      if (showLoading) {
        this.loading = false;
      }
    } catch (error) {
      if (showLoading) {
        this.loading = false;
      }
    }
  }

  doDocumentFilter(value: any) {
    this.dsDocuments.filter = value.toString().trim().toLocaleLowerCase();
  }

  toggleDocView() {
    this.documentsView == 'grid'
      ? (this.documentsView = 'list')
      : (this.documentsView = 'grid');
    this.loadDocuments();
  }

  viewDocument(item) {
    const dialogRef = this.dialog.open(DocumentDialogComponent, {
      width: '450px',
      data: { FileDocument: item, type: 'APPROVE', task: {} },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadDocuments(true);
      }
    });
  }

  onFilesLoaded(files: FormData) {
    this.loading = true;
    this.api.fileDocumentsCreate(this.FILE.id, files).subscribe(
      (res: any) => {
        if (res.progress) {
          this.loading = true;
        } else {
          this.loading = false;
          this.loadDocuments(true);
        }
      },
      (e) => {
        this.loading = false;
      }
    );
  }

  onActiveToggleChange($event: MatSlideToggleChange) {
    this.onlyActiveDocument = $event.checked;
    this.loadDocuments();
  }

  //#endregion

  //#region Comments

  doCommentFilter(value: any) {
    this.dsComments.filter = value.toString().trim().toLocaleLowerCase();
  }

  newComment() {
    const dialogRef = this.dialog.open(CommentDialogComponent, {
      width: '450px',
      data: { file: this.FILE, comment: null },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadComments();
      }
    });
  }

  viewComment(comment) {
    const dialogRef = this.dialog.open(CommentDialogComponent, {
      width: '450px',
      data: { file: this.FILE, comment: comment },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadComments();
      }
    });
  }

  async loadComments() {
    try {
      this.loading = true;

      const res: any = await this.api.comments(this.id);

      this.dsComments = new MatTableDataSource();
      this.dsComments.data = res;
      this.dsComments.paginator = this.paginatorComments;
      this.dsComments.sort = this.sortComments;
      this.dsComments.paginator._intl.itemsPerPageLabel = '';

      this.loading = false;
    } catch (error) {
      this.loading = false;
    }
  }

  //#endregion

  //#region Task

  doTaskFilter(value: any) {
    this.dsTasks.filter = value.toString().trim().toLocaleLowerCase();
  }

  onApprove(task: Task) {
    const dialogRef = this.dialog.open(CompleteTaskDialogComponent, {
      width: '450px',
      data: { action: 'APPROVE', task: task },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadTasks();
        this.loadDocuments();
      }
    });
  }

  onArchive(task: Task) {
    const dialogRef = this.dialog.open(CompleteTaskDialogComponent, {
      width: '450px',
      data: { action: 'ARCHIVE', task: task },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadTasks();
      }
    });
  }

  onCancel(task: Task) {
    const dialogRef = this.dialog.open(CompleteTaskDialogComponent, {
      width: '450px',
      data: { action: 'CANCEL', task: task },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadTasks();
      }
    });
  }

  onRecover(task: Task) {
    const dialogRef = this.dialog.open(CompleteTaskDialogComponent, {
      width: '450px',
      data: { action: 'RECOVER', task: task },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadTasks();
      }
    });
  }

  onDownload(document) {
    this.download$ = this.api.documentDownload(document.id, document.name);

    // this.api.documentDownload(document.id, document.name).subscribe(
    //   (res: any) => {
    //     console.log(res);
    //     if (res.progress) {
    //       this.loading = true;
    //     } else {
    //       this.loading = false;
    //       //saveAs(res.data, document.name);
    //     }
    //   },
    //   (err) => {}
    // );
  }

  async loadTasks() {
    try {
      this.loading = true;
      const res: Task[] = await this.api.tasksByFile(this.FILE.key);
      this.dsTasks = new MatTableDataSource();
      this.dsTasks.data = res;
      this.dsTasks.paginator = this.paginatorTasks;
      this.dsTasks.sort = this.sortTasks;
      this.dsTasks.paginator._intl.itemsPerPageLabel = '';

      this.loading = false;
    } catch (error) {
      this.loading = false;
    }
  }

  newTask() {
    const dialogRef = this.dialog.open(NewTaskDialogComponent, {
      width: '450px',
      data: { file: this.FILE, task: {} },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadTasks();
      }
    });
  }

  //#endregion

  //#region Linked Files

  loadLinkedFiles(data) {
    this.loading = true;

    const files = data.map((e) => {
      return {
        id: e.relationFile.id,
        key: e.relationKey,
        fullName: e.relationFile.fullName,
        reference1: e.relationFile.reference1,
        type: e.relationFile.type,
      };
    });

    this.dsLinkedFiles = new MatTableDataSource();
    this.dsLinkedFiles.data = files;
    this.dsLinkedFiles.paginator = this.paginatorLinkedFiles;
    this.dsLinkedFiles.sort = this.sortLinkedFiles;
    this.dsLinkedFiles.paginator._intl.itemsPerPageLabel = '';

    this.loading = false;
  }

  goToRelatedFile(id) {
    this.router.navigate(['/home/file', id]);
  }

  //#endregion

  //#region FabMenu

  setupFabButton() {
    if (this.taskPermissions.create) {
      this.fabMenuButtons.buttons.push({
        icon: 'list',
        tooltip: 'Do some headline here',
      });
    }

    if (this.commentPermissions.create) {
      this.fabMenuButtons.buttons.push({
        icon: 'chat',
        tooltip: 'Do some timeline here',
      });
    }

    this.fabMenuButtons.visible =
      this.fabMenuButtons.buttons.length > 0 && !this.FILE.delFlag
        ? true
        : false;
  }

  onSpeedDialFabClicked(btn: { icon: string }) {
    if (btn.icon == 'list') {
      this.newTask();
    } else {
      if (btn.icon == 'chat') {
        this.newComment();
      }
    }
  }

  //#endregion
}
