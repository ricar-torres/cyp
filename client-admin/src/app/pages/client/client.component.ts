import { DocsCallsListComponent } from './../docs-calls-list/docs-calls-list.component';
import { DependantsListComponent } from './../dependants-list/dependants-list.component';
import { Component, OnInit, ViewChild, Input, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ClientService } from '@app/shared/client.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '@app/shared/app.service';
import { PERMISSION } from '@app/models/enums';
import { LanguageService } from '@app/shared/Language.service';
import { ClientWizardService } from '@app/shared/client-wizard.service';
import { BonaFideListComponent } from '../bona-fide-list/bona-fide-list.component';
import * as Swal from 'sweetalert2';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css'],
})
export class ClientComponent implements OnInit, OnDestroy {
  clientid: string;
  client;
  loading = true;
  @Input() fromWizard: boolean = false;
  @ViewChild('dependants')
  dependants: DependantsListComponent;
  @ViewChild('docsCalls')
  docsCalls: DocsCallsListComponent;
  @ViewChild('BonafideList')
  bonafideList: BonaFideListComponent;
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
  constructor(
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
      this.reactiveForm
        .get('Ssn')
        .setAsyncValidators(
          this.clientWizard.checkSsn(this.client.ssn).bind(this)
        );
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
    } else {
      this.reactiveForm
        .get('Ssn')
        .setAsyncValidators(this.clientWizard.checkSsn('').bind(this));
    }
    this.clientsService.toggleEditControl.subscribe((val) => {
      this.toggleControls(val);
    });
    this.loading = false;
  }

  async onSubmit() {
    try {
      if (this.fromWizard) {
        //  await this.clientWizard.CreateClient();
      } else {
        var successMessage = await this.languageService.translate
          .get('SAVESUCCESS')
          .toPromise();

        await this.clientWizard.UpdateClientInformation().then(() => {
          Swal.default.fire({
            position: 'center',
            icon: 'success',
            title: successMessage,
            showConfirmButton: false,
            timer: 1300,
            heightAuto: false,
          });
        });
      }
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

  async onSpeedDialFabClicked(ev) {
    switch (ev.tooltip) {
      case 'Bonafide':
        this.bonafideList.goToNew();
        break;
      case 'Calls':
        await this.docsCalls.createThread();
        break;
      case 'Dependents':
        this.dependants.goToNew();
        break;
      default:
        break;
    }
  }

  setupFabButton() {
    this.fabMenuButtons.buttons.push(
      {
        icon: 'group_work',
        tooltip: 'Bonafide',
        desc: '',
      },
      {
        icon: 'insert_emoticon',
        tooltip: 'Dependents',
        desc: '',
      },
      {
        icon: 'link',
        tooltip: 'Alianza',
        desc: '',
      },
      {
        icon: 'queue',
        tooltip: 'Multi',
        desc: '',
      },
      {
        icon: 'perm_phone_msg',
        tooltip: 'Calls',
        desc: '',
      }
    );

    this.fabMenuButtons.visible =
      this.fabMenuButtons.buttons.length > 0 ? true : false;
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
  onIsCallsLoading(bool: boolean) {
    this.loading = bool;
  }

  disableControls() {
    this.clientsService.toggleEditControl.emit(true);
  }
}
