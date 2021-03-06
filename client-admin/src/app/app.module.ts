import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
  HTTP_INTERCEPTORS,
  HttpClientModule,
  HttpClient,
} from '@angular/common/http';
import { httpInterceptorProviders } from './http-interceptors/index';

import { MatSliderModule } from '@angular/material/slider';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialogModule } from '@angular/material/dialog';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatMenuModule } from '@angular/material/menu';

import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSelectModule } from '@angular/material/select';
import {
  MatPaginatorModule,
  MatPaginatorIntl,
} from '@angular/material/paginator';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material';
import { MatRadioModule } from '@angular/material/radio';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatTabsModule } from '@angular/material/tabs';
import { MatBadgeModule } from '@angular/material/badge';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MomentModule } from 'ngx-moment';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { DatePipe, CurrencyPipe } from '@angular/common';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import {
  FormsModule,
  ReactiveFormsModule,
  FormGroup,
  FormControl,
  Validators,
} from '@angular/forms';

import { DialogGenericErrorComponent } from './components/dialog-generic-error/dialog-generic-error.component';
import { DialogGenericSuccessComponent } from './components/dialog-generic-success/dialog-generic-success.component';
import { NgxSpinnerModule } from 'ngx-spinner';

import {
  TranslateModule,
  TranslateLoader,
  TranslateService,
} from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { SnackBarConfirmationComponent } from './components/snack-bar-confirmation/snack-bar-confirmation.component';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FlexLayoutModule } from '@angular/flex-layout';

import {
  MatSidenavModule,
  MatTableModule,
  MatCheckboxModule,
  MatExpansionModule,
  MatSortModule,
} from '@angular/material';

// import { CustomFormsModule } from 'ngx-validators';

import { SsnInputDirective } from './directives/ssn-input.directive';
import { LoginComponent } from './pages/login/login.component';
import { HomeComponent } from './pages/home/home.component';
import { SideNavComponent } from './components/side-nav/side-nav.component';
import { LoaderComponent } from './components/loader/loader.component';
import { UserListComponent } from './pages/user-list/user-list.component';
import { UserComponent } from './pages/user/user.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { DragNDropZoneComponent } from './components/drag-n-drop-zone/drag-n-drop-zone.component';
import { DragNDropDirective } from './directives/drag-n-drop.directive';
import { ProgressComponent } from './components/progress/progress.component';
import { SpeedDialFabComponent } from './components/speed-dial-fab/speed-dial-fab.component';
import { ChangePasswordComponent } from './pages/change-password/change-password.component';
import { PaginatorI18n } from './shared/PaginatorI18n';
import { AccessDeniedComponent } from './pages/access-denied/access-denied.component';
import { CampaignListComponent } from './pages/campaign-list/campaign-list.component';
import { CampaignComponent } from './pages/campaign/campaign.component';
import { AgencyListComponent } from './pages/agencies/agency-list/agency-list.component';
import { AgencyComponent } from './pages/agencies/agency/agency.component';
import { CommunicationMethodsListComponent } from './pages/communication-methods-list/communication-methods-list.component';
import { CommunicationMethodComponent } from './pages/communication-method/communication-method.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { RetirementListComponent } from './pages/retirement-list/retirement-list.component';
import { BonaFideListComponent } from './pages/bona-fide-list/bona-fide-list.component';
import { BonaFideComponent } from './pages/bona-fide/bona-fide.component';
import { RetirementComponent } from './pages/retirement/retirement.component';
import { ChapterListComponent } from './pages/chapter-list/chapter-list.component';
import { ChapterComponent } from './pages/chapter/chapter.component';
import { NgxMaskModule } from 'ngx-mask';
import { QualifyingEventComponent } from './pages/qualifying-event/qualifying-event.component';
import { QualifyingEventListComponent } from './pages/qualifying-event-list/qualifying-event-list.component';
import { ClientListComponent } from './pages/client-list/client-list.component';
import { ClientComponent } from './pages/client/client.component';
import { ClientWizardComponent } from './pages/client-wizard/client-wizard.component';
import { MatStepperModule } from '@angular/material/stepper';
import { AddressComponent } from './pages/address/address.component';
import { GeneralInformationComponent } from './pages/general-information/general-information.component';
import { DocumentationCallComponent } from './components/documentation-call/documentation-call.component';
import { DocsCallsListComponent } from './pages/docs-calls-list/docs-calls-list.component';
import { DialogSuccessComponent } from './components/dialog-success/dialog-success.component';
import { DependantsListComponent } from './pages/dependants-list/dependants-list.component';
import { BonafidesAssociatorComponent } from './pages/bonafides-associator/bonafides-associator.component';
import { DependantComponent } from './pages/dependant/dependant.component';
import { PreviewSsn } from './directives/ssnPipe';
import { AllianceComponent } from './pages/alliance/alliance.component';
import { AllianceListComponent } from './pages/alliance-list/alliance-list.component';
import { AllianceWizardComponent } from './pages/alliance-wizard/alliance-wizard.component';
import { BeneficiariesBenefitDistributionComponent } from './components/beneficiaries-benefit-distribution/beneficiaries-benefit-distribution.component';
import { InsuranceCompanyComponent } from './pages/insurance-company/insurance-company.component';
import {
  InsuranceCompanyItemComponent,
  AddOnsDialog,
} from './pages/insurance-company-item/insurance-company-item.component';
import {
  InsurancePlanItemComponent,
  BenefitTypeDialog,
} from './pages/insurance-plan-item/insurance-plan-item.component';
import { MultiAssistComponent } from './pages/multi-assist/multi-assist.component';
import { MultiAssistListComponent } from './pages/multi-assist-list/multi-assist-list.component';
import { VehicleListComponent } from './components/vehicle-list/vehicle-list.component';
import { AllianceInscriptionSheetComponent } from './pages/alliance-inscription-sheet-psm/alliance-inscription-sheet.component';
import { AllianceInscriptionSheetFirtsMedicalComponent } from './pages/alliance-inscription-sheet-firts-medical/alliance-inscription-sheet-firts-medical.component';
import { DatepickerComponent } from './components/datepicker/datepicker.component';

