import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
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
import { MenuRoles } from '@app/models/enums';
import { ClientService } from '@app/shared/client.service';
import { ClientWizardComponent } from '../client-wizard/client-wizard.component';
import { ClientWizardService } from '@app/shared/client-wizard.service';

@Component({
  selector: 'app-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.css'],
})
export class ClientListComponent implements OnInit, OnDestroy {
  editAccess: boolean = false;
  createAccess: boolean = false;
  deleteAccess: boolean = false;
  dataSource;
  displayedColumns: string[] = [
    'id',
    'name',
    //'ssn',
    //'gender',
    'phone1',
    //'phone2',
    'contract',
    'email',
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
    private router: Router,
    private languageService: LanguageService,
    private dialog: MatDialog,
    private clientService: ClientService,
    private wizardService: ClientWizardService
  ) {}
  ngOnDestroy(): void {
    this.dataSource = undefined;
  }

  ngOnInit(): void {
    //TODO: Acces
    // this.editAccess = this.app.checkMenuRoleAccess(MenuRoles.CLIENT_UPDATE);
    // this.createAccess = this.app.checkMenuRoleAccess(MenuRoles.CLIENT_CREATE);
    // this.deleteAccess = this.app.checkMenuRoleAccess(MenuRoles.CLIENT_DELETE);
    this.editAccess = true;
    this.createAccess = true;
    this.deleteAccess = true;
  }

  ngAfterViewInit(): void {
    this.LoadAgencies();
  }

  private LoadAgencies() {
    this.clientService.getAll().subscribe(
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

  editClient(id: number) {
    this.router.navigate(['/home/client', id]);
  }

  goToNew() {
    this.wizardService.resetFormGroups();
    const dialogRef = this.dialog.open(ClientWizardComponent, {
      width: '95%',
      height: '95%',
      data: {},
    });
    dialogRef.afterClosed().subscribe((result) => {});
  }

  doFilter(value: any) {
    this.dataSource.filter = value.toString().trim().toLocaleLowerCase();
  }

  async deleteConfirm(id: string) {
    const message = await this.languageService.translate
      .get('CLIENTS.ARE_YOU_SURE_DELETE')
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
          await this.clientService.delete(id);
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
}
