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

@Component({
  selector: 'app-agency-search',
  templateUrl: './agency-search.component.html',
  styleUrls: ['./agency-search.component.css'],
})
export class AgencySearchComponent implements OnInit, AfterViewInit {
  reactiveForm: FormGroup;
  editAccess: boolean = false;
  createAccess: boolean = false;
  dataSource;
  displayedColumns: string[] = ['Name', 'Actions'];
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 25, 100];
  pageEvent: PageEvent;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(
    private app: AppService,
    private fb: FormBuilder,
    private AgenciesServices: AgencyService
  ) {}

  ngOnInit(): void {
    this.reactiveForm = this.fb.group({});
    // this.editAccess = this.app.checkMenuRoleAccess(MenuRoles.AGENCIES_UPDATE);
    // this.createAccess = this.app.checkMenuRoleAccess(MenuRoles.AGENCIES_CREATE);
    this.editAccess = true;
    this.createAccess = true;
  }

  ngAfterViewInit(): void {
    this.AgenciesServices.GetAll().subscribe((res) => {
      console.log(res);
      this.dataSource = new MatTableDataSource();
      this.dataSource.data = res;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  onSubmit() {}

  goToNew() {}

  onBack() {}

  setPageSizeOptions(setPageSizeOptionsInput: string) {
    if (setPageSizeOptionsInput) {
      this.pageSizeOptions = setPageSizeOptionsInput
        .split(',')
        .map((str) => +str);
    }
  }

  deleteAgency(Id: number) {
    console.log(Id);
  }

  editAgency(Id: number) {
    console.log(Id);
  }

  doFilter(value: any) {
    this.dataSource.filter = value.toString().trim().toLocaleLowerCase();
  }
}
