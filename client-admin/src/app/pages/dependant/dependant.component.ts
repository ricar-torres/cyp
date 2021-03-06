import { HealthPlanService } from './../../shared/health-plan.service';
import { CoverService } from './../../shared/cover.service';
import { DependantsAPIService } from './../../shared/dependants.api.service';
import { Component, OnInit, Inject, Input, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CommunicationMethodsAPIService } from '@app/shared/communication-methods.api.service';
import { AppService } from '@app/shared/app.service';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DocumentationCallComponent } from '@app/components/documentation-call/documentation-call.component';
import { ClientWizardService } from '@app/shared/client-wizard.service';

@Component({
  selector: 'app-dependant',
  templateUrl: './dependant.component.html',
  styleUrls: ['./dependant.component.css'],
})
export class DependantComponent implements OnInit, AfterViewInit {
  loading: boolean;
  dependantId: string | number;
  clientId: string | number;
  dependant: any;
  reactiveForm: FormGroup;
  covers: any = [];
  healthPlans: any = [];
  relations: any = [];
  fromWizard: boolean;
  dependantFromWizard: any;

  constructor(
    public formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private apiDependant: DependantsAPIService,
    private apiCovers: CoverService,
    private apiHealthPlan: HealthPlanService,
    private app: AppService,
    public dialog: MatDialog,
    private clientWizard: ClientWizardService,
    public dialogRef: MatDialogRef<DependantComponent>,
    @Inject(MAT_DIALOG_DATA) data
  ) {
    this.dependantId = data.id;
    this.clientId = data.clientId;
    this.fromWizard = data.fromWizard;
    this.dependantFromWizard = data.dependantFromWizard;
  }
  async ngAfterViewInit() {
    if (this.dependantId) {
      this.dependant = await this.apiDependant.getById(this.dependantId);
      this.reactiveForm.get('id').setValue(this.dependant.id);
      this.reactiveForm.get('name').setValue(this.dependant.name);
      this.reactiveForm.get('initial').setValue(this.dependant.initial);
      this.reactiveForm.get('lastName1').setValue(this.dependant.lastName1);
      this.reactiveForm.get('lastName2').setValue(this.dependant.lastName2);
      this.reactiveForm.get('ssn').setValue(this.dependant.ssn);
      this.reactiveForm.get('birthDate').setValue(this.dependant.birthDate);
      this.reactiveForm.get('email').setValue(this.dependant.email);
      this.reactiveForm.get('gender').setValue(this.dependant.gender);
      this.reactiveForm.get('phone1').setValue(this.dependant.phone1);
      this.reactiveForm.get('phone2').setValue(this.dependant.phone2);
      this.reactiveForm
        .get('relationship')
        .setValue(this.dependant.relationship.id);
      this.reactiveForm
        .get('healthPlanId')
        .setValue(this.dependant.cover.healthPlan.id);
      this.reactiveForm.get('coverId').setValue(this.dependant.cover.id);
      this.reactiveForm
        .get('contractNumber')
        .setValue(this.dependant.contractNumber);
      this.reactiveForm
        .get('effectiveDate')
        .setValue(this.dependant.effectiveDate);
    }
    if (this.dependantFromWizard) {
      debugger;
      var dependatToEdit = this.clientWizard.DependantsList.find(
        (x) => x.ssn == this.dependantFromWizard.ssn
      );
      this.reactiveForm.get('name').setValue(dependatToEdit.name);
      this.reactiveForm.get('initial').setValue(dependatToEdit.initial);
      this.reactiveForm.get('lastName1').setValue(dependatToEdit.lastName1);
      this.reactiveForm.get('lastName2').setValue(dependatToEdit.lastName2);
      this.reactiveForm.get('ssn').setValue(dependatToEdit.ssn);
      this.reactiveForm.get('birthDate').setValue(dependatToEdit.birthDate);
      this.reactiveForm.get('email').setValue(dependatToEdit.email);
      this.reactiveForm.get('gender').setValue(dependatToEdit.gender);
      this.reactiveForm.get('phone1').setValue(dependatToEdit.phone1);
      this.reactiveForm.get('phone2').setValue(dependatToEdit.phone2);
      this.reactiveForm
        .get('relationship')
        .setValue(dependatToEdit.relationship.id);
      this.reactiveForm
        .get('healthPlanId')
        .setValue(dependatToEdit.cover.healthPlan.id);
      this.reactiveForm.get('coverId').setValue(dependatToEdit.cover.id);
      this.reactiveForm
        .get('contractNumber')
        .setValue(dependatToEdit.contractNumber);
      this.reactiveForm
        .get('effectiveDate')
        .setValue(dependatToEdit.effectiveDate);
    }
  }

