import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormBuilder,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomvalidationServiceService } from '@app/shared/customvalidation-service.service';
import { AppService } from '@app/shared/app.service';
import { ApiService } from '@app/shared/api.service';
import { LanguageService } from '@app/shared/Language.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css'],
})
export class ChangePasswordComponent implements OnInit {
  id: string;
  loading = false;
  reactiveForm: FormGroup;
  user: any;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private customValidator: CustomvalidationServiceService,
    private router: Router,
    private apiservice: ApiService,
    private app: AppService,
    private language: LanguageService
  ) {
    this.user = this.app.getLoggedInUser();
  }

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
    this.setUpForm();
  }

  async onSubmit() {
    if (this.id && this.reactiveForm.valid) {
      this.loading = true;
      try {
        const res: any = await this.apiservice.changePassword(
          this.reactiveForm.value
        );
        this.loading = false;
        this.app.changeUserProperty('isChgPwd', false);
        this.app.redirectUrl = null;
        this.router.navigate(['/home']);
      } catch (ex) {
        this.loading = false;
        if (ex.status == 400)
          this.app.showErrorMessage(
            await this.language.translate.get('GENERIC_ERROR').toPromise()
          );
        else if (ex.status == 403)
          this.app.showErrorMessage(
            await this.language.translate
              .get('CHANGE_PASSWORD.INVALID_CURRENT_PASSWORD')
              .toPromise()
          );
        else if (ex.status == 404)
          this.app.showErrorMessage(
            await this.language.translate
              .get('CHANGE_PASSWORD.INVALID_USER')
              .toPromise()
          );
        else this.app.showErrorMessage(ex.error);
      }
    }
  }

  setUpForm() {
    this.reactiveForm = this.fb.group(
      {
        password: ['', Validators.required],
        newPassword: [
          '',
          Validators.compose([
            Validators.required,
            this.customValidator.patternValidator(),
          ]),
        ],
        confirmNewPassword: ['', [Validators.required]],
      },
      {
        validator: this.customValidator.matchPassword(
          'newPassword',
          'confirmNewPassword'
        ),
      }
    );
    if (this.user.isChgPwd) this.reactiveForm.removeControl('password');
  }
}
