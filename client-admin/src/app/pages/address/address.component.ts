import { Component, OnInit, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ClientService } from '@app/shared/client.service';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { ClientWizardService } from '@app/shared/client-wizard.service';
import { AddressService } from '@app/shared/address.service';

@Component({
  selector: 'app-client-address',
  templateUrl: './address.component.html',
  styleUrls: ['./address.component.css'],
})
export class AddressComponent implements OnInit {
  @Input() fromWizard: boolean = false;
  @Input() clientid: string;
  reactiveForm: FormGroup;
  constructor(
    private route: ActivatedRoute,
    private addressService: AddressService,
    private clientsService: ClientService,
    private fb: FormBuilder,
    private clientWizard: ClientWizardService
  ) {}

  onSubmit() {}

  async ngOnInit() {
    this.reactiveForm = this.clientWizard.clientAddress;
    if (!this.fromWizard) {
      var addresses: any = await this.addressService.getClientAddress(
        this.clientid
      );

      var physicalAddress = addresses.find((addr) => addr.type == 1);
      var postalAddress = addresses.find((addr) => addr.type == 2);
      if (physicalAddress) {
        this.reactiveForm
          .get('PhysicalAddress')
          .get('Type')
          .setValue(physicalAddress.type);
        this.reactiveForm
          .get('PhysicalAddress')
          .get('Line1')
          .setValue(physicalAddress.line1);
        this.reactiveForm
          .get('PhysicalAddress')
          .get('Line2')
          .setValue(physicalAddress.line2);
        this.reactiveForm
          .get('PhysicalAddress')
          .get('State')
          .setValue(physicalAddress.state);
        this.reactiveForm
          .get('PhysicalAddress')
          .get('City')
          .setValue(physicalAddress.city);
        this.reactiveForm
          .get('PhysicalAddress')
          .get('Zipcode')
          .setValue(physicalAddress.zipcode);
        this.reactiveForm
          .get('PhysicalAddress')
          .get('Zip4')
          .setValue(physicalAddress.zip4);
      }

      if (postalAddress) {
        this.reactiveForm
          .get('PostalAddress')
          .get('Type')
          .setValue(postalAddress.type);
        this.reactiveForm
          .get('PostalAddress')
          .get('Line1')
          .setValue(postalAddress.line1);
        this.reactiveForm
          .get('PostalAddress')
          .get('Line2')
          .setValue(postalAddress.line2);
        this.reactiveForm
          .get('PostalAddress')
          .get('State')
          .setValue(postalAddress.state);
        this.reactiveForm
          .get('PostalAddress')
          .get('City')
          .setValue(postalAddress.city);
        this.reactiveForm
          .get('PostalAddress')
          .get('Zipcode')
          .setValue(postalAddress.zipcode);
        this.reactiveForm
          .get('PostalAddress')
          .get('Zip4')
          .setValue(postalAddress.zip4);
      }
    }

    this.clientsService.toggleEditControl.subscribe((val) => {
      this.toggleControls(val);
    });
  }

  toggleControls(disable: boolean) {
    if (this.reactiveForm) {
      for (var property in this.reactiveForm.controls) {
        if (this.reactiveForm.controls.hasOwnProperty(property)) {
          if (disable) {
            this.reactiveForm.get(property).disable();
          } else {
            this.reactiveForm.get(property).enable();
          }
        }
      }
    }
  }
}