  async ngOnInit() {
    this.loading = true;
    this.initForm();

    try {
      this.covers = await this.apiCovers.GetAll();
      this.healthPlans = await this.apiHealthPlan.GetAll().toPromise();
      this.relations = await this.apiDependant.getRelationTypes().toPromise();
    } catch (error) {
    } finally {
      this.loading = false;
    }
  }
  initForm() {
    this.reactiveForm = this.formBuilder.group({
      id: this.dependantId,
      clientId: this.clientId,
      name: this.formBuilder.control('', [
        Validators.required,
        Validators.maxLength(250),
      ]),
      initial: this.formBuilder.control('', [Validators.maxLength(1)]),
      lastName1: this.formBuilder.control('', [
        Validators.required,
        Validators.maxLength(250),
      ]),
      lastName2: this.formBuilder.control('', [Validators.maxLength(250)]),
      birthDate: this.formBuilder.control({ value: '', disabled: true }, [
        Validators.required,
      ]),
      ssn: this.formBuilder.control('', [Validators.required]),
      email: this.formBuilder.control('', [Validators.maxLength(250)]),
      gender: this.formBuilder.control('', [Validators.required]),
      phone1: this.formBuilder.control('', [Validators.required]),
      phone2: this.formBuilder.control(''),
      relationship: this.formBuilder.control('', [Validators.required]),
      healthPlanId: this.formBuilder.control(''),
      coverId: this.formBuilder.control('', [Validators.required]),
      contractNumber: this.formBuilder.control(''),
      effectiveDate: this.formBuilder.control({ value: '', disabled: true }),
    });
  }
  async onSubmit() {
    try {
      if (!this.fromWizard) {
        this.loading = true;
        var payload = this.reactiveForm.getRawValue();
        // console.log(JSON.stringify(this.reactiveForm.value));
        if (this.dependantId > 0) {
          const res: any = await this.apiDependant.update(payload);
        } else {
          const res: any = await this.apiDependant.create(payload);
        }
        this.onBack();
      } else {
        if (this.dependantFromWizard) {
          var dependatIndex = this.clientWizard.DependantsList.findIndex(
            (x) => x.ssn == this.reactiveForm.get('ssn').value
          );
          this.clientWizard.DependantsList.splice(dependatIndex, 1);
          var objectToAdd = this.fillWizardDependant();
          this.clientWizard.DependantsList.push(objectToAdd);
          this.dialogRef.close();
        } else {
          var objectToAdd = this.fillWizardDependant();
          this.clientWizard.DependantsList.push(objectToAdd);
          this.dialogRef.close();
        }
      }
    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
        console.error('error', error);
        this.app.showErrorMessage('Error');
      }
    } finally {
      this.loading = false;
    }
  }
  private fillWizardDependant() {
    var cover = this.covers.find(
      (x) => x.id == this.reactiveForm.get('coverId').value
    );
    var healthPlan = this.healthPlans.find(
      (x) => x.id == this.reactiveForm.get('healthPlanId').value
    );
    cover = Object.assign(cover, { healthPlan: healthPlan });
    var relationship = this.relations.find(
      (x) => x.id == this.reactiveForm.get('relationship').value
    );
    var objectToAdd = Object.assign(this.reactiveForm.getRawValue(), {
      cover: cover,
      relationship: relationship,
    });
    return objectToAdd;
  }

  onBack() {
    this.dialogRef.close();
  }
}
