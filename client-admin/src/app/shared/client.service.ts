import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ClientService {
  toggleEditControl: EventEmitter<boolean> = new EventEmitter<boolean>();
  saveClientEdits: EventEmitter<boolean> = new EventEmitter<boolean>();

  checkName(obj: { name: string }): any {
    return this.http
      .get(`${environment.baseURL}/clients/CheckName/${obj.name}`)
      .toPromise();
  }

  getClientsByCriteria(value: any) {
    return this.http.get(`${environment.baseURL}/clients/criteria/${value}`);
  }

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get(`${environment.baseURL}/clients`);
  }

  create(client) {
    return this.http.post(`${environment.baseURL}/clients`, client).toPromise();
  }

  client(id: string) {
    return this.http.get(`${environment.baseURL}/clients/${id}`).toPromise();
  }

  update(payload: any) {
    return this.http.put(`${environment.baseURL}/clients`, payload).toPromise();
  }

  Decesed(clinetId) {
    return this.http.get(`${environment.baseURL}/clients/${clinetId}/Deceased`);
  }

  delete(id: string) {
    return this.http.delete(`${environment.baseURL}/clients/${id}`).toPromise();
  }

  checkSsn(payload: { ssn: any }) {
    return this.http
      .post(`${environment.baseURL}/clients/checkSsn`, payload)
      .toPromise();
  }
}
