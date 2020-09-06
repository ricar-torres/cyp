import { Component, Inject, OnInit } from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, NgForm, Validators, FormBuilder } from '@angular/forms';
import { Router } from "@angular/router";
import { HttpClient } from '@angular/common/http';
import { HealthPlanService } from '../../shared/health-plan.service';
import { AppService } from '../../shared/app.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  ConfirmDialogModel,
  ConfirmDialogComponent,
} from '@app/components/confirm-dialog/confirm-dialog.component';
import { LanguageService } from '@app/shared/Language.service';





// var ELEMENT_DATA: CoverElement[] = [
//   // {id: 1, name: 'Hydrogen', cost: 1.0079},
//   // {id: 2, name: 'Helium', cost: 4.0026},
//   // {id: 3, name: 'Lithium', cost: 6.941},
//   // {id: 4, name: 'Beryllium', cost: 9.0122},
//   // {id: 5, name: 'Boron', cost: 10.811},
//   // {id: 6, name: 'Carbon', cost: 12.0107},
//   // {id: 7, name: 'Nitrogen', cost: 14.0067},
//   // {id: 8, name: 'Oxygen', cost: 15.9994},
//   // {id: 9, name: 'Fluorine', cost: 18.9984},
//   // {id: 10, name: 'Neon', cost: 20.1797},
// ];




export interface AddonsTypeCalculate {
  id: number;
  name: string;
}

export interface AddOnsElement {
  insuranceCompanyId: number;
  name: string;
  individualRate: number;
  coverageSingleRate: number;
  coverageCoupleRate: number;
  coverageFamilyRate: number;
  minimumEE: number;
  typeCalculate: number;
  ratesByAge: [
    {
      age: number;
      rate: number;
    }
  ];
  id: number;
}


@Component({
  selector: 'app-insurance-company-item',
  templateUrl: './insurance-company-item.component.html',
  styleUrls: ['./insurance-company-item.component.css']
})


export class InsuranceCompanyItemComponent implements OnInit {


  displayedColumns: string[] = ['id', 'name', 'addOns', 'action'];
  displayedColumnsAddOns: string[] = ['id', 'name', 'actionEdit', 'actionDelete'];
  dataSource: any;//new MatTableDataSource(ELEMENT_DATA);
  dataSourceAddOns: any;
  ConverItem: AddOnsElement;
  loading = false;
  id: string;

