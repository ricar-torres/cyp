import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-agency-create-edit',
  templateUrl: './agency-create-edit.component.html',
  styleUrls: ['./agency-create-edit.component.css'],
})
export class AgencyCreateEditComponent implements OnInit {
  reactiveForm: FormGroup;
  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.reactiveForm = this.fb.group({});
  }

  onSubmit() {}
}
