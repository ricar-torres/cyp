import {
  Component,
  OnInit,
  Inject,
  ViewChild,
  ElementRef,
} from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import {
  MatSnackBar,
  MatChipInputEvent,
  MatAutocompleteSelectedEvent,
  MatAutocomplete,
} from '@angular/material';
import { ApiService } from '../../shared/api.service';
import { AppService } from '../../shared/app.service';
import { Task } from '../../models/task';
import { LanguageService } from '@app/shared/Language.service';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';

@Component({
  selector: 'app-new-task-dialog',
  templateUrl: './new-task-dialog.component.html',
  styleUrls: ['./new-task-dialog.component.css'],
})
export class NewTaskDialogComponent implements OnInit {
  @ViewChild('typeInput') typeInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete: MatAutocomplete;

  id: string;
  key: string;
  loading: boolean = false;
  faPaperPlane = faPaperPlane;

  separatorKeysCodes: number[] = [ENTER, COMMA];
  typeCtrl = new FormControl();
  filteredTypes: Observable<string[]>;
  types: any[] = [];
  allTypes: any[] = [];

  form: FormGroup;
  templateFile: File;

  durationInSeconds = 30;

  constructor(
    public dialogRef: MatDialogRef<NewTaskDialogComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: any,
    private fb: FormBuilder,
    private _snackBar: MatSnackBar,
    private api: ApiService,
    private app: AppService,
    public languageService: LanguageService
  ) {
    this.setupForm();
  }

  async ngOnInit() {
    try {
      this.loading = true;

      await this.loadTypes();
      // const res: any = await this.api.documentTypes();
      // this.types = res;

      // if (!this.data.file.email) {
      //   this.openSnackBar();
      // }

      this.loading = false;
    } catch (error) {
      this.loading = false;
    }
  }

  setupForm() {
    this.id = this.data.file.id;
    this.key = this.data.file.key;
    this.form = this.fb.group({
      key: this.key,
      details: ['', Validators.required],
      documentTypeList: [null],
      //templateFile: [null],
      eSign: [true, Validators.required],
    });

    this.filteredTypes = this.typeCtrl.valueChanges.pipe(
      startWith(null),
      map((fruit: string | null) =>
        fruit ? this._filter(fruit) : this.allTypes.slice()
      )
    );
  }

  async onSubmit() {
    try {
      if (this.form.valid) {
        this.loading = true;
        await this.api.taskCreate(
          this.form.value,
          this.templateFile,
          this.languageService.getLang()
        );
        this.loading = false;
        this._snackBar.dismiss();
        this.dialogRef.close(true);
      }
    } catch (error) {
      this.loading = false;
      this.app.showErrorMessage('Error');
    }
  }

  onSelectFile(event) {
    this.templateFile = event.target.files[0];
    //this.form.controls.templateFile.setValue(event.target.files[0]);
    // this.templateFile.append(
    //   'file',
    //   event.target.files[0],
    //   event.target.files[0].name.replace(' ', '-')
    // );
  }

  onNoClick(): void {
    this._snackBar.dismiss();
    this.dialogRef.close(false);
  }

  openSnackBar() {
    this._snackBar.openFromComponent(EmailNotfoundSnackComponent, {
      duration: this.durationInSeconds * 1000,
      verticalPosition: 'top',
    });
  }

  async loadTypes() {
    try {
      this.allTypes = [];

      const res: any = await this.api.documentTypes();
      this.allTypes = res
        .filter((x) => !this.types.some((y) => y.id == x.id))
        .map((type) => {
          return type;
        });
    } catch (error) {
      if (error.status != 401) {
        this.app.showErrorMessage('Error interno');
      }
    }
  }

  add(event: MatChipInputEvent): void {
    // const input = event.input;
    // const value = event.value;
    // // Add our fruit
    // if ((value || '').trim()) {
    //   this.types.push(value.trim());
    // }
    // // Reset the input value
    // if (input) {
    //   input.value = '';
    // }
    // this.typeCtrl.setValue(null);
  }

  remove(type: string): void {
    const index = this.types.indexOf(type);

    if (index >= 0) {
      this.types.splice(index, 1);
    }
    this.updateSelections();
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    if (!this.types.some((x) => x.id === event.option.value.id)) {
      this.types.push(event.option.value);
      this.typeInput.nativeElement.value = '';
      this.typeCtrl.setValue(null);
      this.updateSelections();
    }
  }

  private updateSelections() {
    if (this.types && this.types.length > 0) {
      this.form.controls.documentTypeList.setValue(
        this.types.map((type) => {
          return type.id;
        })
      );
    } else {
      this.form.controls.documentTypeList.setValue(null);
    }
  }

  private _filter(value: any): any[] {
    if (typeof value === 'string' || value instanceof String) {
      const filterValue = value.toLowerCase();
      return this.allTypes.filter(
        (fruit) => fruit.name.toLowerCase().indexOf(filterValue) === 0
      );
    }
  }
}

@Component({
  selector: 'email-not-found-snack',
  template: `<span class="span">
    {{ 'NEW_TASK.EMAIL_NOT_FOUND' | translate }} !!! 📩</span
  >`,
  styles: [
    `
      .span {
        color: hotpink;
      }
    `,
  ],
})
export class EmailNotfoundSnackComponent implements OnInit {
  constructor() {}
  ngOnInit(): void {}
}
