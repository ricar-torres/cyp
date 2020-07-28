import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ApiService } from '../../shared/api.service';
import { AppService } from '../../shared/app.service';
import { throwMatDialogContentAlreadyAttachedError } from '@angular/material';
import { DatePipe, CurrencyPipe } from '@angular/common';
import { MenuRoles } from 'src/app/models/enums';

import { COMMA, ENTER } from '@angular/cdk/keycodes';
import {
  MatAutocompleteSelectedEvent,
  MatAutocomplete,
} from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { CustomvalidationServiceService } from '@app/shared/customvalidation-service.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css'],
})
export class UserComponent implements OnInit {
  id: string;
  checkNo: string;
  reactiveForm: FormGroup;
  loginProvider: any[] = [
    { id: 1, desc: 'Local' },
    { id: 2, desc: 'Active Directory' },
  ];
  loading = false;
  editAccess = false;
  createAccess = false;

  //Chips

  visible = true;
  selectable = true;
  removable = true;
  separatorKeysCodes: number[] = [ENTER, COMMA];
  filteredRoles: Observable<string[]>;
  roles: any[] = [];
  allRoles: any[];

  @ViewChild('roleInput') roleInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete: MatAutocomplete;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private api: ApiService,
    private app: AppService,
    private datePipe: DatePipe,
    private customValidator: CustomvalidationServiceService,
    private currencyPipe: CurrencyPipe
  ) {
    this.setupForm(null);
  }

  async ngOnInit() {
    try {
      this.loading = true;

      this.id = this.route.snapshot.paramMap.get('id');

      if (this.id) {
        const res: any = await this.api.user(this.id);

        if (res) {
          await this.setupForm(res);
        }
      }

      this.loadRoles();

      this.loading = false;
    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
        console.error('error', error);
        this.app.showErrorMessage('Error interno');
      }
    }
  }

  async onSubmit() {
    try {
      if (this.id && this.editAccess && this.reactiveForm.valid) {
        this.loading = true;
        // this.reactiveForm.controls.rolesAlt.setValue(this.roles.map( (r) => {
        //   return r.id
        // }));

        let res: any = await this.api.userUpdate(
          this.id,
          this.reactiveForm.value
        );
        this.loading = false;
        this.onBack();
      } else if (!this.id && this.createAccess && this.reactiveForm.valid) {
        this.loading = true;
        // this.reactiveForm.controls.rolesAlt.setValue(this.roles.map( (r) => {
        //   return r.id
        // }));

        let res: any = await this.api.userCreate(
          this.id,
          this.reactiveForm.value
        );
        this.loading = false;
        this.onBack();
      }
    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
        console.error('error', error);
        this.app.showErrorMessage('Error interno');
      }
    }
  }

  onBack() {
    this.router.navigate(['/home/user-list']);
  }

  async setupForm(object) {
    this.editAccess = this.app.checkMenuRoleAccess(MenuRoles.USERS_UPDATE);
    this.createAccess = this.app.checkMenuRoleAccess(MenuRoles.USERS_CREATE);

    if (object) {
      this.roles = object.roles.map((role) => {
        return role.role;
      });

      this.reactiveForm = new FormGroup({
        userName: new FormControl({ value: object.userName, disabled: false }, [
          Validators.required,
        ]),

        firstName: new FormControl(
          { value: object.firstName, disabled: false },
          [Validators.required]
        ),

        lastName: new FormControl({ value: object.lastName, disabled: false }, [
          Validators.required,
        ]),

        loginProviderId: new FormControl(object.loginProviderId, [
          Validators.required,
        ]),

        // applicationId: new FormControl(object.applicationId, [
        //   Validators.required
        // ]),

        // reference1: new FormControl(object.reference1, [
        //   Validators.required
        // ]),

        delFlag: new FormControl(object.delFlag, [Validators.required]),

        rolesAlt: new FormControl(
          this.roles.map((r) => {
            return r.id;
          })
        ),

        password: new FormControl(null, [
          this.customValidator.patternValidator(),
        ]),
      });
    } else {
      this.reactiveForm = new FormGroup({
        userName: new FormControl({ value: null, disabled: false }, [
          Validators.required,
        ]),

        firstName: new FormControl({ value: null, disabled: false }, [
          Validators.required,
        ]),

        lastName: new FormControl({ value: null, disabled: false }, [
          Validators.required,
        ]),

        loginProviderId: new FormControl(null, [Validators.required]),

        // applicationId: new FormControl(this.app.getLoggedInUserApplication, [
        //   Validators.required
        // ]),

        // reference1: new FormControl(null, [
        //   Validators.required
        // ]),

        delFlag: new FormControl(false, [Validators.required]),

        rolesAlt: new FormControl([]),

        password: new FormControl(null, [
          Validators.required,
          this.customValidator.patternValidator(),
        ]),
      });
    }
  }

  private async loadRoles() {
    try {
      //const currentRoles = this.reactiveForm.controls.roles.value;
      this.allRoles = [];

      const res: any = await this.api.roles();
      this.allRoles = res
        .filter((x) => !this.roles.some((y) => y.id == x.id))
        .map((role) => {
          return role;
        });

      this.filteredRoles = this.reactiveForm.controls.rolesAlt.valueChanges.pipe(
        startWith(null),
        map((role: any | null) =>
          role ? this._filter(role.name) : this.allRoles.slice()
        )
      );
    } catch (error) {
      if (error.status != 401) {
        console.error('error', error);
        this.app.showErrorMessage('Error interno');
      }
    }
  }

  //chips

  add(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    // Add our role
    if ((value || '').trim()) {
      this.roles.push(value.trim());
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }

    this.reactiveForm.controls.rolesAlt.setValue(null);
  }

  remove(role: any): void {
    const index = this.roles.indexOf(role);

    if (index >= 0) {
      this.roles.splice(index, 1);
    }

    this.reactiveForm.controls.rolesAlt.setValue(
      this.roles.map((r) => {
        return r.id;
      })
    );

    this.loadRoles();
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.roles.push(event.option.value);
    this.roleInput.nativeElement.value = '';

    //this.reactiveForm.controls.rolesAlt.setValue(null);

    this.reactiveForm.controls.rolesAlt.setValue(
      this.roles.map((r) => {
        return r.id;
      })
    );

    this.loadRoles();
  }

  private _filter(value: string): any[] {
    const filterValue = value ? value.toLowerCase() : '';
    return this.allRoles
      ? this.allRoles.filter(
          (role) => role.name.toLowerCase().indexOf(filterValue) === 0
        )
      : [];
  }
}
