import {
  Component,
  OnInit,
  ViewChild,
  Inject,
  AfterViewInit,
} from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { QualifyingEventService } from '@app/shared/qualifying-event.service';
import { HealthPlanService } from '@app/shared/health-plan.service';
import { CoverService } from '@app/shared/cover.service';
import { DependantsAPIService } from '@app/shared/dependants.api.service';
import { trigger, transition, style, animate } from '@angular/animations';
import {
  MatStepper,
  MatSnackBar,
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatSelectChange,
  MatSlideToggle,
} from '@angular/material';
import * as Swal from 'sweetalert2';
import { AlliancesService } from '@app/shared/alliances.service';
import { BeneficiariesBenefitDistributionComponent } from '@app/components/beneficiaries-benefit-distribution/beneficiaries-benefit-distribution.component';

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
export class AllianceWizardComponent implements OnInit, AfterViewInit {
  affiliationMethod: FormGroup;
  benefits: FormGroup;
  finalFormGroup: FormGroup;
  BeneficiariesList: FormGroup[] = [];
  healthPlans: any = [];
  covers: any = [];

  addonsList: any[] = new Array<any>();

  qualifyingEvents: [] = [];
  @ViewChild('stepper') stepper: MatStepper;
  @ViewChild('beneficiaries')
  beneficiaries: BeneficiariesBenefitDistributionComponent;
  @ViewChild('mayorMadical') mayorMadical: MatSlideToggle;

  typesOfRelation: any;
  availableAddons: any;

  dependantsEnabled = false;

  constructor(
    private _formBuilder: FormBuilder,
    private qualifyingEventService: QualifyingEventService,
    private AlianceService: AlliancesService,
    private coverService: CoverService,
    private DependantsServices: DependantsAPIService,
    private _snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<AllianceWizardComponent>,
    private halthPanService: HealthPlanService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}
  ngAfterViewInit(): void {
    console.log({
      ClientId: this.data.clientid,
      QualifyingEvetId: this.affiliationMethod.get('qualifyingEvent').value,
    });
    this.stepper.selectionChange.subscribe((ev) => {
      var qlf = this.affiliationMethod.get('qualifyingEvent').value;
      if (ev.selectedIndex == 1) {
        this.AlianceService.AlianceRequest({
          ClientId: this.data.clientid,
          QualifyingEvetId: qlf == null ? 0 : qlf,
        }).subscribe((res) => {
          this.healthPlans = res;
        });
      }
    });

    this.affiliationMethod.get('affiliationMethod').setValue('2');
  }

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
      cover: [null],
    });

    this.benefits.get('HealthPlan').valueChanges.subscribe((res) => {
      this.coverService.GetByPlan(res).subscribe((res) => {
        this.covers = res;
      });
    });
  }

  checkPercent() {
    var percentage: number = 0;
    if (
      this.beneficiaries &&
      this.beneficiaries.dependantsEnabled &&
      this.beneficiaries.dependantsEnabled.checked
    ) {
      //debugger;
      this.BeneficiariesList.forEach((x) => {
        percentage += Number.parseFloat(x.get('percent').value);
      });
      console.log(percentage);
      if (percentage == 100) this.stepper.next();
      else
        this.BeneficiariesList.forEach((x) => {
          x.get('percent').markAsDirty();
          x.get('percent').setErrors({ BadPercentage: true });
        });
    } else {
      this.stepper.next();
    }
  }

  planChanged(event: MatSelectChange) {
    this.halthPanService.GetAllAddOns(event.value).subscribe((res) => {
      this.availableAddons = res;
    });
  }

  lifeInsuranceToggle(chekced, addon) {
    this.toggleAddon(chekced, addon);
  }

  mayorMedical(chekced, addon) {
    this.toggleAddon(chekced, addon);
  }

  private toggleAddon(chekced: any, addon: any) {
    if (chekced) {
      this.addonsList.push(addon.id);
    } else {
      var index = this.addonsList.findIndex((x) => x == addon.id);
      if (index > -1) {
        this.addonsList.splice(index, 1);
      }
    }
    console.log(this.addonsList);
  }

  async submitAliance() {
    var addonsSelected = [];
    var beneficiarieslist = [];
    this.BeneficiariesList.forEach((fg) => {
      beneficiarieslist.push(fg.value);
    });
    await this.AlianceService.create({
      //Id: null, //new item
      //ClientProductId: null, //create item in table with this name to fill with the created id this field
      QualifyingEventId: this.affiliationMethod.get('qualifyingEvent').value,
      CoverId: this.benefits.get('cover').value,
      ClientId: this.data.clientid,
      StartDate: null,
      ElegibleDate: null,
      EndDate: null,
      EndReason: null,
      AffType: null,
      AffStatus: null,
      AffFlag: null,
      Coordination: null,
      //LifeInsurance: null, //will be moved to other table
      //MajorMedical: null, //will be moved to other table
      Prima: null,
      CreatedAt: null,
      UpdatedAt: null,
      DeletedAt: null,
      Joint: null,
      CoverAmount: null,
      LifeInsuranceAmount: null,
      MajorMedicalAmount: null,
      SubTotal: null,
      AddonList: this.addonsList,
      Beneficiaries: beneficiarieslist,
    }).then(() => {
      this.dialogRef.close();
    });
  }
}
