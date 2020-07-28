/// <reference path="../typings.d.ts" />
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {NavService} from './shared/nav.service';
import { AppService } from './shared/app.service';
import {TranslateService} from '@ngx-translate/core';
import {LanguageService} from './shared/Language.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  
  constructor(private router: Router, 
              public app: AppService,
              public translate: TranslateService,
              public languageService: LanguageService) {

      languageService.setup();
      
   }

  ngOnInit() {

  }

}
