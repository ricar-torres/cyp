import {
  Component,
  OnInit,
  Input,
  ViewChild,
  Output,
  EventEmitter,
} from '@angular/core';
import { trigger, transition, style, animate } from '@angular/animations';
import {
  FormGroup,
  FormBuilder,
  Validators,
  AbstractControl,
} from '@angular/forms';
import { DependantsAPIService } from '@app/shared/dependants.api.service';
import { MatSlideToggle } from '@angular/material';
import { Beneficiaries } from '@app/models/MultiAssist';
import { AlliancesService } from '@app/shared/alliances.service';

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

  @ViewChild('dependantsEnabled', { static: true })
  dependantsEnabled: MatSlideToggle;
  typesOfRelation: Object;
  constructor(
    private _formBuilder: FormBuilder,
    private DependantsServices: DependantsAPIService,
    private allianceService: AlliancesService
  ) {}

  ngOnInit(): void {
    this.DependantsServices.getRelationTypes().subscribe((res) => {
      this.typesOfRelation = res;
    });
  }

  addDependant() {
    var newForm = this._formBuilder.group({
      name: [null, [Validators.required]],
      gender: [null, [Validators.required]],
      birthDate: [null, [Validators.required]],
      ssn: [null, [Validators.required], this.checkSsn('').bind(this)],
      relationship: [null, [Validators.required]],
      percent: [null, [Validators.required]],
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

  checkSsn(ssn: string) {
    return async (control: AbstractControl) => {
      //console.log(ssn, control.value);
      if (control.value && ssn != control.value) {
        const res: any = await this.allianceService
          .checkSsn(control.value)
          .toPromise();
        if (res) return { ssnTaken: true };
      }
      return null;
    };
  }
}
