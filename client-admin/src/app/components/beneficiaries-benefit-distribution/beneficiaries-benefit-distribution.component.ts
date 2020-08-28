import { Component, OnInit, Input } from '@angular/core';
import { trigger, transition, style, animate } from '@angular/animations';
import { FormGroup, FormBuilder } from '@angular/forms';
import { DependantsAPIService } from '@app/shared/dependants.api.service';

@Component({
  selector: 'app-beneficiaries-benefit-distribution',
  templateUrl: './beneficiaries-benefit-distribution.component.html',
  styleUrls: ['./beneficiaries-benefit-distribution.component.css'],
  animations: [
    trigger('fadeAnimation', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('300ms', style({ opacity: 1 })),
      ]),
      transition(':leave', [
        style({ opacity: 1 }),
        animate('300ms', style({ opacity: 0 })),
      ]),
    ]),
  ],
})
export class BeneficiariesBenefitDistributionComponent implements OnInit {
  @Input() percentageDependent: FormGroup[] = [];
  @Input() coverSelection: any;
  typesOfRelation: Object;
  constructor(
    private _formBuilder: FormBuilder,
    private DependantsServices: DependantsAPIService
  ) {}

  ngOnInit(): void {
    this.DependantsServices.getRelationTypes().subscribe((res) => {
      this.typesOfRelation = res;
    });
  }

  addDependant() {
    var newForm = this._formBuilder.group({
      name: [null],
      gender: [null],
      birthDate: [null],
      relation: [null],
      percent: [null],
    });
    newForm.get('birthDate').disable();
    this.percentageDependent.push(newForm);
    this.calculatePercent();
  }

  private calculatePercent() {
    var distr = 100 / this.percentageDependent.length;
    this.percentageDependent.forEach((el) => {
      el.get('percent').setValue(distr);
    });
  }

  deleteDependant(i: number) {
    this.percentageDependent.splice(i, 1);
    this.calculatePercent();
  }

  clearIsuranceDependants(event) {
    if (!event) this.percentageDependent = [];
  }
}
