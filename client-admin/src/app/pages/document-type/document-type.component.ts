import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { LanguageService } from '@app/shared/Language.service';
import { AppService } from '@app/shared/app.service';
import { ApiService } from '@app/shared/api.service';
import { Route } from '@angular/compiler/src/core';
import { ActivatedRoute, Router } from '@angular/router';
import { startWith, map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { MenuRoles } from '@app/models/enums';

@Component({
  selector: 'app-document-type',
  templateUrl: './document-type.component.html',
  styleUrls: ['./document-type.component.css'],
})
export class DocumentTypeComponent implements OnInit {
  loading = false;
  id: string;

  reactiveForm: FormGroup;
  editAccess = false;
  createAccess = false;
  filteredRoles: Observable<any>;
  roles: any[] = [];
  allRoles: any[];

  constructor(
    public languageService: LanguageService,
    private app: AppService,
    private api: ApiService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.setUpForm(null);
  }

  async ngOnInit() {
    try {
      this.id = this.route.snapshot.paramMap.get('id');
      this.createAccess = this.app.checkMenuRoleAccess(
        MenuRoles.DOCUMENT_TYPES_CREATE
      );
      this.editAccess = this.app.checkMenuRoleAccess(
        MenuRoles.DOCUMENT_TYPES_UPDATE
      );

      if (this.id) {
        var res = await this.api.documentType(this.id);
        this.setUpForm(res);
      }
    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
        console.error('error', error);
        this.app.showErrorMessage('Error');
      }
    }
  }

  async setUpForm(res) {
    if (res) {
      this.reactiveForm = new FormGroup({
        name: new FormControl({ value: res.name, disabled: false }, [
          Validators.required,
        ]),
        delFlag: new FormControl({ value: res.delFlag, disabled: false }, [
          Validators.required,
        ]),
      });
    } else {
      this.reactiveForm = new FormGroup({
        name: new FormControl({ value: '', disabled: false }, [
          Validators.required,
        ]),
        delFlag: new FormControl({ value: false, disabled: false }, [
          Validators.required,
        ]),
      });
    }
  }

  async onSubmit() {
    try {
      if (this.id && this.reactiveForm.valid && this.editAccess) {
        this.loading = true;
        let res: any = await this.api.updateDocumentType(
          this.id,
          this.reactiveForm.value
        );
        this.loading = false;
        this.onBack();
      } else if (!this.id && this.reactiveForm.valid && this.createAccess) {
        this.loading = true;
        let res: any = await this.api.createDocumentType(
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

  _filter(name: any) {
    throw new Error('Method not implemented.');
  }

  onBack() {
    this.router.navigate(['/home/document-type-list']);
  }
}
