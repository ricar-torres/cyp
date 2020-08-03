import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '@app/shared/app.service';
import { LanguageService } from '@app/shared/Language.service';
import { QualifyingEventService } from '@app/shared/qualifying-event.service';

@Component({
  selector: 'app-qualifying-event',
  templateUrl: './qualifying-event.component.html',
  styleUrls: ['./qualifying-event.component.css'],
})
export class QualifyingEventComponent implements OnInit {
  reactiveForm: FormGroup;
  id: string;
  loading = false;

  qualifyingEvent: string;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private qualifyingEventService: QualifyingEventService,
    private app: AppService,
    private languageService: LanguageService
  ) {}

  async ngOnInit() {
    this.loading = true;
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id) {
      var editQualifyingEvent: any = await this.qualifyingEventService.qualifyingevent(
        this.id
      );
      this.qualifyingEvent = editQualifyingEvent.name;
      this.reactiveForm = this.fb.group({
        Id: [editQualifyingEvent.id],
        Name: [
          editQualifyingEvent.name,
          [
            Validators.minLength(2),
            Validators.required,
            Validators.maxLength(255),
          ],
        ],
        Requirements: [editQualifyingEvent.requirements, [Validators.required]],
      });
    } else {
      this.reactiveForm = this.fb.group({
        Name: [
          '',
          [
            Validators.minLength(2),
            Validators.required,
            Validators.maxLength(255),
          ],
          this.checkName.bind(this),
        ],
        Requirements: ['', [Validators.required]],
      });
    }
    this.loading = false;
  }

  onBack() {
    this.router.navigate(['home/qualifyingevents']);
  }

  async onSubmit() {
    try {
      this.loading = true;
      if (this.id) {
        await this.qualifyingEventService.update(this.reactiveForm.value);
        this.onBack();
      } else {
        await this.qualifyingEventService.create(this.reactiveForm.value);
        this.onBack();
      }
    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
        console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    } finally {
      this.loading = false;
    }
  }

  async checkName(name: FormControl) {
    if (name.value) {
      const res: any = await this.qualifyingEventService.checkName({
        name: name.value,
      });
      if (res) return { nameTaken: true };
    }
  }
}
