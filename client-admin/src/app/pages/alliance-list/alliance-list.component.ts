import { Component, OnInit, Input, ViewChild } from '@angular/core';
import {
  MatSort,
  PageEvent,
  MatPaginator,
  MatDialog,
  MatTableDataSource,
} from '@angular/material';
import { AppService } from '@app/shared/app.service';
import { FormBuilder } from '@angular/forms';
import { AgencyService } from '@app/shared/agency.service';
import { Router } from '@angular/router';
import { LanguageService } from '@app/shared/Language.service';
import {
  ConfirmDialogModel,
  ConfirmDialogComponent,
} from '@app/components/confirm-dialog/confirm-dialog.component';
import { AlliancesService } from '@app/shared/alliances.service';
import { AllianceWizardComponent } from '../alliance-wizard/alliance-wizard.component';

@Component({
  selector: 'app-alliance-list',
  templateUrl: './alliance-list.component.html',
  styleUrls: ['./alliance-list.component.css'],
})
export class AllianceListComponent implements OnInit {
  @Input() clientId: string;
  editAccess: boolean = false;
  createAccess: boolean = false;
  deleteAccess: boolean = false;
  deceased: boolean = false;
  dataSource;
  displayedColumns: string[] = [
    'id',
    'cover',
    'qualifyingEvent',
    'allianceType',
    'startDate',
    'endDate',
    'elegibleDate',
    'createdAt',
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
    private allianceService: AlliancesService,
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
    this.LoadAgencies();
  }

  private LoadAgencies() {
    this.allianceService.getAll(this.clientId).subscribe(
      (res) => {
        //console.log(res);
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

  editAlliance(alliance) {
    const dialogRef = this.dialog.open(AllianceWizardComponent, {
      width: '70%',
      height: '70%',
      disableClose: true,
      data: { alliance: alliance, clientid: this.clientId },
    });
    dialogRef.afterClosed().subscribe((result) => {
      this.LoadAgencies();
    });
  }

  goToNew() {
    const dialogRef = this.dialog.open(AllianceWizardComponent, {
      width: '70%',
      height: '70%',
      disableClose: true,
      data: { clientid: this.clientId },
    });
    dialogRef.afterClosed().subscribe((result) => {
      this.LoadAgencies();
    });
  }

  doFilter(value: any) {
    this.dataSource.filter = value.toString().trim().toLocaleLowerCase();
  }

  async deleteConfirm(id: string) {
    const message = await this.languageService.translate
      .get('AGENCY.ARE_YOU_SURE_DELETE')
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
          await this.allianceService.delete(id).then(() => {
            this.LoadAgencies();
          });
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
