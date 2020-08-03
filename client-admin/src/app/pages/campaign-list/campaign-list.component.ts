import { element } from 'protractor';
import { map } from 'rxjs/operators';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { LanguageService } from '@app/shared/Language.service';
import { Router } from '@angular/router';
import { ApiService } from '@app/shared/api.service';
import { AppService } from '@app/shared/app.service';
import { CampaignApiSerivce } from '@app/shared/campaign.api.service';
import {
  ConfirmDialogModel,
  ConfirmDialogComponent,
} from '@app/components/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'app-campaign-list',
  templateUrl: './campaign-list.component.html',
  styleUrls: ['./campaign-list.component.css'],
})
export class CampaignListComponent implements OnInit, AfterViewInit {
  loading = false;

  dataSource: any;

  editAccess: boolean;
  createAccess: boolean;
  deteleAccess: boolean;

  displayedColumns: string[] = [
    'id',
    'name',
    'origin',
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
    private campaignApi: CampaignApiSerivce,
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
      await this.loadCampaigns();
    } catch (error) {
      this.loading = false;
    } finally {
    }
  }

  async loadCampaigns() {
    try {
      this.loading = true;

      // var data: string = [];
      this.dataSource = new MatTableDataSource();
      await this.campaignApi.getAll().subscribe(
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
    } catch (error) {}
  }
  goToNew() {
    this.router.navigate(['/home/campaigns', 0]);
  }

  goToDetail(id) {
    this.router.navigate(['/home/campaigns', id]);
  }

  doFilter(value: any) {
    this.dataSource.filter = value.toString().trim().toLocaleLowerCase();
  }
  async deleteConfirm(id: string) {
    const message = await this.languageService.translate
      .get('CAMPAIGN_LIST.ARE_YOU_SURE_DELETE')
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
        await this.loadCampaigns();
      }
    });
  }

  async delete(id: string) {
    try {
      await this.campaignApi.delete(id);
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
