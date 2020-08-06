import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import {
  PageEvent,
  MatSort,
  MatPaginator,
  MatTableDataSource,
  MatDialog,
} from '@angular/material';
import { AppService } from '@app/shared/app.service';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { bonaFideservice } from '@app/shared/bonafide.service';
import { MenuRoles } from '@app/models/enums';
import { LanguageService } from '@app/shared/Language.service';
import {
  ConfirmDialogModel,
  ConfirmDialogComponent,
} from '@app/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-bona-fide-list',
  templateUrl: './bona-fide-list.component.html',
  styleUrls: ['./bona-fide-list.component.css'],
})
export class BonaFideListComponent implements OnInit, AfterViewInit {
  editAccess: boolean = false;
  createAccess: boolean = false;
  deleteAccess: boolean = false;
  dataSource;
  displayedColumns: string[] = [
    'id',
    'name',
    'code',
    'siglas',
    'phone',
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
  constructor(
    private app: AppService,
    private fb: FormBuilder,
    private bonafidesService: bonaFideservice,
    private router: Router,
    private languageService: LanguageService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    //TODO: Acces
    // this.editAccess = this.app.checkMenuRoleAccess(MenuRoles.BONAFIDE_UPDATE);
    // this.createAccess = this.app.checkMenuRoleAccess(MenuRoles.BONAFIDE_CREATE);
    // this.deleteAccess = this.app.checkMenuRoleAccess(MenuRoles.BONAFIDE_DELETE);
    this.editAccess = true;
    this.createAccess = true;
    this.deleteAccess = true;
  }

  ngAfterViewInit(): void {
    this.LoadAgencies();
  }

  private LoadAgencies() {
    this.bonafidesService.getAll().subscribe(
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
        console.log('error', error);
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

  async deleteConfirm(id: string) {
    const message = await this.languageService.translate
      .get('BONAFIDE.ARE_YOU_SURE_DELETE')
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
          await this.bonafidesService.delete(id);
          this.LoadAgencies();
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

  editBonafide(id: number) {
    this.router.navigate(['/home/bonafide', id]);
  }

  goToNew() {
    this.router.navigate(['/home/bonafide']);
  }

  doFilter(value: any) {
    this.dataSource.filter = value.toString().trim().toLocaleLowerCase();
  }
}
