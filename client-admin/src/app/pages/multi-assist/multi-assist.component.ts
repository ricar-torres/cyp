import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-multi-assist',
  templateUrl: './multi-assist.component.html',
  styleUrls: ['./multi-assist.component.css'],
})
export class MultiAssistComponent implements OnInit {
  multiassist: FormGroup;
  healthPlans: any = [];
  covers: any = [];
  filteredHealthPlans: any;
  filteredCovers: any;
  constructor(private _formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.multiassist = this._formBuilder.group({
      HealthPlan: [null],
      Addititons: [null],
    });
  }
}
