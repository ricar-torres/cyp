import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { ApiService } from '../../shared/api.service';
import { AppService } from '../../shared/app.service';
import { MenuRoles } from '@app/models/enums';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
})
export class UserListComponent implements AfterViewInit {
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  createAccess: boolean;
  length;
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 25, 100];

  pageEvent: PageEvent;

  displayedColumns: string[] = [
    'id',
    'userName',
    'fullName',
    'status',
    'action',
  ];
  loading = false;

  dataSource: any;

  constructor(
    private router: Router,
    private api: ApiService,
    private app: AppService
  ) {}

  ngAfterViewInit() {
    this.loading = true;
    this.createAccess = this.app.checkMenuRoleAccess(MenuRoles.USERS_CREATE);

    this.api.users().subscribe(
      (res: any) => {
        this.dataSource = new MatTableDataSource();
        this.dataSource.data = res;
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
  }

  goToNew() {
    this.router.navigate(['/home/user']).then((e) => {
      if (e) {
      } else {
      }
    });
  }

  goToDetail(id) {
    this.router.navigate(['/home/user', id]).then((e) => {
      if (e) {
      } else {
      }
    });
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
