import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ChapterServiceService {
  constructor(private http: HttpClient) {}

  getByBonafideById(id: string) {
    return this.http.get(`${environment.baseURL}/chapter/bonafide/${id}`);
  }
  checkName(obj: { name: string }): any {
    return this.http
      .get(`${environment.baseURL}/chapter/CheckName/${obj.name}`)
      .toPromise();
  }

  getAll() {
    return this.http.get(`${environment.baseURL}/chapter`);
  }

  create(chapter) {
    return this.http
      .post(`${environment.baseURL}/chapter`, chapter)
      .toPromise();
  }

  chapter(id: string) {
    return this.http.get(`${environment.baseURL}/chapter/${id}`).toPromise();
  }

  update(agency: any) {
    return this.http
      .put(`${environment.baseURL}/chapter/${agency.Id}`, agency)
      .toPromise();
  }

  delete(id: string) {
    return this.http.delete(`${environment.baseURL}/chapter/${id}`).toPromise();
  }
}
