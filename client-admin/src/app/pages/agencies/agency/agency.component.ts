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
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private agencyApi: AgencyService,
    private app: AppService
  ) {}

  async ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id) {
      this.reactiveForm = this.fb.group({
        Id: [this.id],
        Name: ['', [Validators.minLength(2), Validators.required]],
      });
      var editAgency: any = await this.agencyApi.agency(this.id);
      this.reactiveForm.get('Name').setValue(editAgency.name);
      this.reactiveForm.get('Id').setValue(editAgency.id);
      console.log(editAgency);
    } else {
      this.reactiveForm = this.fb.group({
        Name: ['', [Validators.minLength(2), Validators.required]],
      });
    }
  }

  onBack() {
    this.router.navigate(['home/agencies']);
  }

  async onSubmit() {
    try {
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
      if (error.status != 401) {
        console.error('error', error);
        this.app.showErrorMessage('Error');
      }
    }
  }
}
