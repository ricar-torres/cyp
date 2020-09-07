import {
  Component,
  OnInit,
  ViewChild,
  Inject,
  AfterViewInit,
  ViewChildren,
  QueryList,
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
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatSelectChange,
  MatSlideToggle,
} from '@angular/material';
import * as Swal from 'sweetalert2';
import { AlliancesService } from '@app/shared/alliances.service';
import { BeneficiariesBenefitDistributionComponent } from '@app/components/beneficiaries-benefit-distribution/beneficiaries-benefit-distribution.component';
import { Observable } from 'rxjs';

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

  affTypes: [];
  addonsList: any[] = new Array<any>();

  qualifyingEvents: [] = [];
  @ViewChild('stepper') stepper: MatStepper;
  @ViewChildren('beneficiaries')
  beneficiaries: QueryList<BeneficiariesBenefitDistributionComponent>;
  @ViewChildren('mayorMedicalToggle') mayorMadical: QueryList<MatSlideToggle>;

  allianceWithCost: any;
  typesOfRelation: any[] = new Array();
  availableAddons: any[] = new Array();

  constructor(
    private _formBuilder: FormBuilder,
    private qualifyingEventService: QualifyingEventService,
    private AllianceService: AlliancesService,
    private coverService: CoverService,

    public dialogRef: MatDialogRef<AllianceWizardComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}
  async ngAfterViewInit() {
    this.stepper.selectionChange.subscribe((ev) => {
      var qlf = this.affiliationMethod.get('qualifyingEvent').value;
      if (ev.selectedIndex == 1) {
        this.AllianceService.AlianceRequest(this.data.clientid).subscribe(
          (res) => {
            this.healthPlans = res;
          }
        );
      }
    });

    await this.fillFormEdit();
  }

  private async fillFormEdit() {
    if (this.data.alliance) {
      //get the alliance
      var alliance = this.data.alliance;
      //fill the forms in the wizard
      if (alliance.qualifyingEvent.id == 1) {
        this.affiliationMethod.get('affiliationMethod').setValue('2');
      } else {
        this.affiliationMethod.get('affiliationMethod').setValue('1');
        this.affiliationMethod
          .get('qualifyingEvent')
          .setValue(alliance.qualifyingEvent.id);
      }

      if (alliance.cover.healthPlanId) {
        this.benefits.get('HealthPlan').setValue(alliance.cover.healthPlanId);
        this.benefits.get('cover').setValue(alliance.cover.id);
      }

      if (alliance.addonList) {
        var addons = (<[]>alliance.addonList).map((x) => {
          return { id: x };
        });

        this.addonsList = alliance.addonList;

        this.availableAddons = <[]>(
          await this.coverService.GetAllAddOns(alliance.cover.id).toPromise()
        );

        //checking the corresponding addons
        var lifeInsurance = addons.findIndex((x) => x.id == 1 || x.id == 3);
        if (lifeInsurance > -1) {
          this.beneficiaries.changes.subscribe((x) => {
            x.first.dependantsEnabled.toggle();
          });

          <[]>alliance.beneficiaries.forEach((intm) => {
            var Beneficiary = this._formBuilder.group({
              id: [intm.id],
              name: [intm.name, [Validators.required]],
              gender: [intm.gender, [Validators.required]],
              birthDate: [intm.birthDate, [Validators.required]],
              ssn: [
                intm.ssn,
                [Validators.required],
                this.checkSsn(intm.ssn).bind(this),
              ],
              relationship: [intm.relationship, [Validators.required]],
              percent: [intm.percent, [Validators.required]],
            });
            this.BeneficiariesList.push(Beneficiary);
          });
        }
        var mayorMedical = addons.findIndex((x) => x.id == 2 || x.id == 4);
        if (mayorMedical > -1) {
          this.mayorMadical.changes.subscribe((x) => {
            x.first.toggle();
          });
        }

        //alliance effecivve details
        this.finalFormGroup.get('effectiveDate').setValue(alliance.startDate);
        this.finalFormGroup
          .get('eligibiliyDate')
          .setValue(alliance.elegibleDate);
        this.finalFormGroup.get('inscriptionType').setValue(alliance.affType);
        this.finalFormGroup
          .get('inscriptionStatus')
          .setValue(alliance.affStatus);
      }
    }
  }

  ngOnInit(): void {
    this.qualifyingEventService.getAll().subscribe((res) => {
      this.qualifyingEvents = <any>res;
    });

    this.AllianceService.getAllAffTypes().subscribe((res) => {
      this.affTypes = <[]>res;
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

  async checkPercent(save?) {
    var percentage: number = 0;
    var AllBeneficieriesAreValid = true;
    if (
      this.beneficiaries.first &&
      this.beneficiaries.first.dependantsEnabled &&
      this.beneficiaries.first.dependantsEnabled.checked
    ) {
      //debugger;
      this.BeneficiariesList.forEach((x) => {
        percentage += Number.parseFloat(x.get('percent').value);
        if (x.invalid) {
          AllBeneficieriesAreValid = false;
        }
      });
      if (percentage == 100 && AllBeneficieriesAreValid) {
        if (!this.data.alliance || save)
          this.allianceWithCost = await this.submitAliance();
        this.stepper.next();
      } else {
        if (percentage != 100) {
          this.BeneficiariesList.forEach((x) => {
            x.get('percent').markAsDirty();
            x.get('percent').setErrors({ BadPercentage: true });
          });
        }
      }
    } else {
      if (!this.data.alliance || save)
        this.allianceWithCost = await this.submitAliance();
      this.stepper.next();
    }
  }

  coverChanged(event: MatSelectChange) {
    this.coverService.GetAllAddOns(event.value).subscribe((res) => {
      this.availableAddons = <[]>res;
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
  }

  async submitAliance() {
    var addonsSelected = [];
    var beneficiarieslist = [];
    this.BeneficiariesList.forEach((fg) => {
      beneficiarieslist.push(fg.getRawValue());
    });

    var res = await this.AllianceService.create({
      Id: this.data.alliance ? this.data.alliance.id : null,
      ClientProductId: this.data.alliance
        ? this.data.alliance.clientProductId
        : null,
      QualifyingEventId: this.affiliationMethod.get('qualifyingEvent').value,
      CoverId: this.benefits.get('cover').value,
      ClientId: this.data.clientid,
      StartDate: this.finalFormGroup.get('effectiveDate').value
        ? this.finalFormGroup.get('effectiveDate').value
        : null,
      ElegibleDate: this.finalFormGroup.get('eligibiliyDate').value
        ? this.finalFormGroup.get('eligibiliyDate').value
        : null,
      // EndDate: null,
      // EndReason: null,
      AffType: this.finalFormGroup.get('inscriptionType').value
        ? this.finalFormGroup.get('inscriptionType').value
        : null,
      AffStatus: this.finalFormGroup.get('inscriptionStatus').value
        ? this.finalFormGroup.get('inscriptionStatus').value
        : null,
      // AffFlag: null,
      // Coordination: null,
      // LifeInsurance: null, //will be moved to other table
      // MajorMedical: null, //will be moved to other table
      // Prima: null,
      // Joint: null,
      // CoverAmount: null,
      // LifeInsuranceAmount: null,
      // MajorMedicalAmount: null,
      // SubTotal: null,
      AddonList: this.addonsList,
      Beneficiaries: beneficiarieslist,
    });
    return res;
  }

  close() {
    this.dialogRef.close();
  }

  healthPlanSelected() {
    this.benefits.get('cover').setValue(null);
    this.availableAddons = [];
  }

  checkSsn(ssn: string) {
    return async (control: AbstractControl) => {
      if (control.value && ssn != control.value) {
        const res: any = await this.AllianceService.checkSsn(
          control.value
        ).toPromise();
        if (res) return { ssnTaken: true };
      }
      return null;
    };
  }
}