// AoT requires an exported function for factories
export function HttpLoaderFactory(httpClient: HttpClient) {
  //return new TranslateHttpLoader(httpClient);
  return new TranslateHttpLoader(httpClient, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
    DialogGenericErrorComponent,
    DialogGenericSuccessComponent,
    SnackBarConfirmationComponent,
    SsnInputDirective,
    LoginComponent,
    HomeComponent,
    SideNavComponent,
    LoaderComponent,
    UserListComponent,
    UserComponent,
    DashboardComponent,
    DragNDropZoneComponent,
    DragNDropDirective,
    ProgressComponent,
    SpeedDialFabComponent,
    ChangePasswordComponent,
    AccessDeniedComponent,
    CampaignListComponent,
    CampaignComponent,
    AgencyListComponent,
    AgencyComponent,
    CommunicationMethodsListComponent,
    CommunicationMethodComponent,
    ConfirmDialogComponent,
    RetirementListComponent,
    BonaFideListComponent,
    BonaFideComponent,
    RetirementComponent,
    ChapterListComponent,
    ChapterComponent,
    ConfirmDialogComponent,
    QualifyingEventComponent,
    QualifyingEventListComponent,
    ClientListComponent,
    ClientComponent,
    ClientWizardComponent,
    AddressComponent,
    GeneralInformationComponent,
    RetirementComponent,
    DocumentationCallComponent,
    DocsCallsListComponent,
    DialogSuccessComponent,
    DependantsListComponent,
    BonafidesAssociatorComponent,
    DependantComponent,
    PreviewSsn,
    AllianceComponent,
    AllianceListComponent,
    AllianceWizardComponent,
    BeneficiariesBenefitDistributionComponent,
    InsuranceCompanyComponent,
    InsuranceCompanyItemComponent,
    AddOnsDialog,
    BenefitTypeDialog,
    InsurancePlanItemComponent,
    MultiAssistComponent,
    MultiAssistListComponent,
    AllianceInscriptionSheetComponent,
    VehicleListComponent,
    AllianceInscriptionSheetFirtsMedicalComponent,
    DatepickerComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
    }),
    AppRoutingModule,
    BrowserAnimationsModule,
    MatSliderModule,
    MatToolbarModule,
    MatButtonModule,
    MatFormFieldModule,
    MatChipsModule,
    MatCardModule,
    MatButtonToggleModule,
    MatInputModule,
    MatDividerModule,
    MatListModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatIconModule,
    MatGridListModule,
    MatSnackBarModule,
    MatSidenavModule,
    MatTableModule,
    MatCheckboxModule,
    MatExpansionModule,
    MatSortModule,
    MatSlideToggleModule,
    MatSelectModule,
    MatPaginatorModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatRadioModule,
    MatAutocompleteModule,
    MatMenuModule,
    MatTabsModule,
    MatBadgeModule,
    MatTooltipModule,
    MatProgressBarModule,
    MomentModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    FontAwesomeModule,
    FlexLayoutModule,
    BrowserModule,
    HttpClientModule,
    MatStepperModule,
    // CustomFormsModule,
    NgxMaskModule.forRoot(),
  ],
  providers: [
    {
      provide: MatPaginatorIntl,
      deps: [TranslateService],
      useFactory: (translateService: TranslateService) =>
        new PaginatorI18n(translateService).getPaginatorIntl(),
    },
    //LoaderService,
    //{ provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true },
    DatePipe,
    CurrencyPipe,
    PreviewSsn,
    httpInterceptorProviders,
  ],

  bootstrap: [AppComponent],
  entryComponents: [
    DialogGenericErrorComponent,
    SnackBarConfirmationComponent,
    DialogGenericSuccessComponent,
    ConfirmDialogComponent,
    ClientWizardComponent,
    BonafidesAssociatorComponent,
    AllianceWizardComponent,
    AllianceInscriptionSheetComponent,
    AddOnsDialog,
    BenefitTypeDialog,
    AllianceInscriptionSheetFirtsMedicalComponent,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {}
