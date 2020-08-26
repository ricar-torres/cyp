import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { QualifyingEventService } from '@app/shared/qualifying-event.service';

@Component({
  selector: 'app-alliance-wizard',
  templateUrl: './alliance-wizard.component.html',
  styleUrls: ['./alliance-wizard.component.css'],
})
export class AllianceWizardComponent implements OnInit {
  affiliationMethod: FormGroup;
  secondFormGroup: FormGroup;

  qualifyingEvents: [] = [];
  constructor(
    private _formBuilder: FormBuilder,
    private qualifyingEventService: QualifyingEventService
  ) {}

  ngOnInit(): void {
    this.qualifyingEventService.getAll().subscribe((res) => {
      this.qualifyingEvents = <any>res;
    });

    this.affiliationMethod = this._formBuilder.group({
      affiliationMethod: [null],
      qualifyingEvent: [null],
    });
    this.secondFormGroup = this._formBuilder.group({
      secondCtrl: [null, [Validators.required]],
    });
  }
}
