import { DependantComponent } from './../dependant/dependant.component';
import { DependantsAPIService } from './../../shared/dependants.api.service';
import {
  Component,
  OnInit,
  AfterViewInit,
  ViewChild,
  Input,
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

@Component({
  selector: 'app-dependants-list',
  templateUrl: './dependants-list.component.html',
  styleUrls: ['./dependants-list.component.css'],
})
export class DependantsListComponent implements OnInit, AfterViewInit {
  loading = true;

  dataSource: any;
  relations: any[] = [];

  editAccess: boolean;
  createAccess: boolean;
  deteleAccess: boolean;
  @Input() clientId: string | number;

  displayedColumns: string[] = [
    'id',
    'name',
    'phone1',
    'relationName',
    'coverName',
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
    private apiDependant: DependantsAPIService,
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
      await this.loadData();
      await this.apiDependant.getRelationTypes();
    } catch (error) {
      this.loading = false;
    } finally {
    }
  }

  async loadData() {
    this.loading = true;
    if (this.clientId) {
      await this.apiDependant.getAllByClient(this.clientId).subscribe(
        (data: any) => {
          this.dataSource = new MatTableDataSource();
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
        },
        () => {
          this.loading = false;
        }
      );
    } else {
      await this.apiDependant.getAllByClient('1').subscribe(
        (data: any) => {
          this.dataSource = new MatTableDataSource();
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
        },
        () => {
          this.loading = false;
        }
      );
    }
  }
  goToNew(dependantId?: string | number) {
    const dialogRef = this.dialog.open(DependantComponent, {
      width: '95%',
      height: '95%',
      minWidth: '95%',
      data: { id: 0, clientId: 1 },
    });
    dialogRef.afterClosed().subscribe(async (result) => {
      await this.loadData();
    });
  }

  goToDetail(id) {
    const dialogRef = this.dialog.open(DependantComponent, {
      width: '95%',
      height: '95%',
      minWidth: '95%',
      data: { id: id, clientId: 1 },
    });
    dialogRef.afterClosed().subscribe(async (result) => {
      await this.loadData();
    });
  }

  doFilter(value: any) {
    this.dataSource.filter = value.toString().trim().toLocaleLowerCase();
  }
  async deleteConfirm(id: string) {
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
        await this.delete(id);
        await this.loadData();
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
