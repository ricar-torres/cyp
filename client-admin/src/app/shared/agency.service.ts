import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AgencyService {
  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get(`${environment.baseURL}/agencies`);
  }

  create(agency) {
    return this.http
      .post(`${environment.baseURL}/agencies`, agency)
      .toPromise();
  }

  agency(id: string) {
    return this.http.get(`${environment.baseURL}/agencies/${id}`).toPromise();
  }

  update(agency: any) {
    return this.http
      .put(`${environment.baseURL}/agencies/${agency.Id}`, agency)
      .toPromise();
  }

  delete(id: string) {
    return this.http
      .delete(`${environment.baseURL}/agencies/${id}`)
      .toPromise();
  }
}