  reactiveForm: FormGroup;
  coverForm: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    public http: HttpClient,
    private api: HealthPlanService,
    private app: AppService,
    private languageService: LanguageService,
    public dialog: MatDialog) {
    this.setupForm(null)
  }


  // displayedColumns: string[] = ['position', 'name', 'weight', 'symbol'];

  //  app2 = ApiService;


  // async ngOnInit() {

  //   try {

  //     this.loading = true;
  //     this.id = this.route.snapshot.paramMap.get('id');
  //    // ELEMENT_DATA =   this.api.optionalCoverList();    
  //     const res:any = await this.api.insuranceCompanyById(this.id);
  //     if (res){
  //       console.log(res);
  //       this.setupForm(res);
  //       this.loading = false;
  //     }


  //   } catch (error) {
  //     this.loading = false;
  //     console.log("Error",error);
  //     if (error.status == 401){
  //       //this.app.logout();
  //     }else{
  //       this.app.showErrorMessage("Error interno");
  //     }
  //   }
  // }

  async ngOnInit() {

    try {

      //this.loading = true;
      this.id = this.route.snapshot.paramMap.get('id');

      this.loadData()
      //console.log(id);

      //this.loading = false;

    } catch (error) {

      //this.loading = false;
      if (error.status != 401) {
        this.app.showErrorMessage("Error interno");
      }
    }

  }

  async loadData()
  {

    try {

      this.loading = true;
    const obj: any = await this.api.insuranceCompanyById(this.id);

    //ELEMENT_DATA=Obj.insurancePlans;  

    console.log(obj.insurancePlans);


    this.dataSource = new MatTableDataSource();
    this.dataSource.data = obj.insurancePlans;

    this.dataSourceAddOns = new MatTableDataSource();
    this.dataSourceAddOns.data = obj.insuranceAddOns;


    this.setupForm(obj);
    this.loading = false;

    } catch (error) {

      this.loading = false;
      if (error.status != 401) {
        this.app.showErrorMessage("Error interno");
      }
    }


  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  applyFilterAddOns(filterValue: string) {
    this.dataSourceAddOns.filter = filterValue.trim().toLowerCase();
    this.dataSourceAddOns.filter = filterValue.trim().toLowerCase();
  }


  async onSubmit() {

    try {
      // console.log(this.reactiveForm.value);


      const result: any = await this.api.insuranceCompanySave(this.reactiveForm.value.id, this.reactiveForm.value);

      console.log(result);
      if (result) {
        this.app.showMessage("Mensaje", "Guardado existosamente!", "done");
      }
    } catch (error) {
      if (error.status == 401) {
        //this.app.logout();
      } else {

        console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    }

  }

  onBack() {
    this.router.navigate(['/home/insurance-company']);
  }

  private setupForm(item) {

    if (item) {
      this.reactiveForm =

        new FormGroup({

          id: new FormControl(item.id, [
            Validators.required
          ]),
          name: new FormControl(item.name, [
            Validators.required
          ]),
          comment: new FormControl(item.comment, [])

        });
    }
    else{
      this.reactiveForm =

        new FormGroup({

          id: new FormControl(null, [
            Validators.required
          ]),
          name: new FormControl(null, [
            Validators.required
          ]),
          comment: new FormControl(null, [])

        });
    }

  }


  goToPlanNew() {

    this.router.navigate(['/home/insurance-company', this.reactiveForm.value.id, 'plan']).then((e) => {
      if (e) {
        console.log("Navigation is successful!");
      } else {
        console.log("Navigation has failed!");
      }
    });
  }


  goToPlanEdit(id) {
    this.router.navigate(['/home/insurance-company', this.reactiveForm.value.id, 'plan', id]).then((e) => {
      if (e) {
        console.log("Navigation is successful!");
      } else {
        console.log("Navigation has failed!");
      }
    });
  }



  goToAddonsNew(): void {
    try {
      //var item :AddOnsElement;

      var item = {
        "insuranceCompanyId": 0,
        "name": "",
        "individualRate": 0,
        "coverageSingleRate": 0,
        "coverageCoupleRate": 0,
        "coverageFamilyRate": 0,
        "minimumEE": 0,
        "typeCalculate": 0,
        "ratesByAge": [],
        "id": 0,
        "delFlag": false
      };
      this.openDialog(item);

    } catch (error) {
      console.log("Error", error);
      if (error.status == 401) {
        //this.app.logout();
      } else {

        this.app.showErrorMessage("Error interno");
      }
    }
  }

  async saveAddOns(item) {

    if (item.id > 0) {
      try {
        const result: any = await this.api.addonsUpdate(item.id, item);
        console.log(result);
        if (result) {
          this.app.showMessage("Mensaje", "Guardado existosamente!", "done");
        }

      } catch (error) {
        console.log("Error", error);
        if (error.status == 401) {
          //this.app.logout();
        } else {

          this.app.showMessage("Mensaje", error.error.error, "warning");
        }
      }

    }
    else {


      try {
        // const id = this.route.snapshot.paramMap.get('id');

        //const res:any =
        const result: any = await this.api.addonsCreate(this.id, item);
        //const result:any = await this.api.addonsCreate(item.id, item);
        console.log(item);
        console.log(result);
        if (result) {
          this.app.showMessage("Mensaje", "Guardado existosamente!", "done");
        }
      } catch (error) {
        console.log("Error", error);
        if (error.status == 401) {
          //this.app.logout();
        } else {

          this.app.showMessage("Mensaje", error.error.error, "warning");
        }
      }
    }
    this.loadData();
    //this.ngAfterViewInit();

  }

  openDialog(data): void {

    //  var item = <AddOnsElement> data ;

    //  this.ConverItem.id = data.id;
    //  this.ConverItem.name = data.id;
    //  this.ConverItem.coverageSingleRate = data.id;
    //  this.ConverItem.id = data.id;
    //  this.ConverItem.id = data.id;
    //  this.ConverItem.id = data.id;
    //  this.ConverItem.id = data.id;
    //  this.ConverItem.id = data.id;
    //console.log(data.insuranceCompanyId);

    //  this.ConverItem.individualRate= data.individualRate;
    //  this.ConverItem.coverageSingleRate= data.coverageSingleRate;
    //  this.ConverItem.coverageCoupleRate= data.coverageCoupleRate;
    //  this.ConverItem.coverageFamilyRate= data.coverageFamilyRate;
    //  this.ConverItem.minimumEE= data.minimumEE;
    //  this.ConverItem.typeCalculate= data.typeCalculate;
    //  this.ConverItem.ratesByAge= data.ratesByAge;
    //  this.ConverItem.id= data.id;
    //  this.ConverItem.insuranceCompanyId = data.insuranceCompanyId;
    //  this.ConverItem.name= data.name;

    this.ConverItem = <AddOnsElement>data;
    // item.id=id;
    // item.name=name;


    console.log(this.ConverItem);

    // this.coverForm = new FormGroup({


    //   cost: new FormControl( this.cost, [
    //     Validators.required])

    // }); 





    const dialogRef = this.dialog.open(AddOnsDialog, {
      width: '55%',
      data: this.ConverItem,//{ item: item},
      disableClose: true,

    });

    dialogRef.afterOpened(

    );


    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      if (result) {


        this.saveAddOns(result);
        //     try{


        //       //this.loading = true;
        //       setTimeout(() => {

        //       console.log(result);

        //        var item = {
        //          "insuranceCompanyId": result.insuranceCompanyId,
        //          "name": result.name,
        //          "individualRate": result.individualRate,
        //          "coverageSingleRate": result.coverageSingleRate,
        //          "coverageCoupleRate": result.coverageCoupleRate,
        //          "coverageFamilyRate": result.coverageFamilyRate,
        //          "minimumEE": result.minimumEE,
        //          "typeCalculate": result.typeCalculate,
        //          "ratesByAge": [],
        //          "id": result.id,
        //          "delFlag": false
        //           };
        //          // console.log(res.toPromise());
        //           this.loading = false;
        //         //   res.toPromise().then(e => {
        //         //     //this.fetchedData = data;
        //         //     console.log(e.ok);
        //         //     console.log("Aqui!!!1");
        //         // })

        //           // if (res.toPromise().ok){
        //           //   this.app.showMessage("Mensaje", "Guardado existosamente!","done");
        //           // }
        //           // else
        //           // { 
        //           //   console.log(res.toPromise().error.error);
        //           //   this.app.showMessage("Mensaje", res.toPromise().error.error,"warning");}

        //       });

        //  } 
        //  catch (error) {
        //       this.loading = false;
        //     console.log("Error",error);
        //     if (error.status == 401){
        //       //this.app.logout();
        //     }else{

        //       this.app.showMessage("Mensaje", error.error.error,"warning");
        //     }
        //   }


        //this.ngAfterViewInit();
        this.loadData();



      }
      else {
        console.log('Cancelado');
      }
    });
  }


  async delAdons(id: string) {

    try {

      this.loading = true;
      const res: any = await this.api.addonsDelete(id);
      this.loading = false;
      console.log("Eliminado!");
      //this.ngAfterViewInit();
      this.loadData();

    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
        this.app.showErrorMessage("Error interno");
      }
    }

  }

  async confirmDialog(id, name) {
    //const message = `¿Estás seguro de que quieres eliminar este registro?`;

    //const dialogData = new ConfirmDialogModel(name, message);
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
      maxWidth: "400px",
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      console.log(dialogResult);
      if (dialogResult) {
        this.delAdons(id)
      }
      //this.result = dialogResult;
    });
  }

}



