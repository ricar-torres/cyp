import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ClientService } from '@app/shared/client.service';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { ClientWizardService } from '@app/shared/client-wizard.service';
import { AddressService } from '@app/shared/address.service';
import { LanguageService } from '@app/shared/Language.service';
import { AppService } from '@app/shared/app.service';
import { MatSlideToggleChange } from '@angular/material';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
@Component({
  selector: 'app-client-address',
  templateUrl: './address.component.html',
  styleUrls: ['./address.component.css'],
})
export class AddressComponent implements OnInit {
  @Input() fromWizard: boolean = false;
  @Input() clientid: string;

  sameAsPhysical: FormControl = new FormControl();
  sameAddressSubscription: Observable<any>;
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

  async ngOnInit() {
    try {
      this.countries = await this.addressService.getCoutries();
      this.cities = await this.addressService.getCities();

      this.reactiveForm = this.clientWizard.clientAddressFormGroup;

      this.sameAddressSubscription = this.reactiveForm
        .get('PhysicalAddress')
        .valueChanges.pipe(
          map(() => {
            if (this.sameAsPhysical.value) {
              this.reactiveForm
                .get('PostalAddress')
                .patchValue(this.reactiveForm.get('PhysicalAddress').value);
            }
          })
        );

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

      this.reactiveForm.get('PhysicalAddress').get('State').setValue('PR');
      this.reactiveForm.get('PostalAddress').get('State').setValue('PR');
      this.reactiveForm.get('PhysicalAddress').get('State').disable();
      this.reactiveForm.get('PostalAddress').get('State').disable();

      this.isSameAddress(
        (<FormGroup>this.reactiveForm.get('PhysicalAddress')).value,
        (<FormGroup>this.reactiveForm.get('PostalAddress')).value
      );
    } catch (error) {
      if (error.status != 401) {
        console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    }

    this.clientsService.toggleEditControl.subscribe((val) => {
      this.toggleControls(val);
    });
  }

  isSameAddress(address1, address2) {
    if (
      address1.Line1 == null &&
      address2.Line1 == null &&
      address1.Line2 == null &&
      address2.Line2 == null &&
      address1.City == null &&
      address2.City == null
    ) {
      this.sameAsPhysical.setValue(0);
    } else if (
      address1.Line1 == address2.Line1 &&
      address1.Line2 == address2.Line2 &&
      address1.City == address2.City
    ) {
      this.sameAsPhysical.setValue(1);
    }
  }

  toggleControls(disable: boolean) {
    if (this.reactiveForm) {
      if (disable) {
        this.reactiveForm.disable();
      } else {
        this.reactiveForm.enable();
      }
    }
  }

  usePhysical(ev: MatSlideToggleChange) {
    if (ev.checked) {
      this.reactiveForm
        .get('PostalAddress')
        .patchValue(this.reactiveForm.get('PhysicalAddress').value);
      this.sameAddressSubscription.subscribe();
      return;
    }
    //this.sameAddressSubscription.unsubscribe();
    this.reactiveForm.get('PostalAddress').reset();
  }
}
