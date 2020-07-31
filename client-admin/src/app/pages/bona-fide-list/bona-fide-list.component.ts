import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import {
  PageEvent,
  MatSort,
  MatPaginator,
  MatTableDataSource,
} from '@angular/material';
import { AppService } from '@app/shared/app.service';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { bonaFideservice } from '@app/shared/bonafide.service';
import { MenuRoles } from '@app/models/enums';

@Component({
  selector: 'app-bona-fide-list',
  templateUrl: './bona-fide-list.component.html',
  styleUrls: ['./bona-fide-list.component.css'],
})
export class BonaFideListComponent implements OnInit, AfterViewInit {
  editAccess: boolean = false;
  createAccess: boolean = false;
  deleteAccess: boolean = false;
  dataSource;
  displayedColumns: string[] = ['Name', 'Code', 'Siglas', 'Phone', 'Actions'];
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 25, 100];
  pageEvent: PageEvent;
  loading = true;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(
    private app: AppService,
    private fb: FormBuilder,
    private bonafidesService: bonaFideservice,
    private router: Router
  ) {}

  ngOnInit(): void {
    //TODO: Acces
    // this.editAccess = this.app.checkMenuRoleAccess(MenuRoles.BONAFIDE_UPDATE);
    // this.createAccess = this.app.checkMenuRoleAccess(MenuRoles.BONAFIDE_CREATE);
    // this.deleteAccess = this.app.checkMenuRoleAccess(MenuRoles.BONAFIDE_DELETE);
    this.editAccess = true;
    this.createAccess = true;
    this.deleteAccess = true;
  }

  ngAfterViewInit(): void {
    this.LoadAgencies();
  }

  private LoadAgencies() {
    this.bonafidesService.getAll().subscribe(
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

  async deleteBonafide(id: string) {
    await this.bonafidesService.delete(id);
    this.LoadAgencies();
  }

  editBonafide(id: number) {
    this.router.navigate(['/home/bonafide', id]);
  }

  goToNew() {
    this.router.navigate(['/home/bonafide']);
  }

  doFilter(value: any) {
    this.dataSource.filter = value.toString().trim().toLocaleLowerCase();
  }
}
