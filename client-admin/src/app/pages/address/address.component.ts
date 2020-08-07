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
    /*
      if the component is called from the wizard
      the formGroup holder will reside in the service
      for the wizard
    */
    if (!this.fromWizard) {
      var addresses: any = await this.addressService.getClientAddress(
        this.clientid
      );
      var physicalAddress = addresses.find((addr) => addr.type == 1);
      var postalAddress = addresses.find((addr) => addr.type == 2);

      this.reactiveForm = this.fb.group({
        Id: [addresses.id],
        PhysicalAddress: this.fb.group({
          Type: [1],
          Line1: [physicalAddress.line1],
          Line2: [physicalAddress.line2],
          State: [physicalAddress.state],
          City: [physicalAddress.city],
          Zipcode: [physicalAddress.zipcode],
          Zip4: [physicalAddress.zip4],
        }),
        PostalAddress: this.fb.group({
          Type: [2],
          Line1: [postalAddress.line1],
          Line2: [postalAddress.line2],
          State: [postalAddress.state],
          City: [postalAddress.city],
          Zipcode: [postalAddress.zipcode],
          Zip4: [postalAddress.zip4],
        }),
      });
      this.toggleControls(true);
    } else {
      this.toggleControls(false);
      this.reactiveForm = this.clientWizard.clientAddress;
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
