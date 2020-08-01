import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '@app/shared/app.service';
import { merge, fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { ChapterServiceService } from '@app/shared/chapter-service.service';

@Component({
  selector: 'app-chapter',
  templateUrl: './chapter.component.html',
  styleUrls: ['./chapter.component.css'],
})
export class ChapterComponent implements OnInit {
  reactiveForm: FormGroup;

  loading = false;

  chapter: string;

  Exists: Boolean = true;

  @ViewChild('inputName', { static: true }) inputName: ElementRef;
  chapterid: string;
  bonafideid: string;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private chapterService: ChapterServiceService,
    private app: AppService
  ) {}

  async ngOnInit() {
    this.loading = true;
    this.chapterid = this.route.snapshot.paramMap.get('chapterid');
    this.bonafideid = this.route.snapshot.paramMap.get('bonafideid');
    console.log(this.chapterid, this.bonafideid);
    if (this.chapterid) {
      this.reactiveForm = this.fb.group({
        id: [this.chapterid],
        name: ['', [Validators.minLength(2), Validators.required]],
        BonaFideId: [this.bonafideid],
      });
      var editChapter: any = await this.chapterService.chapter(this.chapterid);
      this.chapter = editChapter.name;
      this.reactiveForm.get('name').setValue(editChapter.name);
      this.reactiveForm.get('id').setValue(editChapter.id);
    } else {
      this.reactiveForm = this.fb.group({
        name: ['', [Validators.minLength(2), Validators.required]],
        BonaFideId: [this.bonafideid],
      });
    }
    this.subscribeEvents();
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
        this.app.showErrorMessage('Error');
      }
    } finally {
      this.loading = false;
    }
  }

  subscribeEvents() {
    merge(fromEvent(this.inputName.nativeElement, 'keydown'))
      .pipe(
        debounceTime(150),
        distinctUntilChanged(),
        tap(async () => {
          await !this.checkName(this.inputName.nativeElement.value);
        })
      )
      .subscribe();
  }

  async checkName(name: string) {
    try {
      if (name) {
        const res: any = await this.chapterService.checkName({
          name: name,
        });
        console.log(res);
        this.Exists = res;
      }
    } catch (error) {
      this.Exists = false;
    }
  }
}
