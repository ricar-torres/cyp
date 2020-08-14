import { Component, OnInit, Input, Inject } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '@app/shared/app.service';
import { bonaFideservice } from '@app/shared/bonafide.service';
import { LanguageService } from '@app/shared/Language.service';
import { ClientWizardService } from '@app/shared/client-wizard.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
@Component({
  selector: 'app-bona-fide',
  templateUrl: './bona-fide.component.html',
  styleUrls: ['./bona-fide.component.css'],
})
export class BonaFideComponent implements OnInit {
  reactiveForm: FormGroup;
  @Input() bonafideId: string;
  loading = false;
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private bonafideService: bonaFideservice,
    private app: AppService,
    private languageService: LanguageService,
    private clientWizard: ClientWizardService,
    private formBuilder: FormBuilder
  ) {}

  async ngOnInit() {
    try {
      this.loading = true;
      this.bonafideId = this.route.snapshot.paramMap.get('id');
      this.reactiveForm = this.formBuilder.group({
        Id: [null],
        Name: [null, [Validators.required], this.bonafideCheckName.bind(this)],
        Code: [null, [Validators.maxLength(255)]],
        Siglas: [null, [Validators.maxLength(255)]],
        Phone: [null, [Validators.maxLength(255)]],
        Email: [
          null,
          [Validators.email, Validators.maxLength(255)],
          this.bonafideCheckEmail.bind(this),
        ],
        Benefits: [null, [Validators.maxLength(255)]],
        Disclaimer: [null, [Validators.maxLength(255)]],
      });
      if (this.bonafideId) {
        var bonafide: any = await this.bonafideService.bonafide(
          this.bonafideId
        );
        this.reactiveForm.get('Id').setValue(bonafide.id);
        this.reactiveForm.get('Name').setValue(bonafide.name);
        this.reactiveForm.get('Code').setValue(bonafide.code);
        this.reactiveForm.get('Siglas').setValue(bonafide.siglas);
        this.reactiveForm.get('Phone').setValue(bonafide.phone);
        this.reactiveForm.get('Email').setValue(bonafide.email);
        this.reactiveForm.get('Benefits').setValue(bonafide.benefits);
        this.reactiveForm.get('Disclaimer').setValue(bonafide.disclaimer);
        this.reactiveForm.get('Email').setAsyncValidators(null);
        this.reactiveForm.get('Name').setAsyncValidators(null);
      } else this.loading = false;
    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
        console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    } finally {
      this.loading = false;
    }
  }

  onBack() {
    this.router.navigate(['home/bonafides']);
  }

  async onSubmit() {
    try {
      this.loading = true;
      if (this.bonafideId) {
        console.log(this.reactiveForm.value);
        await this.bonafideService.update(this.reactiveForm.value);
        this.onBack();
      } else {
        console.log(this.reactiveForm.value);
        await this.bonafideService.create(this.reactiveForm.value);
        this.onBack();
      }
    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
        console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    } finally {
      this.loading = false;
    }
  }

  async bonafideCheckName(name: FormControl) {
    try {
      if (name.value) {
        const res: any = await this.bonafideService.checkName({
          name: name.value,
        });
        if (res) return { nameTaken: true };
      }
    } catch (error) {
      if (error.status != 401) {
        console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    }
  }

  async bonafideCheckEmail(email: FormControl) {
    try {
      if (email.value) {
        const res: any = await this.bonafideService.checkEmail({
          name: email.value,
        });
        if (res) return { emailTaken: true };
      }
    } catch (error) {
      if (error.status != 401) {
        console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    }
  }
}
