import {
  Component,
  OnInit,
  Input,
  ViewChild,
  Output,
  EventEmitter,
} from '@angular/core';
import { trigger, transition, style, animate } from '@angular/animations';
import { FormGroup, FormBuilder } from '@angular/forms';
import { DependantsAPIService } from '@app/shared/dependants.api.service';
import { MatSlideToggle } from '@angular/material';

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
  @Input() BeneficiariesList: FormGroup[] = [];
  @Output() benefitChecked: EventEmitter<boolean> = new EventEmitter<boolean>();

  @ViewChild('dependantsEnabled') dependantsEnabled: MatSlideToggle;
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
    console.log(this.BeneficiariesList);
    var newForm = this._formBuilder.group({
      name: [null],
      gender: [null],
      birthDate: [null],
      relationship: [null],
      percent: [null],
    });

    newForm.get('birthDate').disable();
    this.BeneficiariesList.push(newForm);
    this.calculatePercent();
  }

  private calculatePercent() {
    var distr = 100 / this.BeneficiariesList.length;
    this.BeneficiariesList.forEach((el) => {
      el.get('percent').setValue(distr);
    });
  }

  deleteDependant(i: number) {
    this.BeneficiariesList.splice(i, 1);
    this.calculatePercent();
  }

  clearIsuranceDependants(event) {
    this.benefitChecked.emit(event);
    if (!event) this.BeneficiariesList = [];
  }

  get currentPercentage() {
    var percentage: number = 0;
    this.BeneficiariesList.forEach((x) => {
      percentage += Number.parseFloat(x.get('percent').value);
    });
    return percentage;
  }
}
