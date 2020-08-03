import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '@app/shared/app.service';
import { merge, fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { ChapterServiceService } from '@app/shared/chapter-service.service';
import { LanguageService } from '@app/shared/Language.service';

@Component({
  selector: 'app-chapter',
  templateUrl: './chapter.component.html',
  styleUrls: ['./chapter.component.css'],
})
export class ChapterComponent implements OnInit {
  reactiveForm: FormGroup;

  loading = false;

  Exists: Boolean = false;

  @ViewChild('inputName', { static: true }) inputName: ElementRef;
  chapterid: string;
  bonafideid: string;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private chapterService: ChapterServiceService,
    private app: AppService,
    private languageService: LanguageService
  ) {}

  async ngOnInit() {
    this.loading = true;
    this.chapterid = this.route.snapshot.paramMap.get('chapterid');
    this.bonafideid = this.route.snapshot.paramMap.get('bonafideid');
    if (this.chapterid) {
      try {
        var editChapter: any = await this.chapterService.chapter(
          this.chapterid
        );
      } catch (error) {
        this.loading = false;
        if (error.status != 401) {
          console.error('error', error);
          this.languageService.translate
            .get('GENERIC_ERROR')
            .subscribe((res) => {
              this.app.showErrorMessage(res);
            });
        }
      } finally {
        this.loading = false;
      }
      this.reactiveForm = this.fb.group({
        Id: [editChapter.id],
        Name: [
          editChapter.name,
          [Validators.required, Validators.maxLength(255)],
        ],
        Quota: [editChapter.quota],
        BonaFideId: [this.bonafideid],
      });
    } else {
      this.reactiveForm = this.fb.group({
        Name: [
          '',
          [Validators.required, Validators.maxLength(255)],
          this.uniqueName.bind(this),
        ],
        Quota: [''],
        BonaFideId: [this.bonafideid],
      });
    }
    this.loading = false;
  }

  onBack() {
    this.router.navigate(['home/bonafide', this.bonafideid]);
  }

  async onSubmit() {
    try {
      this.loading = true;
      if (this.chapterid) {
        await this.chapterService.update(this.reactiveForm.value);
        this.onBack();
      } else {
        await this.chapterService.create(this.reactiveForm.value);
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

  async uniqueName(ctrl: FormControl) {
    try {
      if (ctrl.value) {
        const res: any = await this.chapterService.checkName({
          name: ctrl.value,
        });
        if (res) {
          return { nameTaken: true };
        }
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
}
