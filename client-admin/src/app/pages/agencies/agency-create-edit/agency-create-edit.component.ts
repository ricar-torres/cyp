import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AgencyService } from '@app/shared/agency.service';

@Component({
  selector: 'app-agency-create-edit',
  templateUrl: './agency-create-edit.component.html',
  styleUrls: ['./agency-create-edit.component.css'],
})
export class AgencyCreateEditComponent implements OnInit {
  reactiveForm: FormGroup;
  id: string;
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private agencyApi: AgencyService
  ) {}

  async ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id) {
      this.reactiveForm = this.fb.group({
        Id: [this.id],
        Name: ['', [Validators.minLength(2), Validators.required]],
      });
      var editAgency: any = await this.agencyApi.Agency(this.id);
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
    this.router.navigate(['home/agencies']).then((e) => {
      if (e) {
      } else {
      }
    });
  }

  async onSubmit() {
    if (this.id) {
      console.log(this.reactiveForm.value, 'update');
      await this.agencyApi.Update(this.reactiveForm.value);
      this.onBack();
    } else {
      console.log(this.reactiveForm.value, 'create');
      await this.agencyApi.Create(this.reactiveForm.value);
      this.onBack();
    }
  }
}
