import { Component, OnInit, ViewChild } from '@angular/core';
import {
  PageEvent,
  MatSort,
  MatPaginator,
  MatDialog,
  MatTableDataSource,
} from '@angular/material';
import { LanguageService } from '@app/shared/Language.service';
import { Router } from '@angular/router';
import { CommunicationMethodsAPIService } from '@app/shared/communication-methods.api.service';
import { AppService } from '@app/shared/app.service';
import {
  ConfirmDialogModel,
  ConfirmDialogComponent,
} from '@app/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-retirement-list',
  templateUrl: './retirement-list.component.html',
  styleUrls: ['./retirement-list.component.css'],
})
export class RetirementListComponent implements OnInit {
  loading = false;

  dataSource: any;

  editAccess: boolean;
  createAccess: boolean;
  deteleAccess: boolean;

  displayedColumns: string[] = [
    'id',
    'name',
    // 'updDt',
    'action',
  ];
  pageSizeOptions: number[] = [5, 10, 25, 100];
  pageEvent: PageEvent;
  pageSize = 10;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    public languageService: LanguageService,
    private router: Router,
    private apiCommunicationMethod: CommunicationMethodsAPIService,
    private app: AppService,
    private dialog: MatDialog
  ) {}
  ngOnInit(): void {
    //TODO: Get access priviliges
    this.createAccess = true;
    this.editAccess = true;
    this.deteleAccess = true;
    // this.createAccess = this.app.checkMenuRoleAccess(
    //   MenuRoles.DOCUMENT_TYPES_CREATE
    // );
    // this.editAccess = this.app.checkMenuRoleAccess(
    //   MenuRoles.DOCUMENT_TYPES_UPDATE
    // );
  }
  async ngAfterViewInit() {
    try {
      await this.loadCommunicationMethods();
    } catch (error) {
      this.loading = false;
    } finally {
    }
  }

  async loadCommunicationMethods() {
    try {
      this.loading = true;
      this.dataSource = new MatTableDataSource();
      await this.apiCommunicationMethod.getAll().subscribe(
        (data: any) => {
          this.dataSource.data = data;
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;

          this.loading = false;
        },
        (error: any) => {
          this.loading = false;

          if (error.status != 401) {
            console.error('error', error);
            this.app.showErrorMessage('Error interno');
          }
        }
      );
    } catch (error) {
      this.loading = false;
    }
  }
  goToNew() {
    this.router.navigate(['/home/communication-method', 0]);
  }

  goToDetail(id) {
    this.router.navigate(['/home/communication-method', id]);
  }

  doFilter(value: any) {
    this.dataSource.filter = value.toString().trim().toLocaleLowerCase();
  }
  async deleteConfirm(id: string) {
    const message = await this.languageService.translate
      .get('COMMUNICATION_METHOD_LIST.ARE_YOU_SURE_DELETE')
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

    dialogRef.afterClosed().subscribe(async (dialogResult) => {
      if (dialogResult) {
        console.log(id);
        await this.delete(id);
        await this.loadCommunicationMethods();
      }
    });
  }

  async delete(id: string) {
    try {
      await this.apiCommunicationMethod.delete(id);
    } catch (error) {}
  }

  setPageSizeOptions(setPageSizeOptionsInput: string) {
    if (setPageSizeOptionsInput) {
      this.pageSizeOptions = setPageSizeOptionsInput
        .split(',')
        .map((str) => +str);
    }
  }
}
