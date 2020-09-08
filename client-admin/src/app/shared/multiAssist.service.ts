import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root',
})
export class MultiAssistService {
  controllerName = 'MultiAssist';
  constructor(private http: HttpClient) {}
  getMultiAssistPlans() {
    return this.http.get(
      `${environment.baseURL}/${this.controllerName}/GetMultiAssistPlans`
    );
  }
  getAll() {
    return this.http.get(`${environment.baseURL}/${this.controllerName}/`);
  }
  get(id: string | number): Promise<any> {
    try {
      return this.http
        .get<any>(`${environment.baseURL}/${this.controllerName}/${id}`)
        .toPromise();
    } catch (error) {}
  }
  create(payload, clientId: number | string) {
    return this.http
      .post(`${environment.baseURL}/${this.controllerName}`, {
        multiAssist: payload,
        clientId: clientId,
      })
      .toPromise();
  }

  update(payload, clientId: number | string) {
    // console.log(JSON.stringify(payload));
    return this.http
      .put(`${environment.baseURL}/${this.controllerName}`, {
        multiAssist: payload,
        clientId: clientId,
      })
      .toPromise();
  }
  delete(id: string) {
    // console.log(id);
    return this.http
      .delete(`${environment.baseURL}/${this.controllerName}/${id}`)
      .toPromise();
  }
}
