import { CommunicationMethodsAPIService } from './../../shared/communication-methods.api.service';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormBuilder,
} from '@angular/forms';
import { LanguageService } from '@app/shared/Language.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CampaignApiSerivce } from '@app/shared/campaign.api.service';
import { AppService } from '@app/shared/app.service';
import { merge, fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';

@Component({
  selector: 'app-communication-method',
  templateUrl: './communication-method.component.html',
  styleUrls: ['./communication-method.component.css'],
})
export class CommunicationMethodComponent implements OnInit {
  loading: boolean;
  campaign: any;
  id: string;
  origin = [1, 2];
  editAccess: boolean;
  createAccess: boolean;
  reactiveForm: FormGroup;
  exist: boolean;
  originalName: string;
  @ViewChild('inputName', { static: true })
  inputName: ElementRef;
  constructor(
    public formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private apicommunicationMethod: CommunicationMethodsAPIService,
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
        this.campaign = await this.apicommunicationMethod.getById(this.id);
        if (this.campaign) {
          this.originalName = this.campaign.name;
          this.reactiveForm.get('name').setValue(this.campaign.name);
        } else {
          this.onBack();
        }
      } else {
        this.originalName = '';
      }

      this.subscribeEvents();
    } catch (error) {
      this.loading = false;
    } finally {
      this.loading = false;
    }
  }
  initForm() {
    this.reactiveForm = this.formBuilder.group({
      name: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(250),
        Validators.pattern(
          new RegExp(
            `^[A-Za-z0-9\u00C0-\u00FF]{1}[A-Za-z0-9\u00C0-\u00FF/_.\-\\s\]*$`
          )
        ),
      ]),
    });
  }
  onBack() {
    this.router.navigate(['home/communication-method-list']);
  }
  async onSubmit() {
    try {
      this.loading = true;
      if (this.id == '0') {
        await this.apicommunicationMethod.create(this.reactiveForm.value);
        this.onBack();
      } else {
        await this.apicommunicationMethod.update(
          this.id,
          this.reactiveForm.value
        );
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
            this.exist = false;
          }
        })
      )
      .subscribe();
  }
  async checkNameExist(name) {
    const res = await this.apicommunicationMethod.checkNameExist(name);
    this.exist = res as boolean;
  }
}
