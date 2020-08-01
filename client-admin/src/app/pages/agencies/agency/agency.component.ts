import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AgencyService } from '@app/shared/agency.service';
import { AppService } from '@app/shared/app.service';
import { merge, fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';

@Component({
  selector: 'app-agency',
  templateUrl: './agency.component.html',
  styleUrls: ['./agency.component.css'],
})
export class AgencyComponent implements OnInit {
  reactiveForm: FormGroup;
  id: string;
  loading = false;

  agency: string;

  Exists: Boolean = true;

  @ViewChild('inputName', { static: true }) inputName: ElementRef;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private agencyApi: AgencyService,
    private app: AppService
  ) {}

  async ngOnInit() {
    this.loading = true;
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id) {
      this.reactiveForm = this.fb.group({
        id: [this.id],
        name: [
          '',
          [
            Validators.minLength(2),
            Validators.required,
            Validators.maxLength(255),
          ],
        ],
      });
      var editAgency: any = await this.agencyApi.agency(this.id);
      this.agency = editAgency.name;
      this.reactiveForm.get('name').setValue(editAgency.name);
      this.reactiveForm.get('id').setValue(editAgency.id);
    } else {
      this.reactiveForm = this.fb.group({
        name: [
          '',
          [
            Validators.minLength(2),
            Validators.required,
            Validators.maxLength(255),
          ],
        ],
      });
      this.subscribeEvents();
    }
    this.loading = false;
  }

  onBack() {
    this.router.navigate(['home/agencies']);
  }

  async onSubmit() {
    try {
      this.loading = true;
      if (this.id) {
        await this.agencyApi.update(this.reactiveForm.value);
        this.onBack();
      } else {
        await this.agencyApi.create(this.reactiveForm.value);
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
          await !this.checkName(this.inputName.nativeElement.value);
        })
      )
      .subscribe();
  }

  async checkName(name: string) {
    try {
      if (name) {
        const res: any = await this.agencyApi.checkName({
          name: name,
        });
        console.log(res);
        this.Exists = res;
      }
    } catch (error) {
      this.Exists = false;
    }
  }
}
