import { DialogGenericSuccessComponent } from './../dialog-generic-success/dialog-generic-success.component';
import { DocCall } from './../../models/DocCall';
import { DocumentationCallAPIService } from './../../shared/documentation-call.api.service';
import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  Input,
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
import { Router } from '@angular/router';
import { AppService } from '@app/shared/app.service';
import { LanguageService } from '@app/shared/Language.service';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-documentation-call',
  templateUrl: './documentation-call.component.html',
  styleUrls: ['./documentation-call.component.css'],
})
export class DocumentationCallComponent implements OnInit {
  @ViewChild('num') num: ElementRef;
  loading: boolean;
  retirement: any;
  id: number;
  editAccess: boolean;
  createAccess: boolean;
  reactiveForm: FormGroup;
  typesOfCall: any[] = [];

  @Input()
  clientId: number = 1;
  confirmationNumber: string;
  constructor(
    public languageService: LanguageService,
    private formBuilder: FormBuilder,
    private router: Router,
    private apiDocCall: DocumentationCallAPIService,
    private app: AppService,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<DocumentationCallComponent>,
    @Inject(MAT_DIALOG_DATA) data
  ) {
    this.clientId = data.clientId;
    this.confirmationNumber = data.confirmationNumber;
  }

  async ngOnInit() {
    try {
      this.loading = true;
      this.editAccess = true;
      this.createAccess = true;
      this.initForm();
      await this.loadCallTypes();

      // if (this.id != '0') {
      //   //this.retirement = await this.apiDocCall.getById(this.id);
      //   if (this.retirement) {
      //     this.reactiveForm.get('type').setValue(this.retirement.name);
      //     this.reactiveForm.get('comment').setValue(this.retirement.code);
      //   }
      // }
    } catch (error) {
      this.loading = false;
    } finally {
      this.loading = false;
    }
  }

  initForm() {
    this.reactiveForm = this.formBuilder.group({
      num: this.formBuilder.control({
        value: this.confirmationNumber,
        disabled: true,
      }),
      type: this.formBuilder.control('', [Validators.required]),
      comment: this.formBuilder.control('', [Validators.required]),
    });
  }

  async loadCallTypes() {
    const res: any = await this.apiDocCall.getCallTypes();
    this.typesOfCall = res.map((type) => {
      return type;
    });
  }
  onBack() {
    this.dialogRef.close();
  }
  openDialog(): void {
    const dialogRef = this.dialog.open(DialogGenericSuccessComponent, {
      width: '250px',
      data: {
        message: 'Success',
        html: '<h4>' + this.reactiveForm.get('num').value + '</h4>',
      },
    });

    // dialogRef.afterClosed().subscribe((result) => {
    //   console.log('The dialog was closed');
    // });
  }

  async onSubmit() {
    try {
      this.loading = true;
      var payload = new DocCall(
        0,
        this.reactiveForm.get('type').value.id,
        null,
        null,
        this.reactiveForm.get('comment').value,
        this.num.nativeElement.value,
        this.clientId,
        this.app.getLoggedInUser().id
      );
      const res: any = await this.apiDocCall.create(payload);
      this.dialogRef.close(res.confirmationNumber);
    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
        console.error('error', error);
        this.app.showErrorMessage('Error');
      }
    } finally {
      this.loading = false;
    }
  }
}
