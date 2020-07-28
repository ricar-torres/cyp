import { Component, ViewChild, AfterViewInit, OnInit } from '@angular/core';
import {
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CompleteTaskDialogComponent } from '../../components/complete-task-dialog/complete-task-dialog.component';
import { ApiService } from '../../shared/api.service';
import { AppService } from '../../shared/app.service';
import { saveAs } from 'file-saver';
import {
  faInbox,
  faPaperPlane,
  faStickyNote,
  faPaperclip,
  faThumbsUp,
  faArchive,
  faUser,
  faHome,
  faFileDownload,
  faCheckCircle,
  faTrash,
} from '@fortawesome/free-solid-svg-icons';
import { Task } from '@app/models/task';
import { TaskStatus } from '@app/models/enums';
import { Download } from '@app/models/document';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
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
export class DashboardComponent implements OnInit {
  @ViewChild('inboxTable', { read: MatSort, static: true })
  sortInbox: MatSort;
  @ViewChild('paginatorInbox') paginatorInbox: MatPaginator;

  @ViewChild('sentTable', { read: MatSort, static: true })
  sortSent: MatSort;
  @ViewChild('paginatorSent') paginatorSent: MatPaginator;

  @ViewChild('completedTable', { read: MatSort, static: true })
  sortCompleted: MatSort;
  @ViewChild('paginatorCompleted') paginatorCompleted: MatPaginator;

  @ViewChild('archivedTable', { read: MatSort, static: true })
  sortArchived: MatSort;
  @ViewChild('paginatorArchived') paginatorArchived: MatPaginator;

  download$: Observable<Download>;
  faInbox = faInbox;
  faPaperPlane = faPaperPlane;
  faStickyNote = faStickyNote;
  faPaperclip = faPaperclip;
  faThumbsUp = faThumbsUp;
  faArchive = faArchive;
  faUser = faUser;
  faHome = faHome;
  faFileDownload = faFileDownload;
  faCheckCircle = faCheckCircle;
  faTrash = faTrash;

  dsInbox: any;
  dsSent: any;
  dsCompleted: any;
  dsArchived: any;
  length;
  pageSize = 25;
  pageSizeOptions: number[] = [5, 10, 25, 100];
  pageEventInbox: PageEvent;
  pageEventSent: PageEvent;
  pageEventCompleted: PageEvent;
  pageEventArchived: PageEvent;
  displayedColumns: string[] = [
    'attachment',
    'name',
    'reference1',
    'details',
    'date',
    'adminUserName',
  ];
  expandedElement: any;

  loading = false;
  nextDay: Date;
  inboxBadge: number;
  allTasks: boolean = false;

  isExpansionDetailRow = (i: number, row: Object) => row.hasOwnProperty('id');

  constructor(
    private router: Router,
    private api: ApiService,
    private app: AppService,
    private dialog: MatDialog
  ) {
    this.nextDay = new Date();
    this.nextDay.setDate(this.nextDay.getDate() + 1);
  }

  ngOnInit() {
    this.loadTasks();
  }

  async loadTasks() {
    try {
      this.loading = true;

      const res: Task[] = await this.api.tasks(this.allTasks);

      const inbox = res.filter((x) => x.status == TaskStatus.inbox);
      this.inboxBadge = inbox.length;
      this.dsInbox = new MatTableDataSource();
      this.dsInbox.data = inbox;
      this.dsInbox.paginator = this.paginatorInbox;
      this.dsInbox.sort = this.sortInbox;
      //this.dsInbox.paginator._intl.itemsPerPageLabel = 'Pag.';

      this.dsSent = new MatTableDataSource();
      this.dsSent.data = res.filter((x) => x.status == TaskStatus.sent);
      this.dsSent.paginator = this.paginatorSent;
      this.dsSent.sort = this.sortSent;
      // this.dsSent.paginator._intl.itemsPerPageLabel = 'Pag.';

      this.dsCompleted = new MatTableDataSource();
      this.dsCompleted.data = res.filter(
        (x) => x.status == TaskStatus.completed
      );
      this.dsCompleted.paginator = this.paginatorCompleted;
      this.dsCompleted.sort = this.sortCompleted;
      // this.dsCompleted.paginator._intl.itemsPerPageLabel = 'Pag.';

      this.dsArchived = new MatTableDataSource();
      this.dsArchived.data = res.filter((x) => x.status == TaskStatus.archived);
      this.dsArchived.paginator = this.paginatorArchived;
      this.dsArchived.sort = this.sortArchived;
      // this.dsArchived.paginator._intl.itemsPerPageLabel = 'Pag.';

      this.loading = false;
    } catch (error) {
      this.loading = false;
      this.app.showErrorMessage('Error');
    }
  }

  goToDetail(id) {
    this.router.navigate(['/home/user', id]);
  }

  onApprove(task: Task) {
    const dialogRef = this.dialog.open(CompleteTaskDialogComponent, {
      width: '450px',
      data: { action: 'APPROVE', task: task },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadTasks();
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
    // this.api.documentDownload(document.id, document.ext).subscribe(
    //   (res) => {
    //     saveAs(res, document.name);
    //   },
    //   (err) => {}
    // );
  }

  doInboxFilter(value: any) {
    this.dsInbox.filter = value.toString().trim().toLocaleLowerCase();
  }

  doSentFilter(value: any) {
    this.dsSent.filter = value.toString().trim().toLocaleLowerCase();
  }

  doCompletedFilter(value: any) {
    this.dsCompleted.filter = value.toString().trim().toLocaleLowerCase();
  }

  doArchivedFilter(value: any) {
    this.dsArchived.filter = value.toString().trim().toLocaleLowerCase();
  }

  onPageFired(event) {
    // this.theHttpService.theGetDataFunction(event.pageIndex).subscribe((data)=>{
    // // then you can assign data to your dataSource like so
    // this.dataSource = data
    // })
  }

  setPageSizeOptions(setPageSizeOptionsInput: string) {
    if (setPageSizeOptionsInput) {
      this.pageSizeOptions = setPageSizeOptionsInput
        .split(',')
        .map((str) => +str);
    }
  }
}
