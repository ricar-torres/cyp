import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '@app/shared/app.service';
import { bonaFideservice } from '@app/shared/bonafide.service';
import { merge, fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { LanguageService } from '@app/shared/Language.service';
import { ClientWizardService } from '@app/shared/client-wizard.service';
@Component({
  selector: 'app-bona-fide',
  templateUrl: './bona-fide.component.html',
  styleUrls: ['./bona-fide.component.css'],
})
export class BonaFideComponent implements OnInit {
  reactiveForm: FormGroup;
  id: string;
  loading = false;
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private bonafideService: bonaFideservice,
    private app: AppService,
    private languageService: LanguageService,
    private clientWizard: ClientWizardService
  ) {}

  async ngOnInit() {
    try {
      this.loading = true;
      this.id = this.route.snapshot.paramMap.get('id');
      this.reactiveForm = this.clientWizard.bonafidesFormGroup;
      if (this.id) {
        var bonafide: any = await this.bonafideService.bonafide(this.id);
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
      if (this.id) {
        await this.bonafideService.update(this.reactiveForm.value);
        this.onBack();
      } else {
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
}
