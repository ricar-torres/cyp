import {
  Component,
  OnInit,
  Inject,
  ViewChild,
  ElementRef,
} from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormBuilder,
} from '@angular/forms';
import { faSave } from '@fortawesome/free-solid-svg-icons';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatAutocompleteSelectedEvent,
  MatChipInputEvent,
  MatAutocomplete,
} from '@angular/material';
import { ApiService } from '@app/shared/api.service';
import { AppService } from '@app/shared/app.service';
import { Observable } from 'rxjs';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { startWith, map } from 'rxjs/operators';

@Component({
  selector: 'app-document-dialog',
  templateUrl: './document-dialog.component.html',
  styleUrls: ['./document-dialog.component.css'],
})
export class DocumentDialogComponent implements OnInit {
  @ViewChild('typeInput') typeInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete: MatAutocomplete;

  id: string;
  loading: boolean = false;
  faSave = faSave;
  form: FormGroup;
  durationInSeconds = 30;

  separatorKeysCodes: number[] = [ENTER, COMMA];
  typeCtrl = new FormControl();
  filteredTypes: Observable<string[]>;
  types: any[] = [];
  allTypes: any[] = [];

  constructor(
    public dialogRef: MatDialogRef<DocumentDialogComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: any,
    private fb: FormBuilder,
    private api: ApiService,
    private app: AppService
  ) {
    this.setupForm();
  }

  async ngOnInit() {
    try {
      this.loading = true;

      this.id = this.data.FileDocument.id;
      const res: any = await this.api.fileDocument(this.id);
      if (res.documentType) this.types.push(res.documentType);
      await this.loadTypes();

      this.setupForm(res);
      this.loading = false;
    } catch (error) {
      this.loading = false;
    }
  }

  setupForm(item?) {
    if (item) {
      this.form = this.fb.group({
        documentTypeId: new FormControl(item.documentTypeId, [
          Validators.required,
        ]),
        delFlag: new FormControl(item.delFlag, [Validators.required]),
      });

      this.filteredTypes = this.typeCtrl.valueChanges.pipe(
        startWith(null),
        map((fruit: string | null) =>
          fruit ? this._filter(fruit) : this.allTypes.slice()
        )
      );
    } else {
      this.form = this.fb.group({
        documentTypeId: new FormControl(null, [Validators.required]),
        delFlag: new FormControl(null, [Validators.required]),
      });
    }
  }

  async onSubmit() {
    try {
      this.loading = true;

      if (this.form.valid) {
        const res: any = await this.api.fileDocumentUpdate(
          this.id,
          this.form.value
        );
        this.dialogRef.close(true);
      }
      this.loading = false;
    } catch (error) {
      this.loading = false;
      this.app.showErrorMessage('Error');
    }
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
      this.types = [];
      this.types.push(event.option.value);
      this.typeInput.nativeElement.value = '';
      this.typeCtrl.setValue(null);
      this.updateSelections();
    }
  }

  private updateSelections() {
    if (this.types && this.types.length > 0) {
      this.form.controls.documentTypeId.setValue(this.types[0].id);
    } else {
      this.form.controls.documentTypeId.setValue(null);
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

  onNoClick(): void {
    this.dialogRef.close(false);
  }
}
