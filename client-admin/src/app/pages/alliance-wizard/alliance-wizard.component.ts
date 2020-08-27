import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { QualifyingEventService } from '@app/shared/qualifying-event.service';
import { HealthPlanService } from '@app/shared/health-plan.service';
import { CoverService } from '@app/shared/cover.service';

@Component({
  selector: 'app-alliance-wizard',
  templateUrl: './alliance-wizard.component.html',
  styleUrls: ['./alliance-wizard.component.css'],
})
export class AllianceWizardComponent implements OnInit {
  affiliationMethod: FormGroup;
  benefits: FormGroup;
  finalFormGroup: FormGroup;
  percentageDependent: FormGroup[] = [];
  healthPlans: any = [];
  covers: any = [];

  qualifyingEvents: [] = [];
  constructor(
    private _formBuilder: FormBuilder,
    private qualifyingEventService: QualifyingEventService,
    private healthPlansService: HealthPlanService,
    private coverService: CoverService
  ) {}

  ngOnInit(): void {
    this.qualifyingEventService.getAll().subscribe((res) => {
      this.qualifyingEvents = <any>res;
    });

    this.affiliationMethod = this._formBuilder.group({
      affiliationMethod: [null],
      qualifyingEvent: [null],
    });

    this.finalFormGroup = this._formBuilder.group({
      effectiveDate: [{ value: null, disabled: true }, [Validators.required]],
      eligibiliyDate: [{ value: null, disabled: true }, [Validators.required]],
      inscriptionType: [null, Validators.required],
      inscriptionStatus: [null, Validators.required],
    });

    this.benefits = this._formBuilder.group({
      HealthPlan: [null],
      Addititons: [null],
    });

    this.healthPlansService.GetAll().subscribe((res) => {
      this.healthPlans = res;
    });

    this.benefits.get('HealthPlan').valueChanges.subscribe((res) => {
      this.coverService.GetByPlan(res).subscribe((res) => {
        this.covers = res;
      });
    });
  }

  addDependant() {
    var newForm = this._formBuilder.group({
      name: [null],
      lastName1: [null],
      lastName2: [null],
      percent: [null],
    });
    this.percentageDependent.push(newForm);
    this.calculatePercent();
  }

  private calculatePercent() {
    var distr = 100 / this.percentageDependent.length;
    this.percentageDependent.forEach((el) => {
      el.get('percent').setValue(distr);
    });
  }

  deleteDependant(i: number) {
    this.percentageDependent.splice(i, 1);
    this.calculatePercent();
  }
}
