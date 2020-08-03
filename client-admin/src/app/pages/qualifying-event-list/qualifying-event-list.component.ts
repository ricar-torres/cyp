import { Component, OnInit, ViewChild } from '@angular/core';
import {
  PageEvent,
  MatSort,
  MatPaginator,
  MatDialog,
  MatTableDataSource,
} from '@angular/material';
import { AppService } from '@app/shared/app.service';
import { FormBuilder } from '@angular/forms';

import { Router } from '@angular/router';
import { LanguageService } from '@app/shared/Language.service';
import {
  ConfirmDialogModel,
  ConfirmDialogComponent,
} from '@app/components/confirm-dialog/confirm-dialog.component';
import { QualifyingEventService } from '@app/shared/qualifying-event.service';
import { MenuRoles } from '@app/models/enums';

@Component({
  selector: 'app-qualifying-event-list',
  templateUrl: './qualifying-event-list.component.html',
  styleUrls: ['./qualifying-event-list.component.css'],
})
export class QualifyingEventListComponent implements OnInit {
  editAccess: boolean = false;
  createAccess: boolean = false;
  deleteAccess: boolean = false;
  dataSource;
  displayedColumns: string[] = ['id', 'Name', 'Requirements', 'Actions'];
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 25, 100];
  pageEvent: PageEvent;
  loading = true;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(
    private app: AppService,
    private fb: FormBuilder,
    private qualifyingEventService: QualifyingEventService,
    private router: Router,
    private languageService: LanguageService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    // TODO: Acces
    // this.editAccess = this.app.checkMenuRoleAccess(MenuRoles.QUALIFYING_EVENT_UPDATE);
    // this.createAccess = this.app.checkMenuRoleAccess(MenuRoles.QUALIFYING_EVENT_CREATE);
    // this.deleteAccess = this.app.checkMenuRoleAccess(MenuRoles.QUALIFYING_EVENT_DELETE);
    this.editAccess = true;
    this.createAccess = true;
    this.deleteAccess = true;
  }

  ngAfterViewInit(): void {
    this.LoadQualifyingEvents();
  }

  private LoadQualifyingEvents() {
    this.qualifyingEventService.getAll().subscribe(
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
      },
      () => {
        this.loading = false;
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

  editQualifyingEvent(id: number) {
    this.router.navigate(['/home/qualifyingevent', id]);
  }

  goToNew() {
    this.router.navigate(['/home/qualifyingevent']);
  }

  doFilter(value: any) {
    this.dataSource.filter = value.toString().trim().toLocaleLowerCase();
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
          await this.qualifyingEventService.delete(id);
          this.LoadQualifyingEvents();
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
}
