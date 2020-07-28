import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AppService } from './../shared/app.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router, private appService: AppService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    let headers = {
      //'Content-Type': 'application/json',
      Authorization: `Bearer ${this.appService.getToken()}`,
    };
    // if (request.headers.get('multipart') == 'true') {
    //   headers['Content-Type'] = 'multipart/form-data';
    // }
    request = request.clone({
      setHeaders: headers,
    });

    return next.handle(request).pipe(
      tap(
        () => {},
        (err: any) => {
          if (err instanceof HttpErrorResponse) {
            if (err.status !== 401) {
              return;
            }
            this.appService.logout();
            //this.router.navigate(['login']);
          }
        }
      )
    );
  }
}
