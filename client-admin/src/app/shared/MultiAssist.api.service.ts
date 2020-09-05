import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root',
})
export class MultiAssistAPIService {
  controllerName = 'MultiAssist';
  constructor(private http: HttpClient) {}
  GetAllMultiAssist() {
    return this.http.get(`${environment.baseURL}/${this.controllerName}/`);
  }
  Create(payload, clientId: number | string) {
    return this.http
      .post(`${environment.baseURL}/${this.controllerName}`, {
        multiAssist: payload,
        clientId: clientId,
      })
      .toPromise();
  }
}
