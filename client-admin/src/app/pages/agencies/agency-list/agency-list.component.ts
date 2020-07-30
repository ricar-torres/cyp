import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { AppService } from '@app/shared/app.service';
import {
  PageEvent,
  MatTableDataSource,
  MatSort,
  MatPaginator,
} from '@angular/material';
import { AgencyService } from '@app/shared/agency.service';
import { MenuRoles } from '@app/models/enums';
import { Route } from '@angular/compiler/src/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-agency-list',
  templateUrl: './agency-list.component.html',
  styleUrls: ['./agency-list.component.css'],
})
export class AgencyListComponent implements OnInit, AfterViewInit {
  editAccess: boolean = false;
  createAccess: boolean = false;
  deleteAccess: boolean = false;
  dataSource;
  displayedColumns: string[] = ['Name', 'Actions'];
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 25, 100];
  pageEvent: PageEvent;
  loading = true;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(
    private app: AppService,
    private fb: FormBuilder,
    private agencyApi: AgencyService,
    private router: Router
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
    this.agencyApi.getAll().subscribe(
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
        this.app.showErrorMessage('Error');
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

  async deleteAgency(id: string) {
    await this.agencyApi.delete(id);
    this.LoadAgencies();
  }

  editAgency(id: number) {
    this.router.navigate(['/home/agency', id]);
  }

  goToNew() {
    this.router.navigate(['/home/agency']);
  }

  doFilter(value: any) {
    this.dataSource.filter = value.toString().trim().toLocaleLowerCase();
  }
}
