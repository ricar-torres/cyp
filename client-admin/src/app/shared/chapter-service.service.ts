import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ChapterServiceService {
  constructor(private http: HttpClient) {}

  checkName(obj: { name: string }): any {
    return this.http
      .get(`${environment.baseURL}/chapter/CheckName/${obj.name}`)
      .toPromise();
  }

  getAll() {
    return this.http.get(`${environment.baseURL}/chapter`);
  }

  create(agency) {
    return this.http.post(`${environment.baseURL}/chapter`, agency).toPromise();
  }

  agency(id: string) {
    return this.http.get(`${environment.baseURL}/chapter/${id}`).toPromise();
  }

  update(agency: any) {
    return this.http
      .put(`${environment.baseURL}/chapter/${agency.id}`, agency)
      .toPromise();
  }

  delete(id: string) {
    return this.http.delete(`${environment.baseURL}/chapter/${id}`).toPromise();
  }
}
