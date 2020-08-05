import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ClientService } from '@app/shared/client.service';
import { ActivatedRoute, Router } from '@angular/router';
import { debug } from 'console';
import { Route } from '@angular/compiler/src/core';
import { faLessThanEqual } from '@fortawesome/free-solid-svg-icons';
import { MatDatepicker, MatDatepickerToggle } from '@angular/material';
import { AppService } from '@app/shared/app.service';
import { MenuRoles, PERMISSION } from '@app/models/enums';
import { LanguageService } from '@app/shared/Language.service';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css'],
})
export class ClientComponent implements OnInit {
  clientid: string;
  birthDateDisabled: boolean = true;

  editSaveToggle: boolean = false;

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
    private languageService: LanguageService
  ) {}

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

  async ngOnInit() {
    this.setupFabButton();
    this.clientid = this.route.snapshot.paramMap.get('id');
    var client: any = await this.clientsService.client(this.clientid);
    if (this.clientid) {
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
      });
      this.disableControls();
    } else {
      this.reactiveForm = this.fb.group({
        Name: ['', [Validators.required]],
        LastName1: ['', [Validators.required]],
        LastName2: [''],
        Email: ['', [Validators.email]],
        Initial: [client.initial],
        Ssn: ['', Validators.required],
        Gender: [''],
        BirthDate: ['', Validators.required],
        MaritalStatus: [''],
      });
    }
  }

  private disableControls() {
    this.reactiveForm.get('Name').disable();
    this.reactiveForm.get('LastName1').disable();
    this.reactiveForm.get('LastName2').disable();
    this.reactiveForm.get('Email').disable();
    this.reactiveForm.get('Initial').disable();
    this.reactiveForm.get('Ssn').disable();
    this.reactiveForm.get('Gender').disable();
    this.reactiveForm.get('BirthDate').disable();
    this.reactiveForm.get('MaritalStatus').disable();
    this.birthDateDisabled = !this.birthDateDisabled;
  }
  private enableControls() {
    this.editSaveToggle = !this.editSaveToggle;
    this.reactiveForm.get('Name').enable();
    this.reactiveForm.get('LastName1').enable();
    this.reactiveForm.get('LastName2').enable();
    this.reactiveForm.get('Email').enable();
    this.reactiveForm.get('Initial').enable();
    this.reactiveForm.get('Ssn').enable();
    this.reactiveForm.get('Gender').enable();
    //uncoment to allow user to enter characters
    //this.reactiveForm.get('BirthDate').enable();
    this.reactiveForm.get('MaritalStatus').enable();
    this.birthDateDisabled = !this.birthDateDisabled;
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
