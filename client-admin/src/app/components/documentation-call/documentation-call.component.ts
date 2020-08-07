import { DocCall } from './../../models/DocCall';
import { DocumentationCallAPIService } from './../../shared/documentation-call.api.service';
import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AppService } from '@app/shared/app.service';
import { LanguageService } from '@app/shared/Language.service';

@Component({
  selector: 'app-documentation-call',
  templateUrl: './documentation-call.component.html',
  styleUrls: ['./documentation-call.component.css'],
})
export class DocumentationCallComponent implements OnInit {
  loading: boolean;
  retirement: any;
  id: string;
  editAccess: boolean;
  createAccess: boolean;
  reactiveForm: FormGroup;
  typesOfCall: any[] = [];
  clientId: number = 1;

  constructor(
    public languageService: LanguageService,
    private formBuilder: FormBuilder,
    private router: Router,
    private apiDocCall: DocumentationCallAPIService,
    private app: AppService
  ) {}

  async ngOnInit() {
    try {
      this.loading = true;
      this.editAccess = true;
      this.createAccess = true;
      this.initForm();
      await this.loadCallTypes();
      //this.id = this.route.snapshot.paramMap.get('id');

      if (this.id != '0') {
        //this.retirement = await this.apiDocCall.getById(this.id);
        if (this.retirement) {
          this.reactiveForm.get('type').setValue(this.retirement.name);
          this.reactiveForm.get('comment').setValue(this.retirement.code);
        }
      }
    } catch (error) {
      this.loading = false;
    } finally {
      this.loading = false;
    }
  }

  initForm() {
    this.reactiveForm = this.formBuilder.group({
      type: new FormControl('', [Validators.required]),
      comment: new FormControl('', [Validators.required]),
    });
  }

  async loadCallTypes() {
    const res: any = await this.apiDocCall.getCallTypes();
    this.typesOfCall = res.map((type) => {
      return type;
    });
  }
  onBack() {
    this.router.navigate(['home/retirement-list']);
  }
  async onSubmit() {
    try {
      this.loading = true;
      const res = await this.apiDocCall.create(
        new DocCall(
          this.reactiveForm.get('type').value.id,
          this.reactiveForm.get('comment').value,
          this.clientId,
          1 //this.app.getLoggedInUser().Id
        )
      );
      this.onBack();
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
