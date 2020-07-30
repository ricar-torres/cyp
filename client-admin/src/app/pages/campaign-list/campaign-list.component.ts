import { map } from 'rxjs/operators';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { LanguageService } from '@app/shared/Language.service';
import { Router } from '@angular/router';
import { ApiService } from '@app/shared/api.service';
import { AppService } from '@app/shared/app.service';
import { Campaign } from '@app/models/Campaign';

@Component({
  selector: 'app-campaign-list',
  templateUrl: './campaign-list.component.html',
  styleUrls: ['./campaign-list.component.css'],
})
export class CampaignListComponent implements AfterViewInit {
  loading = false;
  pageSize = 10;
  dataSource: any;

  editAccess: boolean = true;
  createAccess: boolean = true;

  displayedColumns: string[] = [
    'id',
    'name',
    'origin',
    // 'updDt',
    // 'status',
    //'action',
  ];
  pageSizeOptions: number[] = [5, 10, 25, 100];
  pageEvent: PageEvent;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    public languageService: LanguageService,
    private router: Router,
    private api: ApiService,
    private app: AppService
  ) {}
  ngAfterViewInit() {
    try {
      this.loading = true;
      //TODO: Get access priviliges
      // this.createAccess = this.app.checkMenuRoleAccess(
      //   MenuRoles.DOCUMENT_TYPES_CREATE
      // );
      // this.editAccess = this.app.checkMenuRoleAccess(
      //   MenuRoles.DOCUMENT_TYPES_UPDATE
      // );
      // var data: string = [];
      this.api.getAllCampaigns().subscribe(
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
    } finally {
      var source = this.dataSource.data;
    }
  }

  // async ngOnInit() {
  //   try {
  //     this.loading = true;
  //     //TODO: Get access priviliges
  //     // this.createAccess = this.app.checkMenuRoleAccess(
  //     //   MenuRoles.DOCUMENT_TYPES_CREATE
  //     // );
  //     // this.editAccess = this.app.checkMenuRoleAccess(
  //     //   MenuRoles.DOCUMENT_TYPES_UPDATE
  //     // );
  //     // var data: string = [];
  //     await this.api.getAllCampaigns().subscribe(
  //       (data: any) => {
  //         this.dataSource = new MatTableDataSource();
  //         this.dataSource.data = data;
  //         this.dataSource.paginator = this.paginator;
  //         this.dataSource.sort = this.sort;
  //         this.loading = false;
  //       },
  //       (error: any) => {
  //         this.loading = false;

  //         if (error.status != 401) {
  //           console.error('error', error);
  //           this.app.showErrorMessage('Error interno');
  //         }
  //       }
  //     );
  //   } catch (error) {
  //     this.loading = false;
  //   }
  // }
  goToNew() {
    //this.router.navigate(['/home/document-type']);
  }

  goToDetail(id) {
    //this.router.navigate(['/home/document-type', id]);
  }

  doFilter(value: any) {
    this.dataSource.filter = value.toString().trim().toLocaleLowerCase();
  }

  onPageFired(event) {
    // this.theHttpService.theGetDataFunction(event.pageIndex).subscribe((data)=>{
    // // then you can assign data to your dataSource like so
    // this.dataSource = data
    // })
  }

  setPageSizeOptions(setPageSizeOptionsInput: string) {
    if (setPageSizeOptionsInput) {
      this.pageSizeOptions = setPageSizeOptionsInput
        .split(',')
        .map((str) => +str);
    }
  }
}
