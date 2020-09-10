import { AlliancesService } from './../../shared/alliances.service';
import { faGlobeAmericas } from '@fortawesome/free-solid-svg-icons';
import { AppService } from '@app/shared/app.service';
import { BeneficiariesBenefitDistributionComponent } from '@app/components/beneficiaries-benefit-distribution/beneficiaries-benefit-distribution.component';
import { MultiAssistService } from '@app/shared/multiAssist.service';
import { startWith, map } from 'rxjs/operators';
import {
  Component,
  OnInit,
  ViewChild,
  Inject,
  AfterViewInit,
} from '@angular/core';
import {
  FormGroup,
  Validators,
  FormBuilder,
  AbstractControl,
} from '@angular/forms';
import { QualifyingEventService } from '@app/shared/qualifying-event.service';
import { HealthPlanService } from '@app/shared/health-plan.service';
import { CoverService } from '@app/shared/cover.service';
import { DependantsAPIService } from '@app/shared/dependants.api.service';
import { trigger, transition, style, animate } from '@angular/animations';
import {
  MatStepper,
  MatSnackBar,
  MatSelectChange,
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatSelect,
  MatDialog,
} from '@angular/material';
import * as Swal from 'sweetalert2';
import { MultiAssist } from '@app/models/MultiAssist';
import { DocumentationCallComponent } from '@app/components/documentation-call/documentation-call.component';
import { DialogGenericSuccessComponent } from '@app/components/dialog-generic-success/dialog-generic-success.component';

@Component({
  selector: 'app-multi-assist',
  templateUrl: './multi-assist.component.html',
  styleUrls: ['./multi-assist.component.css'],
})
export class MultiAssistComponent implements OnInit, AfterViewInit {
  id: number;
  clientId: string;
  multi_assist: FormGroup;
  multi_assist_summary: FormGroup;
  healthPlans: any = [];
  filteredHPs: any;
  filteredCovers: any;
  covers: any = [];
  selectedCover: any;
  client_product_id: string;
  hasBeneficiary: boolean = false;
  hasVehicle: boolean = false;
  BeneficiariesList: FormGroup[] = [];
  VehicleList: FormGroup[] = [];
  multi_assist_vehicule: FormGroup;
  multi_assist_bank: FormGroup;
  daysNums: Array<number>;
  effectiveDate: Date;
  createdDate: Date;
  eligibleWaitingPeriodDate: Date;
  @ViewChild('beneficiaries') beneficiaries;
  @ViewChild('coverSelection') coverSelection;
  constructor(
    private allianceService: AlliancesService,
    private coverService: CoverService,
    private app: AppService,
    private _formBuilder: FormBuilder,
    private multiAssistApiService: MultiAssistService,
    public dialogRef: MatDialogRef<MultiAssistComponent>,
    public dialog: MatDialog,

    @Inject(MAT_DIALOG_DATA) data
  ) {
    this.id = data.id;
    this.clientId = data.clientId;
  }
  async ngAfterViewInit() {}

