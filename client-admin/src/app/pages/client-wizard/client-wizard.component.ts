import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ClientWizardService } from '@app/shared/client-wizard.service';

@Component({
  selector: 'app-client-wizard',
  templateUrl: './client-wizard.component.html',
  styleUrls: ['./client-wizard.component.css'],
})
export class ClientWizardComponent implements OnInit, OnDestroy {
  isLinear: boolean = true;

  constructor(
    public dialogRef: MatDialogRef<ClientWizardComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public wizardForms: ClientWizardService
  ) {}
  ngOnDestroy(): void {
    this.wizardForms.BonafideList = [];
  }

  ngOnInit(): void {}

  onNoClick(): void {
    //this.dialogRef.close();
  }

  async register() {
    await this.wizardForms.register().then((res) => {
      this.dialogRef.close();
    });
  }

  async preRegister() {
    await this.wizardForms.preRegister().then((res) => {
      this.dialogRef.close();
    });
  }
}
