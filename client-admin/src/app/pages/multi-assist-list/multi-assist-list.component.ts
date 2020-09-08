import { Component, OnInit, ViewChild, Input } from '@angular/core';
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
import { MultiAssistAPIService } from '@app/shared/MultiAssist.api.service';
import { DocumentationCallComponent } from '@app/components/documentation-call/documentation-call.component';
import { DialogSuccessComponent } from '@app/components/dialog-success/dialog-success.component';
import { GenericSucessModel } from '@app/models/GenericSuccessModel';
import { MultiAssistComponent } from '../multi-assist/multi-assist.component';

@Component({
  selector: 'app-multi-assist-list',
  templateUrl: './multi-assist-list.component.html',
  styleUrls: ['./multi-assist-list.component.css'],
})
export class MultiAssistListComponent implements OnInit {
  loading = true;

  dataSource: any;

  editAccess: boolean;
  createAccess: boolean;
  deteleAccess: boolean;

  @Input()
  clientId: string;

  displayedColumns: string[] = ['id', 'name', 'effectiveDate', 'action'];
  pageSizeOptions: number[] = [5, 10, 25, 100];
  pageEvent: PageEvent;
  pageSize = 10;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    public languageService: LanguageService,
    private router: Router,
    private apiMultiAssistService: MultiAssistAPIService,
    private app: AppService,
    private dialog: MatDialog
  ) {}
  async ngOnInit() {
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
    try {
      await this.loadData();
    } catch (error) {
      this.loading = false;
    } finally {
    }
  }
  async ngAfterViewInit() {
    // try {
    //   await this.loadData();
    // } catch (error) {
    //   this.loading = false;
    // } finally {
    // }
  }

  async loadData() {
    try {
      this.loading = true;
      this.apiMultiAssistService.GetAll().subscribe(
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
        }
      );
    } catch (error) {
      this.loading = false;
    }
  }
  async create(id?: string) {
    if (id == null) {
      id = '0';
    }
    const dialogRef = this.dialog.open(MultiAssistComponent, {
      width: '80%',
      height: '50%',
      data: {
        id: id,
        clientId: this.clientId,
      },
    });
    dialogRef.afterClosed().subscribe(async (dialogResult) => {
      await this.loadData();
      if (dialogResult) {
        this.dialog.open(DialogSuccessComponent, {
          disableClose: true,
          width: '300px',
          height: '200px',
          data: new GenericSucessModel('DOCUMENTATION_CALL.SUCCESS', ''),
        });
      }
    });
  }

  doFilter(value: any) {
    this.dataSource.filter = value.toString().trim().toLocaleLowerCase();
  }
  async deleteConfirm(id: string) {
    const message = await this.languageService.translate
      .get('MULTI_ASSIST_LIST.ARE_YOU_SURE_DELETE')
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
        // console.log(id);
        await this.delete(id);
        await this.loadData();
      }
    });
  }

  async delete(id: string) {
    try {
      await this.apiMultiAssistService.Delete(id);
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
