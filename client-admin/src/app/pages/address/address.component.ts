import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ClientService } from '@app/shared/client.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ClientWizardService } from '@app/shared/client-wizard.service';
import { AddressService } from '@app/shared/address.service';
import { LanguageService } from '@app/shared/Language.service';
import { AppService } from '@app/shared/app.service';

@Component({
  selector: 'app-client-address',
  templateUrl: './address.component.html',
  styleUrls: ['./address.component.css'],
})
export class AddressComponent implements OnInit {
  @Input() fromWizard: boolean = false;
  @Input() clientid: string;

  countries: [];
  cities: [];
  reactiveForm: FormGroup;
  constructor(
    private route: ActivatedRoute,
    private addressService: AddressService,
    private clientsService: ClientService,
    private fb: FormBuilder,
    private clientWizard: ClientWizardService,
    private languageService: LanguageService,
    private app: AppService
  ) {}

  onSubmit() {}

  async ngOnInit() {
    try {
      this.countries = await this.addressService.getCoutries();
      this.cities = await this.addressService.getCities();

      this.reactiveForm = this.clientWizard.clientAddressFormGroup;
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
          this.reactiveForm
            .get('PhysicalAddress')
            .get('ClientId')
            .setValue(physicalAddress.clientId);
          this.reactiveForm
            .get('PhysicalAddress')
            .get('Id')
            .setValue(physicalAddress.id);
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
          this.reactiveForm
            .get('PostalAddress')
            .get('ClientId')
            .setValue(postalAddress.clientId);
          this.reactiveForm
            .get('PostalAddress')
            .get('Id')
            .setValue(postalAddress.id);
        }
      }

      this.clientsService.toggleEditControl.subscribe((val) => {
        this.toggleControls(val);
      });
    } catch (error) {
      if (error.status != 401) {
        console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    }
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
