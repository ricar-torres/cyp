import { Component, Inject, OnInit, ElementRef, ViewChild } from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, NgForm, Validators, FormBuilder } from '@angular/forms';
import { Router } from "@angular/router";
import { HttpClient } from '@angular/common/http';
import { HealthPlanService } from '../../shared/health-plan.service';
import {UploadfilesService} from '../../shared/upload-files.service';
import { AppService } from '../../shared/app.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfirmDialogModel, ConfirmDialogComponent } from '@app/components/confirm-dialog/confirm-dialog.component';
import { MatAutocompleteSelectedEvent, MatAutocomplete } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { LanguageService } from '@app/shared/Language.service';


export interface benefit {
  id: number;
  parentId: number;
  rowOrder: number;
  name: string;
  value: string;
}


export interface TypeCalculate {
  id: number;
  name: string;
}

@Component({
  selector: 'app-insurance-plan-item',
  templateUrl: './insurance-plan-item.component.html',
  styleUrls: ['./insurance-plan-item.component.css']
})

export class InsurancePlanItemComponent implements OnInit {
  planInfoFormGroup: FormGroup;

  healthPlanId: string;
  id: string;
  dataSourceAddOns: any;
  displayedColumnsAddOns: string[] = ['addOns', 'actionDelete'];
  dataSourceBenefit: any;
  displayedColumnsBenefit: string[] = ['name', 'value', 'actionEdit'];

  displayedColumnsRate: string[] = ['age', 'rate'];
  dataSourceRate: any;//new MatTableDataSource(ELEMENT_DATA);
  loading = false;
  file: Blob;
  isValidFile: boolean = false;

  TypeCalculate: TypeCalculate[] = [
    { id: 1, name: 'Solo por empleados' },
    { id: 2, name: 'Para todos los miembros' },
    { id: 3, name: 'Por tipo cubierta' },
    { id: 4, name: 'Todos los miembros por edad' },
    { id: 5, name: 'Por tipo cubierta y edad' }
  ];
  benefitType: benefit[] = [];
  policyYear: number;

  //Chips 

  visible = true;
  selectable = true;
  removable = true;
  separatorKeysCodes: number[] = [ENTER, COMMA];
  filteredAddons: Observable<string[]>;
  addons: any[] = [];
  benefitValue: any[] = [];
  allAddons: any[];

  @ViewChild('addonInput') addonInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete: MatAutocomplete;


  constructor(
    private route: ActivatedRoute,
    private router: Router,
    public http: HttpClient,
    private api: HealthPlanService,
    private uploadFileApp:UploadfilesService,
    private app: AppService,
    private languageService: LanguageService,
    public dialog: MatDialog
    ) {


    this.healthPlanId = this.route.snapshot.paramMap.get('insuranceCompanyId');
    this.id = this.route.snapshot.paramMap.get('id');
    this.policyYear=(new Date()).getFullYear();
    this.setupForm(null);


  }


