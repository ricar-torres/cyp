import { Component, OnInit, Input } from '@angular/core';
import { ClientWizardService } from '@app/shared/client-wizard.service';
import { FormGroup, FormControl } from '@angular/forms';
import { HealthPlanService } from '@app/shared/health-plan.service';
import { AgencyService } from '@app/shared/agency.service';
import { CoverService } from '@app/shared/cover.service';
import { ClientService } from '@app/shared/client.service';

@Component({
  selector: 'app-general-information',
  templateUrl: './general-information.component.html',
  styleUrls: ['./general-information.component.css'],
})
export class GeneralInformationComponent implements OnInit {
  heathPlans;
  @Input() client;
  agencies = [];
  covers = [];

  healthPlan: FormControl = new FormControl();
  hasTutor: FormControl = new FormControl();

  reactiveForm: FormGroup;
  tutorInformation: FormGroup;
  loading = true;
  constructor(
    public wizadFormGroups: ClientWizardService,
    private hps: HealthPlanService,
    private ag: AgencyService,
    private cs: CoverService,
    private clientService: ClientService
  ) {}

  async ngOnInit() {
    this.reactiveForm = this.wizadFormGroups.generalInformationForm;
    this.tutorInformation = this.wizadFormGroups.tutorInformation;
    this.heathPlans = await this.hps.GetAll();
    this.agencies = await this.ag.getAll().toPromise();
    if (this.client) {
      await this.fillForm();
    }
    this.loading = false;
  }

  private async fillForm() {
    let agency = this.agencies.find((ag) => ag.id == this.client.agencyId);
    let plan = await this.cs.GetPlanByCover(this.client.coverId);
    this.reactiveForm.get('AgencyId').setValue(agency);
    this.healthPlan.setValue(plan);
    await this.loadCovers(plan.id);
    this.reactiveForm.get('CoverId').setValue(this.client.coverId);
    this.reactiveForm.get('EffectiveDate').setValue(this.client.effectiveDate);
    this.reactiveForm.get('MedicareA').setValue(this.client.medicareA);
    this.reactiveForm.get('MedicareB').setValue(this.client.medicareB);
    if (this.client.tutors.length < 1) this.hasTutor.setValue(0);
    else {
      this.hasTutor.setValue(1);
      this.tutorInformation.get('Name').setValue(this.client.tutors[0].name);
      this.tutorInformation
        .get('LastName')
        .setValue(this.client.tutors[0].lastName);
      this.tutorInformation.get('Phone').setValue(this.client.tutors[0].phone);
      this.tutorInformation
        .get('ClientId')
        .setValue(this.client.tutors[0].clientId);
      this.tutorInformation.get('Id').setValue(this.client.tutors[0].id);
    }
    this.hasTutor.valueChanges.subscribe((val) => {
      if (!val) {
        this.tutorInformation.get('Name').reset();
        this.tutorInformation.get('LastName').reset();
        this.tutorInformation.get('Phone').reset();
      }
    });

    this.clientService.toggleEditControl.subscribe((val) => {
      this.toggleControls(val);
    });
  }

  async loadCovers(selection) {
    this.covers = await this.cs.GetByPlan(selection);
  }

  displayFnAgencies(selected: any) {
    if (selected) return selected.name;
  }

  displayFnMedicalPlan(selected: any) {
    if (selected) return selected.name;
  }

  toggleControls(disable: boolean) {
    if (this.reactiveForm) {
      if (disable) {
        this.reactiveForm.disable();
        this.healthPlan.disable();
        this.hasTutor.disable();
        this.tutorInformation.disable();
      } else {
        this.reactiveForm.enable();
        this.healthPlan.enable();
        this.hasTutor.enable();
        this.tutorInformation.enable();
      }
    }
  }
}
