import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Injectable({
  providedIn: 'root',
})
export class ClientWizardService {
  clientDemographic = this.formBuilder.group({
    Name: ['', [Validators.required]],
    LastName1: ['', [Validators.required]],
    LastName2: [''],
    Email: ['', [Validators.email]],
    Initial: [''],
    Ssn: ['', Validators.required],
    Gender: [''],
    BirthDate: ['', Validators.required],
    MaritalStatus: [''],
    Phone1: [''],
    Phone2: [''],
    PhysicalAddress: this.formBuilder.group({
      AddressLine1: [''],
      AddressLine2: [''],
      Country: [''],
      City: [''],
      PostalCode: [''],
    }),
    PostalAddress: this.formBuilder.group({
      AddressLine1: [''],
      AddressLine2: [''],
      Country: [''],
      City: [''],
      PostalCode: [''],
    }),
    Comments: [''],
  });
  secondFormGroup = this.formBuilder.group({
    secondCtrl: ['', Validators.required],
  });

  constructor(private formBuilder: FormBuilder) {}
}
