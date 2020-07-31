import { Component, OnInit, ViewChild, Input } from '@angular/core';
import {
  PageEvent,
  MatSort,
  MatPaginator,
  MatTableDataSource,
} from '@angular/material';
import { AppService } from '@app/shared/app.service';
import { FormBuilder } from '@angular/forms';
import { AgencyService } from '@app/shared/agency.service';
import { Router } from '@angular/router';
import { ChapterServiceService } from '@app/shared/chapter-service.service';

@Component({
  selector: 'app-chapter-list',
  templateUrl: './chapter-list.component.html',
  styleUrls: ['./chapter-list.component.css'],
})
export class ChapterListComponent implements OnInit {
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

  @Input() id: string;
  constructor(
    private app: AppService,
    private fb: FormBuilder,
    private chapterService: ChapterServiceService,
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
    this.chapterService.getByBonafideById(this.id).subscribe(
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
    await this.chapterService.delete(id);
    this.LoadAgencies();
  }

  editAgency(id: number) {
    this.router.navigate(['/home/chapter', id]);
  }

  goToNew() {
    this.router.navigate(['/home/chapter']);
  }

  doFilter(value: any) {
    this.dataSource.filter = value.toString().trim().toLocaleLowerCase();
  }
}