@Component({
  selector: 'AddOnsDialog',
  templateUrl: 'AddOnsDialog.html',
  styleUrls: ['./insurance-company-item.component.css']
})


export class AddOnsDialog {



  displayedColumns: string[] = ['age', 'rate'];
  dataSource: any;//new MatTableDataSource(ELEMENT_DATA);
  loading = false;
  file: Blob;
  isValidFile: boolean = false;
  RatesFormGroup: FormGroup;

  TypeCalculate: AddonsTypeCalculate[] = [
    { id: 1, name: 'Solo por empleados' },
    { id: 2, name: 'Para todos los miembros' },
    { id: 3, name: 'Por tipo cubierta' },
    { id: 4, name: 'Todos los miembros por edad' },
    { id: 5, name: 'Por tipo cubierta y edad' }
  ];

  constructor(
    private api: HealthPlanService, private app: AppService,
    private languageService: LanguageService,
    public dialogRef: MatDialogRef<AddOnsDialog>,
    @Inject(MAT_DIALOG_DATA) public data: AddOnsElement) {


    this.dataSource = new MatTableDataSource();
    console.log(data.ratesByAge.sort((a) => a.age));
    this.dataSource.data = data.ratesByAge.sort((a) => a.age);
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSaveClick(): void {

    this.dialogRef.close();
    // console.log('The dialog was closed');
    // this.cost = result;
  }

  onFileChange(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      // console.log(file);
      this.file = file;
      console.log(this.file);
      // this.RatesFormGroup.controls.file.setValue(file);
      this.isValidFile = true;
    } else {
      this.isValidFile = false;
    }
  }


  submitRateFile(data) {

    //console.log(data);

    this.loading = true;
    const formData = new FormData();
    formData.append('file', this.file);

    if (data.id == 0) {

      this.languageService.translate.get('INSURANCE_COMPANY.SAVE_REGISTER_BEFORE').subscribe((res) => {
        this.app.showErrorMessage(res);
      });
      //this.app.showErrorMessage("Favor grabar el registro antes de agregar las tarifas.");

      this.loading = false;
    }
    else {


      /* this.api.AddAddOnsRateUpload(data.insuranceCompanyId, data.id, formData).subscribe((res) => {

        if (res.length != 0) {


          this.dataSource = new MatTableDataSource();

          this.dataSource.data = res;
          this.loading = false;
          data.ratesByAge = res;
          console.log(data.ratesByAge);

        } else {
          this.loading = false;
        }

      },
        (err) => {
          this.loading = false;
          if (err.status != 401) {
            console.error('error', err);
            this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
              this.app.showErrorMessage(res);
            });
          }

          //err.error ? this.app.showMessage("Error en Archivo", err.error.error, "warning") : this.app.showMessage("Error en Archivo", "Error", "warning");
        }
      ); */

    }

  }


}

