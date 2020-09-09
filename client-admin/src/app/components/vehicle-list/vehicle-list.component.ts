import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  ViewChild,
} from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
} from '@angular/forms';
import {
  MatSlideToggle,
  DateAdapter,
  MAT_DATE_LOCALE,
  MAT_DATE_FORMATS,
  MatDatepicker,
} from '@angular/material';
import { trigger, transition, style, animate } from '@angular/animations';

import * as _moment from 'moment';
// tslint:disable-next-line:no-duplicate-imports
import {
  MomentDateAdapter,
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
} from '@angular/material-moment-adapter';
// import { default as _rollupMoment } from 'moment';
// const moment = _rollupMoment || _moment;

// See the Moment.js docs for the meaning of these formats:
// https://momentjs.com/docs/#/displaying/format/
export const MY_FORMATS = {
  parse: {
    dateInput: 'MM/YYYY',
  },
  display: {
    dateInput: 'MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};
@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css'],
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
  providers: [
    // `MomentDateAdapter` can be automatically provided by importing `MomentDateModule` in your
    // application's root module. We provide it at the component level here, due to limitations of
    // our example generation script.
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],
    },

    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ],
})
export class VehicleListComponent implements OnInit {
  @Input() VehicleList: FormGroup[] = [];
  @Output() benefitChecked: EventEmitter<boolean> = new EventEmitter<boolean>();

  @ViewChild('dependantsEnabled', { static: true })
  dependantsEnabled: MatSlideToggle;
  typesOfRelation: Object;
  //date = new FormControl(moment());

  constructor(private _formBuilder: FormBuilder) {}

  ngOnInit(): void {
    if (this.VehicleList.length < 1) {
      this.addVehicle();
    }
  }

  addVehicle() {
    var newForm = this._formBuilder.group({
      model: [null, [Validators.required]],
      vin: [null, [Validators.required]],
      make: [null, [Validators.required]],
      year: [null, [Validators.required, CustomsValidators]],
    });

    //newForm.get('birthDate').disable();
    this.VehicleList.push(newForm);
  }
  delete(i: number) {
    this.VehicleList.splice(i, 1);
  }

  // clearIsuranceDependants(event) {
  //   this.benefitChecked.emit(event);
  //   if (!event) this.VehicleList = [];
  // }
  // chosenYearHandler(normalizedYear: _moment.Moment) {
  //   const ctrlValue = this.date.value;
  //   ctrlValue.year(normalizedYear.year());
  //   this.date.setValue(ctrlValue);
  // }

  // chosenMonthHandler(
  //   normalizedMonth: _moment.Moment,
  //   datepicker: MatDatepicker<_moment.Moment>
  // ) {
  //   const ctrlValue = this.date.value;
  //   ctrlValue.month(normalizedMonth.month());
  //   this.date.setValue(ctrlValue);
  //   datepicker.close();
  // }
}
