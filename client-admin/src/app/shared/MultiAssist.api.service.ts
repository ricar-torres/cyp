import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root',
})
export class MultiAssistAPIService {
  controllerName = 'MultiAssist';
  constructor(private http: HttpClient) {}
  GetMultiAssistPlans() {
    return this.http.get(
      `${environment.baseURL}/${this.controllerName}/GetMultiAssistPlans`
    );
  }
  GetAll() {
    return this.http.get(`${environment.baseURL}/${this.controllerName}/`);
  }
  Get(id: string | number): Promise<any> {
    try {
      return this.http
        .get<any>(`${environment.baseURL}/${this.controllerName}/${id}`)
        .toPromise();
    } catch (error) {}
  }
  Create(payload, clientId: number | string) {
    try {
      return this.http
        .post(`${environment.baseURL}/${this.controllerName}`, {
          multiAssist: payload,
          clientId: clientId,
        })
        .toPromise();
    } catch (error) {
      console.log(error);
    }
    // console.log(JSON.stringify(payload));
  }

  Update(payload, clientId: number | string) {
    console.log(JSON.stringify(payload));
    return this.http
      .put(`${environment.baseURL}/${this.controllerName}`, {
        multiAssist: payload,
        clientId: clientId,
      })
      .toPromise();
  }
  Delete(id: string) {
    console.log(id);
    return this.http
      .delete(`${environment.baseURL}/${this.controllerName}/${id}`)
      .toPromise();
  }
}
