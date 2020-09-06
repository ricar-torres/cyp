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
import { AllianceComponent } from '../alliance/alliance.component';
import { AllianceListComponent } from '../alliance-list/alliance-list.component';
import { AlliancesService } from '@app/shared/alliances.service';
import { GeneralInformationComponent } from '../general-information/general-information.component';
import { AddressComponent } from '../address/address.component';
import {
  ConfirmDialogModel,
  ConfirmDialogComponent,
} from '@app/components/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css'],
})
export class ClientComponent implements OnInit, OnDestroy {
  clientid: string;
  client;
  loading: boolean;
  loadingBonafide: boolean = true;
  loadingCalls: boolean = true;
  loadingDepen: boolean = true;
  @Input() fromWizard: boolean = false;
  @ViewChild('dependants')
  dependants: DependantsListComponent;
  @ViewChild('docsCalls')
  docsCalls: DocsCallsListComponent;
  @ViewChild('BonafideList')
  bonafideList: BonaFideListComponent;
  @ViewChild('clientAddressComponent')
  clientAddressComponent: AddressComponent;

  @ViewChild('alliance') alliance: AllianceListComponent;
  @ViewChild('generalInformation')
  generalInformation: GeneralInformationComponent;
  taskPermissions: PERMISSION = {
    read: true, //this.app.checkMenuRoleAccess(MenuRoles.CLIENT_CREATE),
    create: true, //this.app.checkMenuRoleAccess(MenuRoles.CLIENT_CREATE),
    update: true, //this.app.checkMenuRoleAccess(MenuRoles.CLIENT_UPDATE),
    delete: true, //this.app.checkMenuRoleAccess(MenuRoles.CLIENT_DELETE),
    upload: true,
  };

  reactiveForm: FormGroup;
  deceased: boolean = false;

  maxDate = new Date(new Date().setFullYear(new Date().getFullYear() - 1));

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
    private clientWizard: ClientWizardService,
    private alianceService: AlliancesService,
    private dialog: MatDialog
  ) {}
  ngOnDestroy(): void {
    this.clientWizard.resetFormGroups();
  }

  async ngOnInit() {
    this.loading = true;
    this.clientsService.toggleEditControl.subscribe((val) => {
      this.toggleControls(val);
    });
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
      if (this.client.status == 3) {
        console.log('disable all');
        this.toggleControls(true);
      }
    } else {
      this.reactiveForm
        .get('Ssn')
        .setAsyncValidators(this.clientWizard.checkSsn('').bind(this));
    }

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
      case 'Alliance':
        this.alianceService.iselegible(this.clientid).subscribe(
          (res: string[]) => {
            var validationNotMeetText = '';
            var promiseArray: Promise<string>[] = new Array();
            if (res)
              res.forEach((s) =>
                promiseArray.push(
                  this.languageService.translate
                    .get(`CLIENTS.${s}`)
                    .toPromise()
                    .then(
                      (txt) =>
                        (validationNotMeetText += `<div style="margin:20px;" >${txt}</div>`)
                    )
                )
              );

            Promise.all(promiseArray).then(() => {
              if (!res) {
                this.alliance.goToNew();
              } else {
                Swal.default.fire({
                  position: 'center',
                  icon: 'error',
                  title: 'Error',
                  html: validationNotMeetText,
                  showConfirmButton: false,
                  heightAuto: false,
                });
              }
            });
          },
          (err) => {}
        );
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
        icon: 'queue',
        tooltip: 'Multi',
        desc: '',
      },
      {
        icon: 'perm_phone_msg',
        tooltip: 'Calls',
        desc: '',
      },
      {
        icon: 'link',
        tooltip: 'Alliance',
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
        this.clientWizard.generalInformationForm.disable();
        this.clientWizard.clientAddressFormGroup.disable();
        this.bonafideList.deceased = true;
        this.dependants.deceased = true;
        this.alliance.deceased = true;
        this.docsCalls.deceased = true;
        this.clientAddressComponent.sameAsPhysical.disable();
        this.generalInformation.healthPlan.disable();
        this.generalInformation.hasTutor.disable();
        this.deceased = true;
      } else {
        this.reactiveForm.enable();
        this.clientWizard.generalInformationForm.enable();
        this.clientWizard.clientAddressFormGroup.enable();
        this.bonafideList.deceased = false;
        this.dependants.deceased = false;
        this.alliance.deceased = false;
        this.docsCalls.deceased = false;
        this.clientAddressComponent.sameAsPhysical.enable();
        this.generalInformation.healthPlan.enable();
        this.generalInformation.hasTutor.enable();
        this.deceased = true;
      }
    }
  }
  onIsCallsLoading(bool: boolean) {
    this.loadingCalls = bool;
    this.finishedLoading();
  }
  onIsDependentsLoading(bool: boolean) {
    this.loadingDepen = bool;
    this.finishedLoading();
  }
  onIsBonafideLoading(bool: boolean) {
    this.loadingBonafide = bool;
    this.finishedLoading();
  }
  finishedLoading() {
    return (this.loading =
      this.loadingBonafide || this.loadingCalls || this.loadingDepen);
  }

  disableControls() {
    this.clientsService.toggleEditControl.emit(true);
  }

  async markAsDeceased() {
    try {
      await this.clientsService
        .Decesed(this.client.id)
        .toPromise()
        .then(() => {
          this.toggleControls(true);
        });
    } catch (ex) {
      this.app.showErrorMessage(ex);
    }
  }

  async deceaseConfirm() {
    const message = await this.languageService.translate
      .get('DEPENDANTS_LIST.ARE_YOU_SURE_DELETE')
      .toPromise();

    const title = await this.languageService.translate
      .get('COMFIRMATION')
      .toPromise();

    const dialogData = new ConfirmDialogModel(
      title,
      message,
      true,
      true,
      false
    );

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: dialogData,
    });

    dialogRef.afterClosed().subscribe(async (dialogResult) => {
      if (dialogResult) {
        await this.markAsDeceased();
      }
    });
  }
}
