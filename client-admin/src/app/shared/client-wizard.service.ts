import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Injectable({
  providedIn: 'root',
})
export class ClientWizardService {
  constructor(private formBuilder: FormBuilder) {}

  resetFormGroups() {
    this.clientDemographic.reset();
    this.clientAddress.reset();
    this.secondFormGroup.reset();
  }

  clientDemographic = this.formBuilder.group({
    Id: [null],
    Name: [null, [Validators.required]],
    LastName1: [null, [Validators.required]],
    LastName2: [null],
    Email: [null, [Validators.email]],
    Initial: [null],
    Ssn: [null, Validators.required],
    Gender: [null],
    BirthDate: [null, Validators.required],
    MaritalStatus: [null],
    Phone1: [null],
    Phone2: [null],
  });

  clientAddress = this.formBuilder.group({
    PhysicalAddress: this.formBuilder.group({
      Id: [null],
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
      Line1: [null],
      Type: [null],
      Line2: [null],
      State: [null],
      City: [null],
      Zipcode: [null],
      Zip4: [null],
    }),
  });

  secondFormGroup = this.formBuilder.group({
    secondCtrl: [null, Validators.required],
  });
}
