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
    private languageService: LanguageService
  ) {}

  async ngOnInit() {
    this.loading = true;
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id) {
      try {
        var editBonafide: any = await this.bonafideService.bonafide(this.id);
      } catch (error) {
        this.loading = false;
        if (error.status != 401) {
          console.error('error', error);
          this.languageService.translate
            .get('GENERIC_ERROR')
            .subscribe((res) => {
              this.app.showErrorMessage(res);
            });
        }
      } finally {
        this.loading = false;
      }
      this.reactiveForm = this.fb.group({
        Id: [editBonafide.id],
        Name: [editBonafide.name, [Validators.required]],
        Code: [editBonafide.code, [Validators.maxLength(255)]],
        Siglas: [editBonafide.siglas, [Validators.maxLength(255)]],
        Phone: [editBonafide.phone, [Validators.maxLength(255)]],
        Email: [
          editBonafide.email,
          [Validators.email, Validators.maxLength(255)],
        ],
        Benefits: [editBonafide.benefits, [Validators.maxLength(255)]],
        Disclaimer: [editBonafide.disclaimer, [Validators.maxLength(255)]],
      });
    } else {
      this.reactiveForm = this.fb.group({
        Name: ['', [Validators.required], this.checkName.bind(this)],
        Code: ['', [Validators.maxLength(255)]],
        Siglas: ['', [Validators.maxLength(255)]],
        Phone: ['', [Validators.maxLength(255)]],
        Email: [
          '',
          [Validators.email, Validators.maxLength(255)],
          this.checkEmail.bind(this),
        ],
        Benefits: ['', [Validators.maxLength(255)]],
        Disclaimer: ['', [Validators.maxLength(255)]],
      });
    }
    this.loading = false;
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

  async checkName(name: FormControl) {
    try {
      if (name.value) {
        const res: any = await this.bonafideService.checkName({
          name: name.value,
        });
        if (res) return { nameTaken: true };
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

  async checkEmail(email: FormControl) {
    try {
      if (email.value) {
        const res: any = await this.bonafideService.checkEmail({
          name: email.value,
        });
        if (res) return { emailTaken: true };
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
  async validate(ctrl: FormControl) {
    if (ctrl.value) {
    }
  }
}
