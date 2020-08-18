import { HealthPlanService } from './../../shared/health-plan.service';
import { CoverService } from './../../shared/cover.service';
import { DependantsAPIService } from './../../shared/dependants.api.service';
import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CommunicationMethodsAPIService } from '@app/shared/communication-methods.api.service';
import { AppService } from '@app/shared/app.service';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DocumentationCallComponent } from '@app/components/documentation-call/documentation-call.component';

@Component({
  selector: 'app-dependant',
  templateUrl: './dependant.component.html',
  styleUrls: ['./dependant.component.css'],
})
export class DependantComponent implements OnInit {
  loading: boolean;
  dependantId: string | number;
  dependant: any;
  reactiveForm: FormGroup;
  genreOptions = ['DEPENDANT.MALE', 'DEPENDANT.FEMALE'];
  covers: any = [];
  healthPlans: any = [];
  relations: any = [];

  constructor(
    public formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private apiDependant: DependantsAPIService,
    private apiCovers: CoverService,
    private apiHealthPlan: HealthPlanService,
    private app: AppService,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<DependantComponent>,
    @Inject(MAT_DIALOG_DATA) data
  ) {
    this.dependantId = data;
  }

  async ngOnInit() {
    this.loading = true;
    this.initForm();

    try {
      this.covers = await this.apiCovers.GetAll();
      this.healthPlans = await this.apiHealthPlan.GetAll();
      this.relations = await this.apiDependant.getRelationTypes();
      console.log(this.relations);
      // this.dependantId = this.route.snapshot.paramMap.get('id');
      // if (this.dependantId) {
      //   //LOAD DEPENDANT TO FORM FOR EDIT
      //   this.dependant = this.apiDependant.getById(this.dependantId);
      // }
    } catch (error) {
    } finally {
      this.loading = false;
    }
  }
  initForm() {
    this.reactiveForm = this.formBuilder.group({
      name: this.formBuilder.control('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(250),
        Validators.pattern(
          new RegExp(`^[A-Z\u00C0-\u00FF]{1}[A-Za-z\u00C0-\u00FF]*$`)
        ),
      ]),
      initial: this.formBuilder.control('', [
        Validators.maxLength(1),
        Validators.pattern(
          new RegExp(`^[A-Z\u00C0-\u00FF]{1}[A-Za-z\u00C0-\u00FF]*$`)
        ),
      ]),
      lastname1: this.formBuilder.control('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(250),
        Validators.pattern(
          new RegExp(`^[A-Z\u00C0-\u00FF]{1}[A-Za-z\u00C0-\u00FF]*$`)
        ),
      ]),
      lastname2: this.formBuilder.control('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(250),
        Validators.pattern(
          new RegExp(`^[A-Z\u00C0-\u00FF]{1}[A-Za-z\u00C0-\u00FF]*$`)
        ),
      ]),
      birthDate: this.formBuilder.control('', [Validators.required]),
      ssn: this.formBuilder.control('', [
        Validators.required,
        Validators.minLength(11),
        Validators.maxLength(11),
        // Validators.pattern(new RegExp(`^[0-9]{3} [0-9]{2} [0-9]{4}*$`)),
      ]),
      email: this.formBuilder.control('', [Validators.maxLength(250)]),
      gender: this.formBuilder.control('', [Validators.required]),
      phone1: this.formBuilder.control('', [Validators.required]),
      phone2: this.formBuilder.control(''),
      relationshipTypes: this.formBuilder.control('', [Validators.required]),
      plan: this.formBuilder.control(''),
      cover: this.formBuilder.control('', [Validators.required]),
      contractNumber: this.formBuilder.control(''),
      effectiveDate: this.formBuilder.control(''),
    });
  }
  async onSubmit() {
    try {
      this.loading = true;
      console.log(this.reactiveForm.controls['gender'].value.index);
      this.reactiveForm.removeControl('gender');
      var payload = this.reactiveForm.value;
      console.log(payload);
      const res: any = await this.apiDependant.create(payload);
    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
        console.error('error', error);
        this.app.showErrorMessage('Error');
      }
    } finally {
      this.loading = false;
    }
  }
  onBack() {
    // this.router.navigate(['home/communication-method-list']);
  }
}
