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
import { validateBasis } from '@angular/flex-layout';

@Injectable({
  providedIn: 'root',
})
export class ClientWizardService {
  DependantsList = [];
  constructor(
    private formBuilder: FormBuilder,
    private languageService: LanguageService,
    private app: AppService,
    private bonafideService: bonaFideservice,
    private clientService: ClientService
  ) {}

  BonafideList = new Array();

  clientDemographic = this.formBuilder.group({
    Id: [null],
    Name: [null, [Validators.required, Validators.maxLength(250)]],
    LastName1: [null, [Validators.required, Validators.maxLength(250)]],
    LastName2: [null, [Validators.maxLength(250)]],
    Email: [null, [Validators.email, Validators.maxLength(250)]],
    Initial: [null, [Validators.maxLength(1)]],
    Ssn: [
      null,
      [Validators.required, Validators.minLength(4), Validators.maxLength(9)],
    ],
    Gender: [null, [Validators.required]],
    BirthDate: [{ value: null, disabled: true }, [Validators.required]],
    MaritalStatus: [null],
    Phone1: [null, [Validators.required]],
    Phone2: [null],
  });

  tutorInformation = this.formBuilder.group({
    Id: [''],
    ClientId: [''],
    Name: ['', [Validators.maxLength(250)]],
    LastName: ['', [Validators.maxLength(250)]],
    Phone: [''],
  });

  clientAddressFormGroup = this.formBuilder.group({
    PhysicalAddress: this.formBuilder.group({
      Id: [null],
      ClientId: [null],
      Line1: [null, [Validators.maxLength(250), Validators.required]],
      Type: [null],
      Line2: [null, Validators.maxLength(250)],
      State: [null, [Validators.required]],
      City: [null, [Validators.required]],
      Zipcode: [
        null,
        [
          Validators.pattern(new RegExp('[0-9]{5}(-[0-9]{5})?')),
          Validators.required,
        ],
      ],
      Zip4: [
        null,
        [
          Validators.pattern(new RegExp('[0-9]{4}(-[0-9]{4})?')),
          Validators.required,
        ],
      ],
    }),
    PostalAddress: this.formBuilder.group({
      Id: [null],
      ClientId: [null],
      Line1: [null, [Validators.maxLength(250), Validators.required]],
      Type: [null],
      Line2: [null, [Validators.maxLength(250), Validators.required]],
      State: [null, [Validators.required]],
      City: [null, [Validators.required]],
      Zipcode: [
        null,
        [
          Validators.pattern(new RegExp('[0-9]{5}(-[0-9]{5})?')),
          Validators.required,
        ],
      ],
      Zip4: [
        null,
        [
          Validators.pattern(new RegExp('[0-9]{4}(-[0-9]{4})?')),
          Validators.required,
        ],
      ],
    }),
  });

  generalInformationForm = this.formBuilder.group({
    AgencyId: [null],
    CoverId: [null],
    EffectiveDate: [{ value: null, disabled: true }],
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
    this.BonafideList = [];
    this.DependantsList = [];
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
        Bonafides: this.BonafideList,
        Dependants: this.DependantsList,
      };
      await this.clientService.create(ClientInforation);
      if (agency) this.adaptInfoForGUI(agency);
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
      this.clientDemographic.getRawValue(),
      this.generalInformationForm.getRawValue()
    );
    buff['Tutors'] = [this.tutorInformation.value];
    // console.log(buff);
    return buff;
  }

  private adaptInfoForGUI(agency: any) {
    // console.log(agency);
    this.generalInformationForm.get('AgencyId').setValue(agency);
  }

  private adaptInfoForAPI() {
    var Agency = this.generalInformationForm.get('AgencyId').value;
    if (Agency) this.generalInformationForm.get('AgencyId').setValue(Agency.id);
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
      //console.log(ssn, control.value);
      if (control.value && ssn != control.value && control.value.length > 4) {
        const res: any = await this.clientService.checkSsn({
          ssn: control.value,
        });
        if (res) return { ssnTaken: true };
      }
      return null;
    };
  }
}
