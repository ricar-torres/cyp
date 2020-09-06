import { Component, OnInit, Inject, OnDestroy, ViewChild } from '@angular/core';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatStepper,
  MatDialog,
} from '@angular/material';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ClientWizardService } from '@app/shared/client-wizard.service';
import { AllianceWizardComponent } from '../alliance-wizard/alliance-wizard.component';

@Component({
  selector: 'app-client-wizard',
  templateUrl: './client-wizard.component.html',
  styleUrls: ['./client-wizard.component.css'],
})
export class ClientWizardComponent implements OnInit, OnDestroy {
  isLinear: boolean = true;

  @ViewChild('stepper') stepper: MatStepper;

  constructor(
    public dialogRef: MatDialogRef<ClientWizardComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public wizardForms: ClientWizardService,
    private dialog: MatDialog
  ) {}
  ngOnDestroy(): void {
    this.wizardForms.BonafideList = [];
  }

  ngOnInit(): void {}

  onNoClick(): void {
    //this.dialogRef.close();
  }

  async register(option?: number) {
    await this.wizardForms.register().then((res) => {
      console.log(res);
      if (option == 1) {
        const dialogRefs = this.dialog.open(AllianceWizardComponent, {
          width: '70%',
          height: '70%',
          disableClose: true,
          data: { clientid: (<any>res).id },
        });
      }
      this.dialogRef.close();
    });
  }

  async preRegister() {
    this.wizardForms.generalInformationForm.markAsUntouched();
    this.wizardForms.clientDemographic.markAllAsTouched();
    this.wizardForms.clientAddressFormGroup.markAsUntouched();

    if (this.wizardForms.clientDemographic.valid) {
      await this.wizardForms.preRegister().then((res) => {
        this.dialogRef.close();
      });
    }
  }

  canContinue() {
    console.log('called');
    this.wizardForms.generalInformationForm.markAllAsTouched();
    this.wizardForms.clientDemographic.markAllAsTouched();
    this.wizardForms.clientAddressFormGroup.markAllAsTouched();
    var canContinue = true;
    if (!this.wizardForms.generalInformationForm.get('AgencyId').value) {
      this.wizardForms.generalInformationForm
        .get('AgencyId')
        .setErrors({ required: true });
      canContinue = false;
    } else {
      canContinue = true;
      this.wizardForms.generalInformationForm.get('AgencyId').setErrors(null);
    }

    if (
      (<FormGroup>(
        this.wizardForms.clientAddressFormGroup.get('PhysicalAddress')
      )).invalid
    ) {
      (<FormGroup>(
        this.wizardForms.clientAddressFormGroup.get('PhysicalAddress')
      )).markAllAsTouched();
      canContinue = false;
    }
    if (
      (<FormGroup>this.wizardForms.clientAddressFormGroup.get('PostalAddress'))
        .invalid
    ) {
      (<FormGroup>(
        this.wizardForms.clientAddressFormGroup.get('PostalAddress')
      )).markAllAsTouched();
      canContinue = false;
    }

    if (canContinue) {
      this.stepper.next();
    }
  }

  close() {
    this.dialogRef.close();
  }
}
