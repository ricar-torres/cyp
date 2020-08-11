import { DocCall } from './../models/DocCall';
import { map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpParams,
  HttpEventType,
  HttpEvent,
  HttpResponse,
  HttpProgressEvent,
} from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DocumentationCallAPIService {
  private readonly _apiName = 'DocumentationCall';
  /**
   *
   */
  constructor(private http: HttpClient) {}

  getAll(): Observable<any> {
    return this.http.get(`${environment.baseURL}/${this._apiName}`);
  }

  getById(id: string): Promise<any> {
    try {
      return this.http
        .get(`${environment.baseURL}/${this._apiName}/${id}`)
        .toPromise();
    } catch (error) {}
  }
  create(payload) {
    return this.http
      .post(`${environment.baseURL}/${this._apiName}`, payload)
      .toPromise();
  }

  getCallTypes(): Promise<any> {
    return this.http
      .get(`${environment.baseURL}/${this._apiName}/GetCallTypes`)
      .toPromise();
  }
  getClientDocCalls(clientId: string | number): Observable<any[]> {
    return this.http.get<any[]>(
      `${environment.baseURL}/${this._apiName}/GetClientCalls/${clientId}`
    );
  }
}
