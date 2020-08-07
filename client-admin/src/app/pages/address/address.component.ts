import { Component, OnInit, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ClientService } from '@app/shared/client.service';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { ClientWizardService } from '@app/shared/client-wizard.service';

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
      var client: any = await this.clientsService.client(this.clientid);
      this.reactiveForm = this.fb.group({
        Id: [client.id],
        PhysicalAddress: this.fb.group({
          Type: [1],
          Line1: [client.physicalAddress.line1],
          Line2: [client.physicalAddress.line2],
          State: [client.physicalAddress.state],
          City: [client.physicalAddress.city],
          Zipcode: [client.physicalAddress.zipcode],
          Zip4: [client.physicalAddress.zip4],
        }),
        PostalAddress: this.fb.group({
          Type: [2],
          Line1: [client.postalAddress.line1],
          Line2: [client.postalAddress.line2],
          State: [client.postalAddress.state],
          City: [client.postalAddress.city],
          Zipcode: [client.postalAddress.zipcode],
          Zip4: [client.postalAddress.zip4],
        }),
      });
      this.toggleControls(true);
    } else {
      this.toggleControls(false);
      this.reactiveForm = this.clientWizard.clientDemographic;
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
