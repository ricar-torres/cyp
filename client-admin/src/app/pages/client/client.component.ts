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
export class ClientComponent implements OnInit {
  clientid: string;
  editSaveToggle: boolean = false;

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

  async ngOnInit() {
    /*
      if the component is called from the wizard
      the formGroup holder will reside in the service
      for the wizard
    */
    if (!this.fromWizard) {
      this.setupFabButton();
      this.clientid = this.route.snapshot.paramMap.get('id');
      if (this.clientid) {
        var client: any = await this.clientsService.client(this.clientid);
        console.log(client);
        this.reactiveForm = this.fb.group({
          Id: [client.id],
          Name: [client.name, [Validators.required]],
          LastName1: [client.lastName1, [Validators.required]],
          LastName2: [client.lastName2],
          Email: [client.email, [Validators.email]],
          Initial: [client.initial],
          Ssn: [client.ssn, Validators.required],
          Gender: [client.gender],
          BirthDate: [client.birthDate, Validators.required],
          MaritalStatus: [client.maritalStatus],
          Phone1: [client.phone1],
          Phone2: [client.phone2],
        });
      } else {
        this.reactiveForm = this.fb.group({
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
      }
      this.toggleControls(true);
    } else {
      this.toggleControls(false);
      this.reactiveForm = this.clientWizard.clientDemographic;
    }
    this.clientsService.toggleEditControl.subscribe((val) => {
      this.toggleControls(val);
    });
  }

  async onSubmit() {
    try {
      console.log(this.reactiveForm.value);
      await this.clientsService.update(this.reactiveForm.value);
    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
        console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    }
    this.disableControls();
    this.editSaveToggle = !this.editSaveToggle;
  }

  private disableControls() {
    this.clientsService.toggleEditControl.emit(true);
  }
  private enableControls() {
    this.editSaveToggle = !this.editSaveToggle;
    this.clientsService.toggleEditControl.emit(false);
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
}
