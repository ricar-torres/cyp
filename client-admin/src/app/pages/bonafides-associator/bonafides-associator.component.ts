import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { bonaFideservice } from '@app/shared/bonafide.service';
import {
  FormControl,
  FormGroup,
  FormBuilder,
  Validators,
} from '@angular/forms';
import { ChapterServiceService } from '@app/shared/chapter-service.service';

@Component({
  selector: 'app-bonafides-associator',
  templateUrl: './bonafides-associator.component.html',
  styleUrls: ['./bonafides-associator.component.css'],
})
export class BonafidesAssociatorComponent implements OnInit {
  availableBonafides: [];
  correspondingChapters: [];
  bonafides: FormControl = new FormControl();
  reactiveForm: FormGroup;
  constructor(
    private fb: FormBuilder,
    private bonafideService: bonaFideservice,
    private chapterService: ChapterServiceService,
    public dialogRef: MatDialogRef<BonafidesAssociatorComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  async ngOnInit() {
    this.reactiveForm = this.fb.group({
      Id: [null],
      ChapterId: [null, [Validators.required]],
      ClientId: [null],
      RegistrationDate: [null],
      NewRegistration: [null],
      Primary: [null],
    });
    this.availableBonafides = await this.bonafideService.getAvailableBonafides(
      this.data.clientId
    );
    this.bonafides.valueChanges.subscribe(async (val) => {
      this.correspondingChapters = await this.chapterService.getChaptersByBonafidesIds(
        val
      );
    });

    if (this.data.bonafideId) {
      this.bonafides.setValue(this.data.bonafideId);
      var chapter: any = await this.chapterService.getChapterOfClientByBonafideId(
        this.data
      );
      this.reactiveForm.get('Id').setValue(chapter.id);
      this.reactiveForm.get('ChapterId').setValue(chapter.chapterId);
      this.reactiveForm.get('ClientId').setValue(chapter.clientId);
      this.reactiveForm
        .get('RegistrationDate')
        .setValue(chapter.registrationDate);
      this.reactiveForm
        .get('NewRegistration')
        .setValue(chapter.newRegistration);
      this.reactiveForm.get('Primary').setValue(chapter.primary);
    } else {
      this.reactiveForm.get('ClientId').setValue(this.data.clientId);
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  async saveChapterClient() {
    await this.chapterService
      .saveChapterClient(this.reactiveForm.value)
      .then(() => {
        this.dialogRef.close();
      });
  }
}
