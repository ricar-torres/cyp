import { DependantsAPIService } from './../../shared/dependants.api.service';
import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CommunicationMethodsAPIService } from '@app/shared/communication-methods.api.service';
import { AppService } from '@app/shared/app.service';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DocumentationCallComponent } from '@app/components/documentation-call/documentation-call.component';

@Component({
  selector: 'app-dependant',
  templateUrl: './dependant.component.html',
  styleUrls: ['./dependant.component.css'],
})
export class DependantComponent implements OnInit {
  loading: boolean;
  dependantId: string | number;
  dependant: any;
  reactiveForm: FormGroup;

  constructor(
    public formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private apiDependant: DependantsAPIService,
    private app: AppService,
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<DependantComponent>,
    @Inject(MAT_DIALOG_DATA) data
  ) {
    this.dependantId = data;
  }

  ngOnInit(): void {
    this.loading = true;
    try {
      this.initForm();
      this.dependantId = this.route.snapshot.paramMap.get('id');
      if (this.dependantId) {
        //LOAD DEPENDANT TO FORM FOR EDIT
        this.dependant = this.apiDependant.getById(this.dependantId);
      }
    } catch (error) {
    } finally {
      this.loading = false;
    }
  }
  initForm() {
    this.reactiveForm = this.formBuilder.group({
      name: this.formBuilder.control('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(250),
        Validators.pattern(
          new RegExp(
            `^[A-Za-z0-9\u00C0-\u00FF]{1}[A-Za-z0-9\u00C0-\u00FF/_.\-\\s\]*$`
          )
        ),
      ]),
    });
  }
  async onSubmit() {
    // try {
    //   this.loading = true;
    //   var payload = new DocCall(
    //     0,
    //     this.reactiveForm.get('type').value.id,
    //     null,
    //     null,
    //     this.reactiveForm.get('comment').value,
    //     this.num.nativeElement.value,
    //     this.clientId,
    //     1 //this.app.getLoggedInUser().Id
    //   );
    //   const res: any = await this.apiDocCall.create(payload);
    //   this.dialogRef.close(res.confirmationNumber);
    // } catch (error) {
    //   this.loading = false;
    //   if (error.status != 401) {
    //     console.error('error', error);
    //     this.app.showErrorMessage('Error');
    //   }
    // } finally {
    //   this.loading = false;
    // }
  }
  onBack() {
    // this.router.navigate(['home/communication-method-list']);
  }
}