  async ngOnInit() {
    try {
      this.initForms();
      this.daysNums = Array.from(Array(2), (x, i) => (i + 1) * 15);
      this.healthPlans = await this.multiAssistApiService
        .getMultiAssistPlans()
        .toPromise();

      this.filteredHPs = this.multi_assist.get('HealthPlan').valueChanges.pipe(
        startWith(''),
        map((value) => (typeof value === 'string' ? value : value.name)),
        map((name) => (name ? this.filter(name) : this.healthPlans.slice()))
      );
      this.multi_assist.get('HealthPlan').valueChanges.subscribe((res) => {
        // console.log(res.id);
        if (res.id > 0) {
          this.coverService.GetByPlan(res.id).subscribe((res) => {
            this.covers = res;
          });
        }
      });

      this.createdDate = new Date();
      this.effectiveDate = new Date(
        this.createdDate.getFullYear(),
        this.createdDate.getMonth() + 1,
        this.createdDate.getDay()
      );
      this.eligibleWaitingPeriodDate = new Date(
        this.effectiveDate.getFullYear(),
        this.effectiveDate.getMonth() + 1,
        this.effectiveDate.getDay()
      );

      if (this.id > 0) {
        let ma = await this.multiAssistApiService.get(this.id);
        this.multi_assist.get('HealthPlan').setValue(ma.healthPlan);
        this.multi_assist.get('Addititons').setValue(ma.multiAssist.cover.id);
        this.multi_assist_bank
          .get('accType')
          .setValue(ma.multiAssist.accountType);
        if (ma.multiAssist.accountType == '') {
        }
        this.multi_assist_bank
          .get('bankName')
          .setValue(ma.multiAssist.bankName);
        this.multi_assist_bank
          .get('holderName')
          .setValue(ma.multiAssist.accountHolderName);
        this.multi_assist_bank
          .get('routingNum')
          .setValue(ma.multiAssist.routingNum);
        this.multi_assist_bank
          .get('accountNum')
          .setValue(ma.multiAssist.accountNum);
        this.multi_assist_bank.get('expDate').setValue(ma.multiAssist.expDate);
        this.multi_assist_bank.get('depdate').setValue(ma.multiAssist.debDay);
        this.multi_assist_bank
          .get('depRecurringType')
          .setValue(ma.multiAssist.debRecurringType);

        this.multi_assist_summary
          .get('endDate')
          .setValue(ma.multiAssist.endDate);

        this.hasVehicle = ma.multiAssist.cover.type == 'ASSIST-VEH';
        this.hasBeneficiary = ma.multiAssist.cover.beneficiary;
        ma.multiAssist.multiAssistsVehicle.forEach((intm) => {
          var Veh = this._formBuilder.group({
            model: [intm.model, [Validators.required]],
            vin: [intm.vin, [Validators.required]],
            make: [intm.make, [Validators.required]],
            year: [intm.year, [Validators.required]],
          });
          this.VehicleList.push(Veh);
        });
        ma.multiAssist.beneficiaries.forEach((intm) => {
          var ben = this._formBuilder.group({
            name: [intm.name, [Validators.required]],
            gender: [intm.gender, [Validators.required]],
            birthDate: [intm.birthDate, [Validators.required]],
            ssn: [
              intm.ssn,
              [Validators.required],
              this.checkSsn('').bind(this),
            ],
            relationship: [intm.relationship, [Validators.required]],
            percent: [intm.percent, [Validators.required]],
          });
          this.BeneficiariesList.push(ben);
        });
        this.client_product_id = ma.multiAssist.clientProductId;
      }
    } catch (error) {
      // console.log(error);
    }
  }
  public filter(value: string) {
    const filterValue = value.toLowerCase();

    return this.healthPlans.filter(
      (option) => option.name.toLowerCase().indexOf(filterValue) === 0
    );
  }
  displayNameFn(selected: any) {
    if (selected) return selected.name;
  }
  coverChange(event: MatSelectChange) {
    this.selectedCover = event.value;

    if (this.selectedCover) {
      this.covers.forEach((fg) => {
        if (fg.id == this.selectedCover) {
          this.hasVehicle = fg.type == 'ASSIST-VEH';
          this.hasBeneficiary = fg.beneficiary;
        }
      });
    }
  }
  async initForms() {
    this.multi_assist = this._formBuilder.group({
      HealthPlan: [null, [Validators.required]],
      Addititons: [null, [Validators.required]],
    });
    this.multi_assist_vehicule = this._formBuilder.group({
      model: [null],
      plateNum: [null],
      brand: [null],
      year: [null],
    });
    this.multi_assist_bank = this._formBuilder.group({
      accType: [null, [Validators.required]],
      bankName: [null],
      holderName: [null],
      routingNum: [null],
      accountNum: [null, [Validators.required]],
      expDate: [{ value: null, disabled: false }],
      depdate: [null, [Validators.required]],
      depRecurringType: [null, [Validators.required]],
    });
    this.multi_assist_summary = this._formBuilder.group({
      endDate: [{ value: null, disabled: false }],
    });
  }

  async register(id: number) {
    try {
      var beneficiarieslist = [];
      var vehicleList = [];
      var multiAssist = this.multi_assist.getRawValue();
      var bank = this.multi_assist_bank.getRawValue();
      this.BeneficiariesList.forEach((fg) => {
        beneficiarieslist.push(fg.getRawValue());
      });
      this.VehicleList.forEach((fg) => {
        vehicleList.push(fg.getRawValue());
      });

      if (id == 0) {
        var payload = new MultiAssist(
          id,
          multiAssist.Addititons,
          this.effectiveDate,
          this.eligibleWaitingPeriodDate,
          this.multi_assist_summary.get('endDate').value,
          100,
          1,
          bank.accType,
          bank.bankName,
          bank.holderName,
          bank.routingNum,
          bank.accountNum,
          bank.expDate,
          bank.depdate,
          bank.depRecurringType,
          beneficiarieslist,
          vehicleList,
          this.client_product_id
        );
        console.log('create');
        try {
          const res: any = await this.multiAssistApiService.create(payload, 1);
        } catch (error) {}
        this.dialogRef.close();
      } else {
        var payload = new MultiAssist(
          id,
          multiAssist.Addititons,
          this.effectiveDate,
          this.eligibleWaitingPeriodDate,
          this.multi_assist_summary.get('endDate').value,
          100,
          1,
          bank.accType,
          bank.bankName,
          bank.holderName,
          bank.routingNum,
          bank.accountNum,
          bank.expDate,
          bank.depdate,
          bank.depRecurringType,
          beneficiarieslist,
          vehicleList,
          this.client_product_id
        );

        console.log('update');
        await this.multiAssistApiService.update(payload, 1);
        this.dialogRef.close();
      }
    } catch (error) {
      console.log(error);
    }
  }
  checkSsn(ssn: string) {
    return async (control: AbstractControl) => {
      if (control.value && ssn != control.value) {
        const res: any = await this.allianceService
          .checkSsn(control.value)
          .toPromise();
        if (res) return { ssnTaken: true };
      }
      return null;
    };
  }
  openDialog(): void {
    const dialogRef = this.dialog.open(DialogGenericSuccessComponent, {
      width: '250px',
      data: {
        message: 'Success',
      },
    });
  }
}