  async ngOnInit() {

    try {

      this.loading = true;

      // this.healthPlanId = this.route.snapshot.paramMap.get('healthPlanId');
      // this.id = this.route.snapshot.paramMap.get('id');
      console.log(this.route.snapshot.paramMap);
      
      
    //   .subscribe(params => {
    //     console.log(params);
    // });

      const res: any = await this.api.BenefitTypesList();

      this.benefitType = [];

      res.map(async (e, u) => {
        this.benefitType.push({
          id: e.id,
          parentId: e.parentBenefitTypeID,
          rowOrder: e.rowOrder,
          name: e.benefitType,
          value: e.benefitType
        }

        );
      });

      console.log(this.healthPlanId);

      if (this.id) {
        const res: any = await this.api.insurancePlanById(this.id);
         const res2: any = await this.api.insurancePlanRateById(this.id);

         this.dataSourceRate = new MatTableDataSource();
         this.dataSourceRate.data = res2;
        // res.members = res2;
        this.setupForm(res);
      }


      this.loadAddons();
      this.loadBenefits();

      

      this.loading = false;

    } catch (error) {

      this.loading = false;
      if (error.status != 401) {
        
      console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });

      }

    }

  }


  async setupForm(object) {

    this.loading = true;


    try {
      if (object) {

        console.log(object);
        console.log(this.id);


        this.dataSourceAddOns = new MatTableDataSource();
        this.dataSourceAddOns.data = object.addOns;


        this.addons = object.addOns.map((addOn) => {
          // console.log(addOn);
          return addOn.insuranceAddOns;
        });

        this.benefitValue = object.benefitTypes;

        //console.log(this.addons);

        this.planInfoFormGroup = new FormGroup({

          id: new FormControl(object.id, [
          ]),
          healthPlanId: new FormControl(object.healthPlanId, [
            Validators.required
          ]),
          code: new FormControl(object.code, [
          ]),
          name: new FormControl(object.name, [
            Validators.required
          ]),
          sob: new FormControl(object.sob, []),
          alianza: new FormControl(object.alianza, []),
          type: new FormControl(object.type, [
            Validators.required
          ]),
          beneficiary: new FormControl(object.beneficiary, []),

          addOnsAlt: new FormControl(this.addons.map((r) => {
            return r.id
          })),
          // addOnsAlt: new FormControl(object.addOnsAlt, [])

        });

      } else {

        this.planInfoFormGroup = new FormGroup({

          // id: new FormControl(object.id, [
          // ]),
          healthPlanId: new FormControl(this.healthPlanId, [
            Validators.required
          ]),
          code: new FormControl('', [
          ]),
          name: new FormControl('', [
            Validators.required
          ]),
          sob: new FormControl('', []),
          alianza: new FormControl(false, []),
          type: new FormControl('', [
            Validators.required
          ]),
          beneficiary: new FormControl(false, []),

          addOnsAlt: new FormControl([]),
          //addOnsAlt: new FormControl(null, [])

        });

      }
      this.loading = false;

    } catch (error) {

      this.loading = false;
      if (error.status != 401) {
        
      console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    }

  }

  private async loadBenefits() {

    try {

      this.benefitType.map(async (e, u) => {
        this.benefitType[u].value = this.benefitValue.filter(x => x.insuranceBenefitTypeId == e.id).reduce(function (n, benefit) {
          return benefit.value;
        }, '');
      });


      console.log(this.benefitType);
      this.dataSourceBenefit = new MatTableDataSource();
      this.dataSourceBenefit.data = this.benefitType;

    }
    catch (error) {
      if (error.status != 401) {
        
      console.error('error', error);
      this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
        this.app.showErrorMessage(res);
      });
      }

    }

  }


  private async loadAddons() {

    try {

      //const currentRoles = this.reactiveForm.controls.roles.value;
      this.allAddons = [];

      const res: any = await this.api.addonAvailable(this.healthPlanId);
      this.allAddons = res.filter(x => !this.addons.some(y => y.id == x.id)).map((addon) => {
        return addon
      });

      this.filteredAddons = this.planInfoFormGroup.controls.addOnsAlt.valueChanges.pipe(
        startWith(null),
        map((addOn: any | null) => addOn ? this._filter(addOn.name) : this.allAddons.slice())
      );

      console.log(this.filteredAddons);

    } catch (error) {
      if (error.status != 401) {
        
      console.error('error', error);
      this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
        this.app.showErrorMessage(res);
      });
      }

    }

  }


  //chips

  add(event: MatChipInputEvent): void {



    const input = event.input;
    const value = event.value;

    // Add our role
    if ((value || '').trim()) {
      this.addons.push(value.trim());
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }

    this.planInfoFormGroup.controls.addOnsAlt.setValue(null);
  }

  remove(addons: any): void {

    const index = this.addons.indexOf(addons);

    if (index >= 0) {
      this.addons.splice(index, 1);
    }

    this.planInfoFormGroup.controls.addOnsAlt.setValue(this.addons.map((r) => {
      return r.id
    }));

    this.loadAddons();

  }

  selected(event: MatAutocompleteSelectedEvent): void {

    console.log(event.option.value);



    this.addons.push(event.option.value);
    this.addonInput.nativeElement.value = '';

    //this.reactiveForm.controls.rolesAlt.setValue(null);

    console.log(this.addons);
    this.planInfoFormGroup.controls.addOnsAlt.setValue(this.addons.map((r) => {
      return r.id
    }));

    this.loadAddons();

  }


  private _filter(value: string): any[] {

    console.log(value);
    const filterValue = value ? value.toLowerCase() : '';
    return this.allAddons ? this.allAddons.filter(item => item.name.toLowerCase().indexOf(filterValue) === 0) : [];

  }


  async onSubmit() {


    console.log(this.planInfoFormGroup.value);
    if (this.id) {
      try {
        // console.log(this.reactiveForm.value);

        const result: any = await this.api.InsurancePlansave(this.id, this.planInfoFormGroup.value);

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
    else if (!this.id) {
      try {

        const result: any = await this.api.insurancePlanCreate(this.planInfoFormGroup.value);

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

  }


  onBack() {
    this.router.navigate(['/home/insurance-company', this.healthPlanId]).then((e) => {
      if (e) {
        console.log("Navigation is successful!");
      } else {
        console.log("Navigation has failed!");
      }
    });
  }


  async delAdons(InsurancePlanId: string, InsuranceAddOnsId: string) {

    try {

      this.loading = true;
      const res1: any = await this.api.planAddOnsDelete(InsurancePlanId, InsuranceAddOnsId);

      if (this.id) {
        const res: any = await this.api.insurancePlanById(this.id);
        // const res2: any = await this.api.insuranceEstimatesMembers(this.id);
        // res.members = res2;
        this.setupForm(res);
      }

      this.loading = false;
      console.log("Eliminado!");
      // this.ngAfterViewInit();

    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
       
      console.error('error', error);
      this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
        this.app.showErrorMessage(res);
      });
      }
    }

  }

  async delPlan(InsurancePlanId: string) {

    try {

      this.loading = true;
      const res1: any = await this.api.insurancePlanDelete(InsurancePlanId);

      //console.log(res1);
      //if (this.id) {
      //const res: any = await this.api.insurancePlanById(this.id);
      // const res2: any = await this.api.insuranceEstimatesMembers(this.id);
      // res.members = res2;
      //this.setupForm(res);
      //} 
      this.onBack();

      this.loading = false;
      console.log("Eliminado!");
      // this.ngAfterViewInit();

    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
       
      console.error('error', error);
      this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
        this.app.showErrorMessage(res);
      });
      }
    }

  }



  async saveBenefitType(item) {

    try {

      this.loading = true;
      const res: any = await this.api.addBenefitTypes(this.id, item);
      this.loading = false;
      console.log("save benefit!");
      //this.ngAfterViewInit();
      //this.loadBenefits();

    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
       
      console.error('error', error);
      this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
        this.app.showErrorMessage(res);
      });
      }
    }

  }

  async confirmDialog(InsurancePlanId, name, type) {
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
      // console.log(dialogResult);
      if (dialogResult) {
        if (type == 1)
          this.delPlan(this.id)
      }
      //this.result = dialogResult;
    });
  }


