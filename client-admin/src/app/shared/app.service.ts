import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DialogGenericErrorComponent } from 'src/app/components/dialog-generic-error/dialog-generic-error.component';
import { DialogGenericSuccessComponent } from '../components/dialog-generic-success/dialog-generic-success.component';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { User } from './../models/User';
import * as Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root',
})
export class AppService {
  constructor(private dialog: MatDialog, private router: Router) {}

  redirectUrl: string;

  isLoggedIn() {
    if (localStorage.getItem('currentUser')) {
      return true;
    }
    return false;
  }

  setApplicationKey(applicationKey: string) {
    localStorage.setItem('applicationKey', applicationKey);
  }

  setLoggedInUser(user: User) {
    localStorage.setItem('currentUser', JSON.stringify(user));
  }

  getLoggedInUser() {
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    return currentUser;
  }

  getLoggedInDisplayName() {
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    return currentUser.username;
  }

  getToken() {
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    return currentUser ? currentUser.token : '';
  }

  getLoggedInUserRoles() {
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    return currentUser.claims;
  }

  getLoggedInUserApplication() {
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    return currentUser.applicationId;
  }

  getLoggedInUserApplicationKey() {
    return localStorage.getItem('applicationKey');
  }

  checkMenuRoleAccess(menuRole: string[]) {
    return menuRole.some((r) => this.getLoggedInUserRoles().indexOf(r) >= 0);
  }

  cleanSession() {
    localStorage.removeItem('applicationKey');
    localStorage.removeItem('currentUser');
  }

  logout() {
    const appKey = this.getLoggedInUserApplicationKey();
    this.router.navigate(['/login', appKey]);
  }

  showError(error: HttpErrorResponse) {
    Swal.default.fire({
      icon: 'error',
      title: error.status >= 400 && error.status < 500 ? '' : 'Error interno',
      text:
        error.status == 0
          ? 'favor de contactar al administrador del sistema'
          : error.error,
      heightAuto: false,
      // footer: '<a href>Why do I have this issue?</a>'
    });

    // this.dialog.open(DialogGenericErrorComponent, {
    //   data: {
    //     header:
    //       error.status >= 400 && error.status < 500 ? '' : 'Error interno',
    //     detail:
    //       error.status == 0
    //         ? 'favor de contactar al administrador del sistema'
    //         : error.error,
    //     icon: '',
    //   },
    // });
  }

  showErrorMessage(error: string) {
    Swal.default.fire({
      icon: 'error',
      title: 'Error',
      text: error,
      heightAuto: false,
      // footer: '<a href>Why do I have this issue?</a>'
    });


  

    // this.dialog.open(DialogGenericErrorComponent, {
    //   data: {
    //     header: 'Error',
    //     detail: error,
    //     icon: '',
    //   },
    // });
  }


  showMessage( header: string, msg: string, icon: string) {
    
    this.dialog.open(DialogGenericErrorComponent, {
      data: {
        header: header,
        detail: msg ,
        icon: icon
      }
    });
  }

  changeUserProperty(key: string, val) {
    var currentStorageUsr = JSON.parse(localStorage.getItem('currentUser'));
    currentStorageUsr[key] = val;
    localStorage.setItem('currentUser', JSON.stringify(currentStorageUsr));
  }
}
