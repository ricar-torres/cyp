import {
  Component,
  OnInit,
  ViewChild,
  Input,
  ChangeDetectorRef,
} from '@angular/core';
import {
  PageEvent,
  MatSort,
  MatPaginator,
  MatTableDataSource,
  MatDialog,
} from '@angular/material';
import { AppService } from '@app/shared/app.service';
import { FormBuilder } from '@angular/forms';
import { AgencyService } from '@app/shared/agency.service';
import { Router } from '@angular/router';
import { ChapterServiceService } from '@app/shared/chapter-service.service';
import {
  ConfirmDialogModel,
  ConfirmDialogComponent,
} from '@app/components/confirm-dialog/confirm-dialog.component';
import { LanguageService } from '@app/shared/Language.service';

@Component({
  selector: 'app-chapter-list',
  templateUrl: './chapter-list.component.html',
  styleUrls: ['./chapter-list.component.css'],
})
export class ChapterListComponent implements OnInit {
  editAccess: boolean = false;
  createAccess: boolean = false;
  deleteAccess: boolean = false;
  dataSource;
  displayedColumns: string[] = [
    'id',
    'name',
    'quota',
    'createdAt',
    'updatedAt',
    'actions',
  ];
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 25, 100];
  pageEvent: PageEvent;
  loading = true;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  @Input() id: string;

  constructor(
    private app: AppService,
    private fb: FormBuilder,
    private chapterService: ChapterServiceService,
    private router: Router,

    private languageService: LanguageService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    //TODO: Acces
    //this.editAccess = this.app.checkMenuRoleAccess(MenuRoles.AGENCIES_UPDATE);
    //this.createAccess = this.app.checkMenuRoleAccess(MenuRoles.AGENCIES_CREATE);
    //this.deleteAccess = this.app.checkMenuRoleAccess(MenuRoles.AGENCIES_DELETE);
    this.editAccess = true;
    this.createAccess = true;
    this.deleteAccess = true;
  }

  ngAfterViewInit(): void {
    this.LoadChapters();
  }

  private LoadChapters() {
    this.chapterService.getByBonafideById(this.id).subscribe(
      (res) => {
        this.loading = true;
        this.dataSource = new MatTableDataSource();
        this.dataSource.data = res;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    );
  }

  onBack() {}

  setPageSizeOptions(setPageSizeOptionsInput: string) {
    if (setPageSizeOptionsInput) {
      this.pageSizeOptions = setPageSizeOptionsInput
        .split(',')
        .map((str) => +str);
    }
  }

  async deleteConfirm(id: string) {
    const message = await this.languageService.translate
      .get('CHAPTER.ARE_YOU_SURE_DELETE')
      .toPromise();

    const title = await this.languageService.translate
      .get('COMFIRMATION')
      .toPromise();

    const dialogData = new ConfirmDialogModel(
      title,
      message,
      true,
      true,
      false
    );

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: dialogData,
    });
    try {
      dialogRef.afterClosed().subscribe(async (dialogResult) => {
        if (dialogResult) {
          console.log(id);
          await this.chapterService.delete(id);
          this.LoadChapters();
        }
      });
    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
        console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    } finally {
      this.loading = false;
    }
  }

  editChapter(chapterId: number) {
    this.router.navigate(['/home/chapter', this.id, chapterId]);
  }

  goToNew() {
    this.router.navigate(['/home/chapter', this.id]);
  }

  doFilter(value: any) {
    this.dataSource.filter = value.toString().trim().toLocaleLowerCase();
  }
}
