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
  AbstractControl,
} from '@angular/forms';
import { MatSlideToggle } from '@angular/material';
import { DependantsAPIService } from '@app/shared/dependants.api.service';
import { AlliancesService } from '@app/shared/alliances.service';
import { trigger, transition, style, animate } from '@angular/animations';

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
      year: [null, [Validators.required]],
    });

    //newForm.get('birthDate').disable();
    this.VehicleList.push(newForm);
  }
  delete(i: number) {
    this.VehicleList.splice(i, 1);
  }

  clearIsuranceDependants(event) {
    this.benefitChecked.emit(event);
    if (!event) this.VehicleList = [];
  }
}
