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
export class CommunicationMethodsAPIService {
  private readonly _apiName = 'CommunicationMethod';
  /**
   *
   */
  constructor(private http: HttpClient) {}

  getAll(): Observable<any> {
    return this.http.get(`${environment.baseURL}/${this._apiName}`);
  }
  delete(id: string) {
    try {
      return this.http
        .delete(`${environment.baseURL}/${this._apiName}/${id}`)
        .toPromise();
    } catch (error) {}
  }
  getById(id: string): Promise<any> {
    try {
      return this.http
        .get(`${environment.baseURL}/${this._apiName}/${id}`)
        .toPromise();
    } catch (error) {}
  }
  create(communicationiMethod) {
    return this.http
      .post(`${environment.baseURL}/${this._apiName}`, communicationiMethod)
      .toPromise();
  }
  update(id, campaign) {
    return this.http
      .put(`${environment.baseURL}/${this._apiName}/${id}`, campaign)
      .toPromise();
  }
  checkNameExist(name) {
    return this.http
      .get(`${environment.baseURL}/${this._apiName}/CheckNameExist/`, {
        params: new HttpParams().set('name', name),
      })
      .toPromise();
  }
}
