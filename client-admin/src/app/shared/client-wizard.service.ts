import { Injectable, OnDestroy } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormControl,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { LanguageService } from './Language.service';
import { AppService } from './app.service';
import { bonaFideservice } from './bonafide.service';
import { ClientService } from './client.service';
import { debug } from 'console';

@Injectable({
  providedIn: 'root',
})
export class ClientWizardService {
  constructor(
    private formBuilder: FormBuilder,
    private languageService: LanguageService,
    private app: AppService,
    private bonafideService: bonaFideservice,
    private clientService: ClientService
  ) {}

  clientDemographic = this.formBuilder.group({
    Id: [null],
    Name: [null, [Validators.required]],
    LastName1: [null, [Validators.required]],
    LastName2: [null],
    Email: [null, [Validators.email]],
    Initial: [null],
    Ssn: [null, [Validators.required]],
    Gender: [null],
    BirthDate: [null],
    MaritalStatus: [null],
    Phone1: [null, [Validators.required]],
    Phone2: [null],
  });

  tutorInformation = this.formBuilder.group({
    Id: [''],
    ClientId: [''],
    Name: [''],
    LastName: [''],
    Phone: [''],
  });

  clientAddressFormGroup = this.formBuilder.group({
    PhysicalAddress: this.formBuilder.group({
      Id: [null],
      ClientId: [null],
      Line1: [null],
      Type: [null],
      Line2: [null],
      State: [null],
      City: [null],
      Zipcode: [null],
      Zip4: [null],
    }),
    PostalAddress: this.formBuilder.group({
      Id: [null],
      ClientId: [null],
      Line1: [null],
      Type: [null],
      Line2: [null],
      State: [null],
      City: [null],
      Zipcode: [null],
      Zip4: [null],
    }),
  });

  generalInformationForm = this.formBuilder.group({
    AgencyId: [null],
    CoverId: [null],
    EffectiveDate: [null],
    HealthPlan: [null],
    MedicareA: [null],
    MedicareB: [null],
  });

  secondFormGroup = this.formBuilder.group({
    secondCtrl: [null, Validators.required],
  });

  resetFormGroups() {
    this.clientDemographic.reset();
    this.clientAddressFormGroup.reset();
    this.secondFormGroup.reset();
    this.generalInformationForm.reset();
    this.tutorInformation.reset();
  }

  async preRegister() {
    try {
      var agency = this.adaptInfoForAPI();
      var clientDemographic = this.addValues();
      var ClientInforation = {
        PreRegister: true,
        Demographic: clientDemographic,
        Address: this.clientAddressFormGroup.value,
      };
      await this.clientService.create(ClientInforation);
      this.adaptInfoForGUI(agency);
    } catch (error) {
      if (error.status != 401) {
        console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    }
  }

  async register() {
    try {
      var agency = this.adaptInfoForAPI();
      var clientDemographic = this.addValues();
      var ClientInforation = {
        PreRegister: false,
        Demographic: clientDemographic,
        Address: this.clientAddressFormGroup.value,
      };
      await this.clientService.create(ClientInforation);
      this.adaptInfoForGUI(agency);
    } catch (error) {
      if (error.status != 401) {
        console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    }
  }

  async UpdateClientInformation() {
    try {
      var agency = this.adaptInfoForAPI();
      var clientDemographic = this.addValues();
      var ClientInforation = {
        Demographic: clientDemographic,
        Address: this.clientAddressFormGroup.value,
      };
      await this.clientService.update(ClientInforation);
      this.adaptInfoForGUI(agency);
    } catch (error) {
      if (error.status != 401) {
        console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    }
  }

  private addValues() {
    var buff = Object.assign(
      this.clientDemographic.value,
      this.generalInformationForm.value
    );
    buff['Tutors'] = [this.tutorInformation.value];
    console.log(buff);
    return buff;
  }

  private adaptInfoForGUI(agency: any) {
    console.log(agency);
    this.generalInformationForm.get('AgencyId').setValue(agency);
  }

  private adaptInfoForAPI() {
    var Agency = this.generalInformationForm.get('AgencyId').value;
    this.generalInformationForm.get('AgencyId').setValue(Agency.id);
    return Agency;
  }

  async checkClientSsn(control: FormControl) {
    if (control.value) {
      const res: any = await this.clientService.checkSsn({
        ssn: control.value,
      });
      if (res) return { ssnTaken: true };
    }
  }

  checkSsn(ssn: string) {
    return async (control: AbstractControl) => {
      if (control.value && ssn != control.value) {
        const res: any = await this.clientService.checkSsn({
          ssn: control.value,
        });
        if (res) return { ssnTaken: true };
      }
      return null;
    };
  }
}
