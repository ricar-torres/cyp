import { Component, OnInit, AfterViewInit } from '@angular/core';
import { LanguageService } from '@app/shared/Language.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CampaignApiSerivce } from '@app/shared/campaign.api.service';
import { AppService } from '@app/shared/app.service';
import { Location } from '@angular/common';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-campaign',
  templateUrl: './campaign.component.html',
  styleUrls: ['./campaign.component.css'],
})
export class CampaignComponent implements OnInit {
  readonly _actionTitleCampaign = 'CAMPAIGN_ACTION_TITLE';
  readonly _actionNameCampaign = 'CAMPAIGN_ACTION_NAME';
  loading: boolean;
  campaign?: any;
  id: string;
  actionTitle: string;
  actionName: string;
  campaignName: string;
  origin = [1, 2];
  selectedOption: number;
  editAccess: boolean = true;
  reactiveForm: FormGroup;

  constructor(
    public languageService: LanguageService,
    private router: Router,
    private route: ActivatedRoute,
    private campaignApi: CampaignApiSerivce,
    private app: AppService
  ) {}

  async ngOnInit() {
    try {
      this.loading = true;
      this.editAccess = true;
      this.id = this.route.snapshot.paramMap.get('id');

      if (this.id != '0') {
        this.campaign = await this.campaignApi.getById(this.id);
      }

      this.initActionLabel();
      this.initForm();
    } catch (error) {
      this.loading = false;
    } finally {
      this.loading = false;
    }
  }
  onSubmit() {}

  initForm() {
    this.reactiveForm = new FormGroup({
      campaignName: new FormControl(null, [
        Validators.required,
        Validators.minLength(5),
      ]),
      campaignOrigin: new FormControl(null, [Validators.required]),
    });
    this.reactiveForm.get('campaignName').setValue(this.campaign.name);
    this.reactiveForm.get('campaignOrigin').setValue(this.campaign.origin);
    this.selectedOption = this.campaign.origin;
  }
  onBack() {
    this.router.navigate(['home/campaigns']);
  }

  initActionLabel() {
    this.actionTitle = history.state.actionTitle;
    this.actionName = history.state.actionName;

    if (
      this.actionTitle == null ||
      this.actionTitle == undefined ||
      this.actionTitle === ''
    ) {
      this.actionTitle = localStorage.getItem(this._actionTitleCampaign);
    } else {
      localStorage.setItem(this._actionTitleCampaign, this.actionTitle);
    }

    if (
      this.actionName == null ||
      this.actionName == undefined ||
      this.actionName === ''
    ) {
      this.actionName = localStorage.getItem(this._actionNameCampaign);
    } else {
      localStorage.setItem(this._actionNameCampaign, this.actionName);
    }
  }
}
