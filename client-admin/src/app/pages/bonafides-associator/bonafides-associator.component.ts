import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { bonaFideservice } from '@app/shared/bonafide.service';
import {
  FormControl,
  FormGroup,
  FormBuilder,
  Validators,
} from '@angular/forms';
import { ChapterServiceService } from '@app/shared/chapter-service.service';
import { ClientWizardService } from '@app/shared/client-wizard.service';

@Component({
  selector: 'app-bonafides-associator',
  templateUrl: './bonafides-associator.component.html',
  styleUrls: ['./bonafides-associator.component.css'],
})
export class BonafidesAssociatorComponent implements OnInit, OnDestroy {
  availableBonafides: any[];
  correspondingChapters: any[];
  bonafides: FormControl = new FormControl();
  reactiveForm: FormGroup;
  constructor(
    private fb: FormBuilder,
    private bonafideService: bonaFideservice,
    private chapterService: ChapterServiceService,
    public dialogRef: MatDialogRef<BonafidesAssociatorComponent>,
    private clientWizard: ClientWizardService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}
  ngOnDestroy(): void {
    //this.clientWizard.BonafideList = [];
  }

  async ngOnInit() {
    this.reactiveForm = this.fb.group({
      Id: [null],
      ChapterId: [null, [Validators.required]],
      ClientId: [null],
      RegistrationDate: [null],
      NewRegistration: [null],
      Primary: [null],
    });

    this.bonafides.valueChanges.subscribe(async (val) => {
      this.correspondingChapters = await this.chapterService.getChaptersByBonafidesIds(
        val
      );
    });

    if (this.data.fromWizard || this.data.listItem) {
      this.availableBonafides = await this.bonafideService
        .getAll(undefined)
        .toPromise();
    } else {
      this.availableBonafides = await this.bonafideService.getAvailableBonafides(
        this.data.clientId
      );
    }

    if (this.data.listItem) {
      var bonafide = this.clientWizard.BonafideList.find(
        (x) => x.id == this.data.listItem
      );
      this.bonafides.setValue(bonafide.id);
      this.reactiveForm.get('ChapterId').setValue(bonafide.Chapter.ChapterId);
      this.reactiveForm.get('ClientId').setValue(bonafide.Chapter.ClientId);
      this.reactiveForm
        .get('RegistrationDate')
        .setValue(bonafide.Chapter.RegistrationDate);
      this.reactiveForm
        .get('NewRegistration')
        .setValue(bonafide.Chapter.NewRegistration);
      this.reactiveForm.get('Primary').setValue(bonafide.Chapter.Primary);
    }

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
    if (this.data.fromWizard) {
      var bonafidesSelected = this.availableBonafides.find(
        (x) => x.id == this.bonafides.value
      );
      bonafidesSelected['Chapter'] = this.reactiveForm.value;
      //if current bonafides is listed as the primary, all other bonafides
      //must yield this property as false
      this.setPrimary();
      this.clientWizard.BonafideList.push(bonafidesSelected);
      this.dialogRef.close();
    } else if (this.data.listItem) {
      //finding new bonafide selected
      var bonafide = this.availableBonafides.find(
        (x) => x.id == this.bonafides.value
      );
      //finding the index of the bonafides in the list to remove it
      var index = this.clientWizard.BonafideList.findIndex(
        (x) => x.id == this.data.listItem
      );
      // removing the old bonafides information
      this.clientWizard.BonafideList.splice(index, 1);
      //creating the Chapter property in the bonafides
      bonafide['Chapter'] = this.reactiveForm.value;
      //if current bonafides is listed as the primary, all other bonafides
      //must yield this property as false
      this.setPrimary();
      //adding the new bonafides to ths list
      this.clientWizard.BonafideList.push(bonafide);
      this.dialogRef.close();
    } else {
      await this.chapterService
        .saveChapterClient(this.reactiveForm.value)
        .then((rs) => {
          this.dialogRef.close();
        });
    }
  }

  //if current bonafides is listed as the primary, all other bonafides
  //must yield this property as false
  private setPrimary() {
    if (this.reactiveForm.get('Primary').value) {
      for (
        let index = 0;
        index < this.clientWizard.BonafideList.length;
        index++
      ) {
        this.clientWizard.BonafideList[index].Chapter.Primary = false;
      }
    }
  }
}
