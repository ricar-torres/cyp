import { Component, OnInit, ViewChild, Input, OnDestroy } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
  FormArray,
} from '@angular/forms';
import { ClientService } from '@app/shared/client.service';
import { ActivatedRoute, Router } from '@angular/router';
import { debug } from 'console';
import { Route } from '@angular/compiler/src/core';
import { faLessThanEqual } from '@fortawesome/free-solid-svg-icons';
import { MatDatepicker, MatDatepickerToggle } from '@angular/material';
import { AppService } from '@app/shared/app.service';
import { MenuRoles, PERMISSION } from '@app/models/enums';
import { LanguageService } from '@app/shared/Language.service';
import { ClientWizardService } from '@app/shared/client-wizard.service';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css'],
})
export class ClientComponent implements OnInit, OnDestroy {
  clientid: string;
  client;
  @Input() fromWizard: boolean = false;

  taskPermissions: PERMISSION = {
    read: true, //this.app.checkMenuRoleAccess(MenuRoles.CLIENT_CREATE),
    create: true, //this.app.checkMenuRoleAccess(MenuRoles.CLIENT_CREATE),
    update: true, //this.app.checkMenuRoleAccess(MenuRoles.CLIENT_UPDATE),
    delete: true, //this.app.checkMenuRoleAccess(MenuRoles.CLIENT_DELETE),
    upload: true,
  };

  reactiveForm: FormGroup;

  fabMenuButtons = {
    visible: false,
    buttons: [],
  };
  loading: boolean;
  constructor(
    private fb: FormBuilder,
    private clientsService: ClientService,
    private route: ActivatedRoute,
    private router: Router,
    private app: AppService,
    private languageService: LanguageService,
    private clientWizard: ClientWizardService
  ) {}
  ngOnDestroy(): void {
    this.clientWizard.resetFormGroups();
  }

  async ngOnInit() {
    this.reactiveForm = this.clientWizard.clientDemographic;
    if (!this.fromWizard) {
      this.setupFabButton();
      this.clientid = this.route.snapshot.paramMap.get('id');
      this.client = await this.clientsService.client(this.clientid);
      this.reactiveForm.get('Id').setValue(this.client.id);
      this.reactiveForm.get('Name').setValue(this.client.name);
      this.reactiveForm.get('LastName1').setValue(this.client.lastName1);
      this.reactiveForm.get('LastName2').setValue(this.client.lastName2);
      this.reactiveForm.get('Email').setValue(this.client.email);
      this.reactiveForm.get('Initial').setValue(this.client.initial);
      this.reactiveForm.get('Ssn').setValue(this.client.ssn);
      this.reactiveForm.get('Gender').setValue(this.client.gender);
      this.reactiveForm.get('BirthDate').setValue(this.client.birthDate);
      this.reactiveForm
        .get('MaritalStatus')
        .setValue(this.client.maritalStatus);
      this.reactiveForm.get('Phone1').setValue(this.client.phone1);
      this.reactiveForm.get('Phone2').setValue(this.client.phone2);
    }
    this.clientsService.toggleEditControl.subscribe((val) => {
      this.toggleControls(val);
    });
  }

  async onSubmit() {
    try {
      if (this.fromWizard) {
        //  await this.clientWizard.CreateClient();
      }
      await this.clientWizard.UpdateClientInformation();
    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
        console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    }
  }

  onBack() {
    this.router.navigate(['home/clients']);
  }

  onSpeedDialFabClicked(ev) {
    switch (ev) {
      case 'bonafide':
        break;
      case 'Call':
        break;
      case 'Dependant':
        break;

      default:
        break;
    }
  }

  setupFabButton() {
    this.fabMenuButtons.buttons.push({
      icon: 'list',
      tooltip: 'Bonafide',
      desc: '',
    });

    this.fabMenuButtons.visible =
      this.fabMenuButtons.buttons.length > 0 ? true : false;
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
  onIsCallsLoading(bool: boolean) {
    this.loading = bool;
  }
}
