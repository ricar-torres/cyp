import { Component, OnInit, Input } from '@angular/core';
import { ClientWizardService } from '@app/shared/client-wizard.service';
import { FormGroup, FormControl } from '@angular/forms';
import { HealthPlanService } from '@app/shared/health-plan.service';
import { AgencyService } from '@app/shared/agency.service';
import { CoverService } from '@app/shared/cover.service';

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

  reactiveForm: FormGroup;
  constructor(
    public wizadFormGroups: ClientWizardService,
    private hps: HealthPlanService,
    private ag: AgencyService,
    private cs: CoverService
  ) {}

  async ngOnInit() {
    this.reactiveForm = this.wizadFormGroups.generalInformationForm;
    this.heathPlans = await this.hps.GetAll();
    this.agencies = await this.ag.getAll().toPromise();
    await this.fillForm();
  }

  private async fillForm() {
    let agency = this.agencies.find((ag) => ag.id == this.client.agencyId);
    let plan = await this.cs.GetPlanByCover(this.client.coverId);
    this.reactiveForm.get('AgencyId').setValue(agency);
    this.healthPlan.setValue(plan);
    await this.loadCovers(plan.id);
    this.reactiveForm.get('CoverId').setValue(this.client.coverId);
    console.log(this.client);
    this.reactiveForm.get('EffectiveDate').setValue(this.client.effectiveDate);
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
}