//Rate

  onFileChange(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      // console.log(file);
      this.file = file;
      console.log(this.file);
      // this.RatesFormGroup.controls.file.setValue(file);
      if (this.id) {
        this.isValidFile = true;
      }
    } else {
      this.isValidFile = false;
    }
  }


submitRateFile() {

  //console.log(data);

  this.loading = true;
  const formData = new FormData();
  formData.append('file', this.file);

  if (!this.id) {
    this.app.showMessage("Favor grabar el registro antes de agregar las tarifas.", "Error", "warning");

  }
  else {

    
    console.log(formData);

     this.uploadFileApp.AddPlanRateUpload(this.id, this.policyYear , formData).subscribe((res) => {

      if (res.length != 0) {


        this.dataSourceRate = new MatTableDataSource();

        this.dataSourceRate.data = res;
        this.loading = false;
        // data.ratesByAge = res;
         console.log(res);

      } else {
        this.loading = false;
      }

    },
      (err) => {
        this.loading = false;
        err.error ? this.app.showMessage("Error en Archivo", err.error.error, "warning") : this.app.showMessage("Error en Archivo", "Error", "warning");
      }
    ); 

  }

}


  openDialog(data): void {

    const benefit = <benefit>data;

    console.log(benefit);

    const dialogRef = this.dialog.open(BenefitTypeDialog, {
      width: '55%',
      data: benefit,//{ item: item},
      disableClose: true,

    });

    dialogRef.afterOpened(

    );


    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      if (result) {

        console.log(result);

        const item: any = {
          InsurancePlanId: this.id,
          InsuranceBenefitTypeId: result.id,
          Value: result.value
        };

        this.saveBenefitType(item);


      }
      else {
        console.log('Cancelado');
      }
    });
  }




}


@Component({
  selector: 'dialogBenefitType',
  templateUrl: './dialogBenefitType.html',
  styleUrls: ['./insurance-plan-item.component.css']
})
export class BenefitTypeDialog {

  constructor(
    public dialogRef: MatDialogRef<BenefitTypeDialog>,
    @Inject(MAT_DIALOG_DATA) public data: benefit) { }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
