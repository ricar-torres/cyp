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
  ValidatorFn,
} from '@angular/forms';
import {
  MatSlideToggle,
  DateAdapter,
  MAT_DATE_LOCALE,
  MAT_DATE_FORMATS,
  MatDatepicker,
} from '@angular/material';
import { trigger, transition, style, animate } from '@angular/animations';
import { UniversalValidators } from 'ngx-validators';
import * as _moment from 'moment';
// tslint:disable-next-line:no-duplicate-imports
import {
  MomentDateAdapter,
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
} from '@angular/material-moment-adapter';

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
      year: [null, Validators.required, this.min(1990), this.max(2020)],
    });

    //newForm.get('birthDate').disable();
    this.VehicleList.push(newForm);
  }
  delete(i: number) {
    this.VehicleList.splice(i, 1);
  }
  max(max: number): ValidatorFn {
    return (control: FormControl): { [key: string]: boolean } | null => {
      let val: number = control.value;

      if (control.pristine || control.pristine) {
        return null;
      }
      if (val <= max) {
        return null;
      }
      return { max: true };
    };
  }

  min(min: number): ValidatorFn {
    return (control: FormControl): { [key: string]: boolean } | null => {
      let val: number = control.value;

      if (control.pristine || control.pristine) {
        return null;
      }
      if (val >= min) {
        return null;
      }
      return { min: true };
    };
  }
}
