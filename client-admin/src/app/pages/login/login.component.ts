import {
  Component,
  OnInit,
  Directive,
  ElementRef,
  ViewChild,
} from '@angular/core';
import { FormGroup, FormControl, NgForm, Validators } from '@angular/forms';
import { ApiService } from '../../shared/api.service';
import { AppService } from '../../shared/app.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router, ActivatedRoute } from '@angular/router';
import { faLock } from '@fortawesome/free-solid-svg-icons';
import { LanguageService } from '@app/shared/Language.service';
import { environment } from '@environments/environment';
import { User } from '@app/models/User';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  @ViewChild('inputUserName') inputUserName: ElementRef;

  faLock = faLock;

  reactiveForm: FormGroup;
  loading = false;

  application: any = {};
  env = environment;

  constructor(
    private api: ApiService,
    private app: AppService,
    private router: Router,
    private route: ActivatedRoute,
    public languageService: LanguageService
  ) {
    this.app.cleanSession();
    this.init();
  }

  async ngOnInit() {
    const key = this.route.snapshot.paramMap.get('app');
    this.application = await this.applicationBykey(key);
    if (this.application) {
      this.init();
      this.inputUserName.nativeElement.focus();
    } else {
      this.router.navigate(['/access-denied']);
    }
  }

  async onSubmit() {
    try {
      this.loading = true;

      const res: User = await this.api.login(this.reactiveForm.value);
      this.app.setLoggedInUser(res);
      this.app.setApplicationKey(this.application.key);
      this.loading = false;
      const redirect = this.app.redirectUrl ? this.app.redirectUrl : '/home';
      this.router.navigate([redirect]);
    } catch (error) {
      this.loading = false;
      if (error.status === 403) {
        this.app.showErrorMessage(
          await this.languageService.translate
            .get('LOGIN-FORM.INVALID_CREDENTIALS')
            .toPromise()
        );
      } else if (error.status === 404) {
        this.app.showErrorMessage(
          await this.languageService.translate
            .get('LOGIN-FORM.INVALID_USERNAME')
            .toPromise()
        );
      } else {
        this.app.showErrorMessage(
          await this.languageService.translate.get('GENERIC_ERROR').toPromise()
        );
      }
    }
  }

  init() {
    this.reactiveForm = new FormGroup({
      applicationKey: new FormControl(
        this.application ? this.application.key : null,
        [Validators.required]
      ),
      userName: new FormControl(null, [
        Validators.required,
        Validators.minLength(3),
      ]),
      password: new FormControl(null, [
        Validators.required,
        Validators.minLength(1),
      ]),
      userType: new FormControl('ADMIN', [Validators.required]),
    });
  }

  async applicationBykey(key: string) {
    try {
      const res: any = await this.api.applicationByKey(key);
      return res;
    } catch (error) {
      return null;
    }
  }
}
