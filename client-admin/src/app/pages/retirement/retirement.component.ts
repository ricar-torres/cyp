import { RetirementAPIService } from './../../shared/retirement.api.service';
import {
  Component,
  OnInit,
  AfterViewInit,
  ViewChild,
  ElementRef,
} from '@angular/core';
import { LanguageService } from '@app/shared/Language.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CampaignApiSerivce } from '@app/shared/campaign.api.service';
import { AppService } from '@app/shared/app.service';
import { Location } from '@angular/common';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { merge, fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';

@Component({
  selector: 'app-retirement',
  templateUrl: './retirement.component.html',
  styleUrls: ['./retirement.component.css'],
})
export class RetirementComponent implements OnInit {
  @ViewChild('inputName', { static: true })
  inputName: ElementRef;
  @ViewChild('inputCode', { static: true })
  inputCode: ElementRef;

  loading: boolean;
  retirement: any;
  id: string;
  editAccess: boolean;
  createAccess: boolean;
  reactiveForm: FormGroup;
  nameExists: boolean = false;
  codeExists: boolean = false;
  originalName: string;
  originalCode: string;

  constructor(
    public languageService: LanguageService,
    private router: Router,
    private route: ActivatedRoute,
    private apiRetirement: RetirementAPIService,
    private app: AppService
  ) {}

  async ngOnInit() {
    try {
      this.loading = true;
      this.editAccess = true;
      this.createAccess = true;
      this.initForm();
      this.id = this.route.snapshot.paramMap.get('id');

      if (this.id != '0') {
        this.retirement = await this.apiRetirement.getById(this.id);
        if (this.retirement) {
          this.originalName = this.retirement.name;
          this.originalCode = this.retirement.code;
          this.reactiveForm.get('name').setValue(this.retirement.name);
          this.reactiveForm.get('code').setValue(this.retirement.code);
        } else {
          this.onBack();
        }
      } else {
        this.originalName = '';
        this.originalCode = '';
      }

      this.subscribeEvents();
    } catch (error) {
      this.loading = false;
    } finally {
      this.loading = false;
    }
  }

  initForm() {
    const regexString = new RegExp(
      `^[A-Za-z0-9\u00C0-\u00FF]{1}[A-Za-z0-9\u00C0-\u00FF/_.\-\\s\]*$`
    );
    this.reactiveForm = new FormGroup({
      name: new FormControl('', [
        Validators.required,
        Validators.maxLength(250),
        Validators.pattern(regexString),
      ]),
      code: new FormControl('', [
        Validators.required,
        Validators.maxLength(250),
        Validators.pattern(regexString),
      ]),
    });
  }
  onBack() {
    this.router.navigate(['home/retirement-list']);
  }
  async onSubmit() {
    try {
      this.loading = true;
      if (this.id == '0') {
        await this.apiRetirement.create(this.reactiveForm.value);
        this.onBack();
      } else {
        await this.apiRetirement.update(this.id, this.reactiveForm.value);
        this.onBack();
      }
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
  subscribeEvents() {
    merge(fromEvent(this.inputName.nativeElement, 'keydown'))
      .pipe(
        debounceTime(150),
        distinctUntilChanged(),

        tap(async () => {
          let value = this.inputName.nativeElement.value;
          if (value != this.originalName) {
            this.checkNameExist(value);
          } else {
            this.nameExists = false;
          }
        })
      )
      .subscribe();
    merge(fromEvent(this.inputCode.nativeElement, 'keydown'))
      .pipe(
        debounceTime(150),
        distinctUntilChanged(),

        tap(async () => {
          let value = this.inputCode.nativeElement.value;
          if (value != this.originalCode) {
            this.checkCodeExist(value);
          } else {
            this.codeExists = false;
          }
        })
      )
      .subscribe();
    // this.subsName();
    // this.subsCode();
  }

  async checkNameExist(name) {
    const res = await this.apiRetirement.checkNameExist(name);
    this.nameExists = res as boolean;
  }
  async checkCodeExist(code) {
    const res = await this.apiRetirement.checkCodeExist(code);
    this.codeExists = res as boolean;
  }
}
