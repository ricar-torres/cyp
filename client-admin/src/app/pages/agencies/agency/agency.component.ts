import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AgencyService } from '@app/shared/agency.service';
import { AppService } from '@app/shared/app.service';

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
    private agencyApi: AgencyService,
    private app: AppService
  ) {}

  async ngOnInit() {
    this.loading = true;
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id) {
      this.reactiveForm = this.fb.group({
        id: [this.id],
        name: ['', [Validators.minLength(2), Validators.required]],
      });
      var editAgency: any = await this.agencyApi.agency(this.id);
      this.agency = editAgency.name;
      this.reactiveForm.get('name').setValue(editAgency.name);
      this.reactiveForm.get('id').setValue(editAgency.id);
      console.log(editAgency);
    } else {
      this.reactiveForm = this.fb.group({
        name: ['', [Validators.minLength(2), Validators.required]],
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
        console.log(this.reactiveForm.value, 'update');
        await this.agencyApi.update(this.reactiveForm.value);
        this.onBack();
      } else {
        console.log(this.reactiveForm.value, 'create');
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
}
