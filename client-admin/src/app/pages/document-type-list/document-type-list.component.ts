import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { LanguageService } from '@app/shared/Language.service';
import { Router } from '@angular/router';
import {
  PageEvent,
  MatTableDataSource,
  MatSort,
  MatPaginator,
} from '@angular/material';
import { AppService } from '@app/shared/app.service';
import { ApiService } from '@app/shared/api.service';
import { MenuRoles } from '@app/models/enums';

@Component({
  selector: 'app-document-type-list',
  templateUrl: './document-type-list.component.html',
  styleUrls: ['./document-type-list.component.css'],
})
export class DocumentTypeListComponent implements OnInit {
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  length;
  pageSize = 10;
  pageSizeOptions: number[] = [5, 10, 25, 100];
  pageEvent: PageEvent;
  displayedColumns: string[] = [
    'id',
    'name',
    'createdDt',
    'updDt',
    'status',
    'action',
  ];
  loading = false;
  dataSource: any;
  editAccess: boolean;
  createAccess: boolean;

  constructor(
    public languageService: LanguageService,
    private router: Router,
    private api: ApiService,
    private app: AppService
  ) {}

  async ngOnInit() {
    try {
      this.loading = true;
      this.createAccess = this.app.checkMenuRoleAccess(
        MenuRoles.DOCUMENT_TYPES_CREATE
      );
      this.editAccess = this.app.checkMenuRoleAccess(
        MenuRoles.DOCUMENT_TYPES_UPDATE
      );

      const res: any = await this.api.documentTypes();

      this.dataSource = new MatTableDataSource();
      this.dataSource.data = res;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;

      this.loading = false;
    } catch (error) {
      this.loading = false;

      if (error.status != 401) {
        this.app.showErrorMessage('Error');
      }
    }
  }

  goToNew() {
    this.router.navigate(['/home/document-type']);
  }

  goToDetail(id) {
    this.router.navigate(['/home/document-type', id]);
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
