import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AgencyService {
  Update(agency: any) {
    return this.http
      .put(`${environment.baseURL}/Agencies/${agency.Id}`, agency)
      .toPromise();
  }
  constructor(private http: HttpClient) {}

  GetAll() {
    return this.http.get(`${environment.baseURL}/Agencies`);
  }

  Create(agency) {
    return this.http
      .post(`${environment.baseURL}/Agencies`, agency)
      .toPromise();
  }

  Agency(id: string) {
    return this.http.get(`${environment.baseURL}/Agencies/${id}`).toPromise();
  }

  Delete(id: string) {
    return this.http
      .delete(`${environment.baseURL}/Agencies/${id}`)
      .toPromise();
  }
}
