import { Injectable } from '@angular/core';
import {
  CanActivate,
  CanActivateChild,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
  Router,
  ActivatedRoute,
} from '@angular/router';
import { Observable } from 'rxjs';
import { AppService } from '../shared/app.service';
import { User } from '@app/models/User';

@Injectable({
  providedIn: 'root',
})
export class AuthGuardService implements CanActivate, CanActivateChild {
  constructor(
    private appService: AppService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    const url: string = state.url;
    const expectedRoles = route.data.expectedRoles;
    return this.checkLogin(url, expectedRoles, route);
  }

  canActivateChild(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    return this.canActivate(route, state);
  }

  checkLogin(url: string, expectedRoles: any, route: ActivatedRouteSnapshot) {
    const userRoles: any = this.appService.getLoggedInUserRoles();
    const user: User = this.appService.getLoggedInUser();

    if (this.appService.isLoggedIn()) {
      if (user.userType == 'ADMIN') {
        if (user.isChgPwd == true) {
          if (!url.includes('change-password'))
            this.router.navigate([`/home/user/${user.id}/change-password`]);
          return true;
        }

        if (
          expectedRoles == null ||
          expectedRoles.some((r) => userRoles.indexOf(r) >= 0)
        ) {
          return true;
        }
      }
    }
    this.appService.redirectUrl = url;
    const appKey = this.appService.getLoggedInUserApplicationKey();
    this.router.navigate(['login', appKey]);
  }
}
