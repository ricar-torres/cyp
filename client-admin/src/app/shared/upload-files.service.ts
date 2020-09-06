import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
  HttpEventType
} from '@angular/common/http';
// import * as base64 from 'base-64';
// import * as utf8 from 'utf8';
import { throwError, Observable, of, observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
// import { Login } from '../models/login';
//import { UploadfilesService } from '../shared/upload-files.service';
import { environment } from '../../environments/environment';
export const InterceptorSkipHeader = 'X-Skip-Interceptor';

@Injectable({
  providedIn: 'root'
})
export class UploadfilesService {
  errorData: {};



  constructor( app: UploadfilesService, private http: HttpClient) {
      
  }


  AddPlanRateUpload( id, policyYear, payload) {

    const headers = new HttpHeaders().set(InterceptorSkipHeader, '');
    
    return this.http.post<any>(`${environment.baseURL}/InsurancePlans/${id}/Rate/${policyYear}/Upload/`, payload, {
      headers,
      reportProgress: true,
      observe: 'events'
    }).pipe(map((event) => {

      switch (event.type) {

        case HttpEventType.UploadProgress:
          const progress = Math.round(100 * event.loaded / event.total);
          return { status: 'progress', message: progress };

        case HttpEventType.Response:
          return event.body;
        default:
          return `Unhandled event: ${event.type}`;
      }
    })
    );
    
  }



}