import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import * as moment from 'moment';
import 'moment/locale/es';

@Injectable({
  providedIn: 'root',
})
export class LanguageService {
  browserLang: any;
  english: boolean;
  espanol: boolean;

  constructor(public translate: TranslateService) {}

  setup() {
    this.translate.addLangs(['en', 'es']);
    //const browserLang = this.translate.getBrowserLang();
    this.translate.setDefaultLang('es');
    this.useLang('es');
    //this.useLang(browserLang.match(/en|es/) ? browserLang : 'en');
  }
  useLang(language: string) {
    this.translate.use(language);
    moment.locale(language);

    localStorage.setItem('lang', language);

    if (language === 'es') {
      this.espanol = true;
      this.english = false;
    }
    if (language === 'en') {
      this.english = true;
      this.espanol = false;
    }
  }

  getLang() {
    var lan = localStorage.getItem('lang');
    return lan;
  }
}
