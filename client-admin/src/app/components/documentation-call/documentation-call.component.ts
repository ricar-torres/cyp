import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CommunicationMethodsAPIService } from '@app/shared/communication-methods.api.service';
import { AppService } from '@app/shared/app.service';

@Component({
  selector: 'app-documentation-call',
  templateUrl: './documentation-call.component.html',
  styleUrls: ['./documentation-call.component.css'],
})
export class DocumentationCallComponent implements OnInit {
  loading: boolean;
  id: string;
  reactiveForm: FormGroup;
  constructor(
    public formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private apicommunicationMethod: CommunicationMethodsAPIService,
    private app: AppService
  ) {}

  ngOnInit(): void {}
  async onSubmit() {
    try {
      this.loading = true;
      if (this.id == '0') {
        await this.apicommunicationMethod.create(this.reactiveForm.value);
        this.onBack();
      } else {
        await this.apicommunicationMethod.update(
          this.id,
          this.reactiveForm.value
        );
        this.onBack();
      }
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
  onBack() {
    this.router.navigate(['home/communication-method-list']);
  }
}
