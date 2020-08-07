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
  });

  clientAddress = this.formBuilder.group({
    PhysicalAddress: this.formBuilder.group({
      Line1: [''],
      Line2: [''],
      State: [''],
      City: [''],
      Zipcode: [''],
      Zipcode4: [''],
    }),
    PostalAddress: this.formBuilder.group({
      Line1: [''],
      Line2: [''],
      State: [''],
      City: [''],
      Zipcode: [''],
      Zip4: [''],
    }),
  });

  secondFormGroup = this.formBuilder.group({
    secondCtrl: ['', Validators.required],
  });

  constructor(private formBuilder: FormBuilder) {}
}
