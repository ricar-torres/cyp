import { BeneficiariesBenefitDistributionComponent } from '@app/components/beneficiaries-benefit-distribution/beneficiaries-benefit-distribution.component';
import { MultiAssistAPIService } from '@app/shared/MultiAssist.api.service';
import { startWith, map } from 'rxjs/operators';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { QualifyingEventService } from '@app/shared/qualifying-event.service';
import { HealthPlanService } from '@app/shared/health-plan.service';
import { CoverService } from '@app/shared/cover.service';
import { DependantsAPIService } from '@app/shared/dependants.api.service';
import { trigger, transition, style, animate } from '@angular/animations';
import { MatStepper, MatSnackBar, MatSelectChange } from '@angular/material';
import * as Swal from 'sweetalert2';

@Component({
  selector: 'app-multi-assist',
  templateUrl: './multi-assist.component.html',
  styleUrls: ['./multi-assist.component.css'],
})
export class MultiAssistComponent implements OnInit {
  multi_assist: FormGroup;
  healthPlans: any = [];
  filteredHPs: any;
  filteredCovers: any;
  covers: any = [];
  selectedCover: any;
  hasBeneficiary: boolean = false;
  hasVehicule: boolean = false;
  BeneficiariesList: FormGroup[] = [];
  multi_assist_vehicule: FormGroup;
  multi_assist_bank: FormGroup;
  daysNums: Array<number>;
  @ViewChild('beneficiaries') beneficiaries;
  constructor(
    private _formBuilder: FormBuilder,
    private qualifyingEventService: QualifyingEventService,
    private healthPlansService: HealthPlanService,
    private coverService: CoverService,
    private DependantsServices: DependantsAPIService,
    private multiAssistApiService: MultiAssistAPIService,
    private _snackBar: MatSnackBar
  ) {}

  async ngOnInit() {
    this.initForms();
    this.daysNums = Array.from(Array(30), (x, i) => i + 1);
    this.healthPlans = await this.multiAssistApiService
      .GetAllMultiAssist()
      .toPromise();

    this.filteredHPs = this.multi_assist.get('HealthPlan').valueChanges.pipe(
      startWith(''),
      map((value) => (typeof value === 'string' ? value : value.name)),
      map((name) => (name ? this.filter(name) : this.healthPlans.slice()))
    );
    this.multi_assist.get('HealthPlan').valueChanges.subscribe(async (res) => {
      this.covers = res.covers;
    });
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
      this.hasBeneficiary = this.selectedCover.beneficiary;
      this.hasVehicule = this.selectedCover.type == 'ASSIST-VEH';
    }
  }
  initForms() {
    this.multi_assist = this._formBuilder.group({
      HealthPlan: [null, [Validators.required]],
      Addititons: [null, [Validators.required]],
    });
    this.multi_assist_vehicule;
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
      expDate: [{ value: null, disabled: true }],
      depdate: [null, [Validators.required]],
      depRecurringType: [null, [Validators.required]],
    });
  }

  register() {
    var multiAssist = this.multi_assist.getRawValue();
    var bank = this.multi_assist_bank.getRawValue();
    var vehicle = this.multi_assist_vehicule.getRawValue();
    // var benefi = new Arra
    var payload = Object.assign(multiAssist, bank, vehicle, benefi);
    console.log(JSON.stringify(payload));
    this.multiAssistApiService.AttatchMultiAssist(payload);
  }
}
