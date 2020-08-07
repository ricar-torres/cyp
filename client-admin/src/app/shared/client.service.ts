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

  update(client: any) {
    return this.http
      .put(`${environment.baseURL}/clients/${client.Id}`, client)
      .toPromise();
  }

  delete(id: string) {
    return this.http.delete(`${environment.baseURL}/clients/${id}`).toPromise();
  }
}
