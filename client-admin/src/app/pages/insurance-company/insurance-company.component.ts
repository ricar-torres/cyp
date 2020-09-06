import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import {PageEvent, MatPaginator} from '@angular/material/paginator';
import { Router } from "@angular/router";
import { EventEmitter } from 'protractor';
import { HttpClient } from '@angular/common/http';
import { LoaderService } from '../../shared/loader.service';
import { HealthPlanService } from '../../shared/health-plan.service';
import { AppService } from '../../shared/app.service';
import { LanguageService } from '@app/shared/Language.service';
import {
  ConfirmDialogModel,
  ConfirmDialogComponent,
} from '@app/components/confirm-dialog/confirm-dialog.component';

import {
  MatDialog,
} from '@angular/material';

@Component({
  selector: 'app-insurance-company',
  templateUrl: './insurance-company.component.html',
  styleUrls: ['./insurance-company.component.css']
})
 

export class InsuranceCompanyComponent implements AfterViewInit {

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  //  @Output()
  //  page: EventEmitter<PageEvent>

  // MatPaginator Inputs
  length;
  pageSize = 10;
  pageSizeOptions: number[] = [5, 10, 25, 100];

  displayedColumns: string[] = [
    'id',
    'name',
    'createdAt',
    'updatedAt',
    'actions',
  ];
 
   // MatPaginator Output
  pageEvent: PageEvent;
  editAccess: boolean = false;
  createAccess: boolean = false;
  deleteAccess: boolean = false;
  //displayedColumns: string[] = ['id', 'name', 'comment','action'];
  loading = false;

  //DATA: ReturnedChecks[];
  dataSource: any;
  //dataSource = new MatTableDataSource<any>();
  //dataSource = new MatTableDataSource<ReturnedChecks>(this.DATA);

 
  constructor(private router: Router,
              private http: HttpClient,
              private loaderService: LoaderService,
              private languageService: LanguageService,
              private api: HealthPlanService,
              private app: AppService,
              private dialog: MatDialog) {
  }


  ngOnInit(): void {
    // TODO: Acces
    // this.editAccess = this.app.checkMenuRoleAccess(MenuRoles.QUALIFYING_EVENT_UPDATE);
    // this.createAccess = this.app.checkMenuRoleAccess(MenuRoles.QUALIFYING_EVENT_CREATE);
    // this.deleteAccess = this.app.checkMenuRoleAccess(MenuRoles.QUALIFYING_EVENT_DELETE);
    this.editAccess = true;
    this.createAccess = true;
    this.deleteAccess = true;
  }

  ngAfterViewInit() {

    this.loading = true;
    
    setTimeout(() => {
      this.api.insuranceCompaniesList().subscribe((res:any) => {

        this.dataSource = new MatTableDataSource();  
        this.dataSource.data = res;  
        this.dataSource.paginator = this.paginator;  
        this.dataSource.sort = this.sort;

        this.loading = false;

      },(error:any) => {
        this.loading = false;
        this.app.showErrorMessage("Error interno");
      })
    });

  }

  goToInsuranceCompanyDetail(id){
    this.router.navigate(['/home/insurance-company',id]).then( (e) => {
      if (e) {
        console.log("Navigation is successful!");
      } else {
        console.log("Navigation has failed!");
      }
    });
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  doFilter(value: string){
    this.dataSource.filter = value.trim().toLocaleLowerCase();
  }

  onPageFired(event){
    // this.theHttpService.theGetDataFunction(event.pageIndex).subscribe((data)=>{
    // // then you can assign data to your dataSource like so
    // this.dataSource = data
    // })
  }

  setPageSizeOptions(setPageSizeOptionsInput: string) {
    if (setPageSizeOptionsInput) {
      this.pageSizeOptions = setPageSizeOptionsInput.split(',').map(str => +str);
    }
  }


  async deleteConfirm(id: string) {
    const message = await this.languageService.translate
      .get('INSURANCE_COMPANY.ARE_YOU_SURE_DELETE')
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
          // console.log(id);
          await this.api.insuranceCompaniesDelete(id);
          this.ngAfterViewInit();
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
