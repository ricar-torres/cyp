import { DependantComponent } from './../dependant/dependant.component';
import { DependantsAPIService } from './../../shared/dependants.api.service';
import {
  Component,
  OnInit,
  AfterViewInit,
  ViewChild,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';
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
import { ClientWizardService } from '@app/shared/client-wizard.service';
import { constructor } from 'moment';
import { async } from 'rxjs/internal/scheduler/async';
import { element } from 'protractor';

@Component({
  selector: 'app-dependants-list',
  templateUrl: './dependants-list.component.html',
  styleUrls: ['./dependants-list.component.css'],
})
export class DependantsListComponent implements OnInit, AfterViewInit {
  loading: boolean;

  dataSource: any;
  relations: any[] = [];

  editAccess: boolean;
  createAccess: boolean;
  deteleAccess: boolean;
  @Input() clientId: string | number;
  @Output() isLoadingEvent = new EventEmitter<boolean>();

  displayedColumns: string[] = [
    'id',
    'name',
    // 'phone1',
    'relationName',
    'coverName',
    'action',
  ];
  pageSizeOptions: number[] = [5, 10, 25, 100];
  pageEvent: PageEvent;
  pageSize = 10;

  @Input() fromWizard: boolean;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    public languageService: LanguageService,
    private router: Router,
    private apiDependant: DependantsAPIService,
    private app: AppService,
    private dialog: MatDialog,
    private clientWizard: ClientWizardService
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
      await this.loadData();
      await this.apiDependant.getRelationTypes().toPromise();
    } catch (error) {}
  }

  async loadData() {
    try {
      this.loading = true;
      this.isLoadingEvent.emit(this.loading);
      if (!this.fromWizard) {
        this.apiDependant.getAllByClient(this.clientId).subscribe(
          (data: any) => {
            this.loading = true;
            this.isLoadingEvent.emit(this.loading);
            this.dataSource = new MatTableDataSource();
            this.dataSource.data = data;
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
            this.loading = false;
            this.isLoadingEvent.emit(this.loading);
          },
          (error: any) => {
            this.loading = false;
            this.isLoadingEvent.emit(this.loading);
            if (error.status != 401) {
              //console.error('error', error);
              this.app.showErrorMessage('Error interno');
            }
          },
          () => {
            this.loading = false;
            this.isLoadingEvent.emit(this.loading);
          }
        );
      } else {
        this.loading = true;
        this.isLoadingEvent.emit(this.loading);
        this.dataSource = new MatTableDataSource();
        this.dataSource.data = this.clientWizard.DependantsList;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.loading = false;
        this.isLoadingEvent.emit(this.loading);
      }
    } catch (error) {}
  }

  goToNew(dependantId?: string | number) {
    if (!this.fromWizard) {
      const dialogRef = this.dialog.open(DependantComponent, {
        width: '90%',
        height: '60%',
        minWidth: '90%',
        data: { id: 0, clientId: this.clientId },
      });
      dialogRef.afterClosed().subscribe(async (result) => {
        await this.loadData();
      });
    } else {
      const dialogRef = this.dialog.open(DependantComponent, {
        width: '90%',
        height: '60%',
        minWidth: '90%',
        data: { fromWizard: true },
      });
      dialogRef.afterClosed().subscribe(async (result) => {
        await this.loadData();
      });
    }
  }

  goToDetail(element) {
    if (!this.fromWizard) {
      const dialogRef = this.dialog.open(DependantComponent, {
        width: '90%',
        height: '60%',
        minWidth: '90%',
        data: { id: element.id, clientId: this.clientId },
      });
      dialogRef.afterClosed().subscribe(async (result) => {
        await this.loadData();
      });
    } else {
      const dialogRef = this.dialog.open(DependantComponent, {
        width: '90%',
        height: '60%',
        minWidth: '90%',
        data: { dependantFromWizard: element, fromWizard: true },
      });
      dialogRef.afterClosed().subscribe(async (result) => {
        await this.loadData();
      });
    }
  }

  doFilter(value: any) {
    this.dataSource.filter = value.toString().trim().toLocaleLowerCase();
  }
  async deleteConfirm(element) {
    const message = await this.languageService.translate
      .get('DEPENDANTS_LIST.ARE_YOU_SURE_DELETE')
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
        if (!this.fromWizard) {
          await this.delete(element.id).then(async () => {
            await this.loadData();
          });
        } else {
          var i = this.clientWizard.DependantsList.findIndex(
            (x) => x.id == element.id
          );
          this.clientWizard.DependantsList.splice(i, 1);
          await this.loadData();
        }
      }
    });
  }

  async delete(id: string) {
    try {
      await this.apiDependant.delete(id);
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
