import { Component, ViewChild, HostListener, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { VERSION } from '@angular/material';
import { NavItem } from '../../models/navItem';
import { MatSidenav } from '@angular/material/sidenav';
import { AppService } from '../../shared/app.service';
import { MenuRoles } from '../../models/enums';
import { LanguageService } from '../../shared/Language.service';
import { faGlobeAmericas } from '@fortawesome/free-solid-svg-icons';
import { CustomvalidationServiceService } from '@app/shared/customvalidation-service.service';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.css'],
})
export class SideNavComponent implements OnInit {
  faGlobeAmericas = faGlobeAmericas;

  opened = false;
  @ViewChild('sidenav', { static: true }) sidenav: MatSidenav;

  navItems: NavItem[] = [];
  userDisplayName: string;

  constructor(
    public app: AppService,
    public languageService: LanguageService,
    public validate: CustomvalidationServiceService
  ) {}

  async ngOnInit(): Promise<void> {
    if (this.app.isLoggedIn()) {
      this.userDisplayName = this.app.getLoggedInDisplayName();

      this.navItems = [
        {
          displayName: await this.languageService.translate
            .get('SIDE_NAV.HOME')
            .toPromise(),
          iconName: 'home',
          route: '/home',
          visible: true,
        },
        {
          displayName: await this.languageService.translate
            .get('SIDE_NAV.CLIENTS')
            .toPromise(),
          iconName: 'account_circle',
          route: '/home/clients',
          visible: this.app.checkMenuRoleAccess(MenuRoles.USERS),
        },
        {
          displayName: await this.languageService.translate
            .get('SIDE_NAV.SETTINGS')
            .toPromise(),
          iconName: 'settings_applications',
          visible:
            this.app.checkMenuRoleAccess(MenuRoles.USERS) ||
            this.app.checkMenuRoleAccess(MenuRoles.CAMPAIGNS) ||
            this.app.checkMenuRoleAccess(MenuRoles.AGENCIES),
          children: [
            {
              displayName: await this.languageService.translate
                .get('SIDE_NAV.USERS')
                .toPromise(),
              iconName: 'person',
              route: '/home/user-list',
              visible: this.app.checkMenuRoleAccess(MenuRoles.USERS),
            },
            {
              displayName: await this.languageService.translate
                .get('SIDE_NAV.INSURANCE_COMPANY')
                .toPromise(),
              iconName: 'business',
              route: '/home/insurance-company',
              visible: true,//this.app.checkMenuRoleAccess(MenuRoles.USERS),
            },
            {
              displayName: await this.languageService.translate
                .get('SIDE_NAV.AGENCIES')
                .toPromise(),
              iconName: 'account_balance',
              route: '/home/agencies',
              visible: this.app.checkMenuRoleAccess(MenuRoles.USERS),
            },
            {
              displayName: await this.languageService.translate
                .get('SIDE_NAV.BONAFIDES')
                .toPromise(),
              iconName: 'group_work',
              route: '/home/bonafides',
              visible: this.app.checkMenuRoleAccess(MenuRoles.USERS),
            },

            {
              displayName: await this.languageService.translate
                .get('SIDE_NAV.CAMPAIGNS')
                .toPromise(),
              iconName: 'online_prediction',
              route: '/home/campaigns',
              visible: true, //TODO:this.app.checkMenuRoleAccess(MenuRoles.USERS),
            },
            {
              displayName: await this.languageService.translate
                .get('SIDE_NAV.COMMUNICATION_METHODS')
                .toPromise(),
              iconName: 'settings_input_antenna',
              route: '/home/communication-method-list',
              visible: true, //TODO: this.app.checkMenuRoleAccess(MenuRoles.USERS),
            },
            {
              displayName: await this.languageService.translate
                .get('SIDE_NAV.QUALIFYIN_EVENTS')
                .toPromise(),
              iconName: 'check_circle_outline',
              route: '/home/qualifyingevents',
              visible: this.app.checkMenuRoleAccess(MenuRoles.USERS),
            },
            {
              displayName: await this.languageService.translate
                .get('SIDE_NAV.RETIREMENTS')
                .toPromise(),
              iconName: 'rowing',
              route: '/home/retirement-list',
              visible: true, //TODO: this.app.checkMenuRoleAccess(MenuRoles.USERS),
            },
          ],
        },
      ];

      if (window.innerWidth < 768) {
        this.sidenav.fixedTopGap = 55;
        this.opened = false;
      } else {
        this.sidenav.fixedTopGap = 55;
        this.opened = true;
      }
    } else {
      this.opened = false;
    }
  }

  @HostListener('window:resize', ['$event'])
  onResize(event) {
    if (event.target.innerWidth < 768) {
      this.sidenav.fixedTopGap = 55;
      this.opened = false;
    } else {
      this.sidenav.fixedTopGap = 55;
      this.opened = true;
    }
  }

  isBiggerScreen() {
    const width =
      window.innerWidth ||
      document.documentElement.clientWidth ||
      document.body.clientWidth;
    if (width < 768) {
      return true;
    } else {
      return false;
    }
  }
}
