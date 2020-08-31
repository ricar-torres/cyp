import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { QualifyingEventService } from '@app/shared/qualifying-event.service';
import { HealthPlanService } from '@app/shared/health-plan.service';
import { CoverService } from '@app/shared/cover.service';
import { DependantsAPIService } from '@app/shared/dependants.api.service';
import { trigger, transition, style, animate } from '@angular/animations';
import { MatStepper, MatSnackBar } from '@angular/material';
import * as Swal from 'sweetalert2';

@Component({
  selector: 'app-alliance-wizard',
  templateUrl: './alliance-wizard.component.html',
  styleUrls: ['./alliance-wizard.component.css'],
  animations: [
    trigger('fadeAnimation', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('300ms', style({ opacity: 1 })),
      ]),
      transition(':leave', [
        style({ opacity: 1 }),
        animate('300ms', style({ opacity: 0 })),
      ]),
    ]),
  ],
})
export class AllianceWizardComponent implements OnInit {
  affiliationMethod: FormGroup;
  benefits: FormGroup;
  finalFormGroup: FormGroup;
  percentageDependent: FormGroup[] = [];
  healthPlans: any = [];
  covers: any = [];

  qualifyingEvents: [] = [];
  @ViewChild('stepper') stepper: MatStepper;

  typesOfRelation: any;

  constructor(
    private _formBuilder: FormBuilder,
    private qualifyingEventService: QualifyingEventService,
    private healthPlansService: HealthPlanService,
    private coverService: CoverService,
    private DependantsServices: DependantsAPIService,
    private _snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    // this.DependantsServices.getRelationTypes().subscribe((res) => {
    //   this.typesOfRelation = res;
    // });
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

  checkPercent() {
    var percentage: number = 0;
    this.percentageDependent.forEach((x) => {
      percentage += Number.parseFloat(x.get('percent').value);
    });
    console.log(percentage);
    if (percentage == 100) this.stepper.next();
    else
      this.percentageDependent.forEach((x) => {
        x.get('percent').markAsDirty();
        x.get('percent').setErrors({ BadPercentage: true });
      });
  }
}
