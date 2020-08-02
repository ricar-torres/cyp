import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
} from '@angular/forms';
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

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private agencyService: AgencyService,
    private app: AppService
  ) {}

  async ngOnInit() {
    this.loading = true;
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id) {
      var editAgency: any = await this.agencyService.agency(this.id);
      this.agency = editAgency.name;
      this.reactiveForm = this.fb.group({
        Id: [editAgency.id],
        Name: [
          editAgency.name,
          [
            Validators.minLength(2),
            Validators.required,
            Validators.maxLength(255),
          ],
        ],
      });
    } else {
      this.reactiveForm = this.fb.group({
        Name: [
          '',
          [
            Validators.minLength(2),
            Validators.required,
            Validators.maxLength(255),
          ],
          this.checkName.bind(this),
        ],
      });
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
        await this.agencyService.update(this.reactiveForm.value);
        this.onBack();
      } else {
        await this.agencyService.create(this.reactiveForm.value);
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

  async checkName(name: FormControl) {
    if (name.value) {
      const res: any = await this.agencyService.checkName({
        name: name.value,
      });
      if (res) return { nameTaken: true };
    }
  }
}
