import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class QualifyingEventService {
  checkName(obj: { name: string }): any {
    return this.http
      .get(`${environment.baseURL}/qualifyingevents/CheckName/${obj.name}`)
      .toPromise();
  }

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get(`${environment.baseURL}/qualifyingevents`);
  }

  create(qualifyingevent) {
    return this.http
      .post(`${environment.baseURL}/qualifyingevents`, qualifyingevent)
      .toPromise();
  }

  qualifyingevent(id: string) {
    return this.http
      .get(`${environment.baseURL}/qualifyingevents/${id}`)
      .toPromise();
  }

  update(qualifyingevent: any) {
    return this.http
      .put(
        `${environment.baseURL}/qualifyingevents/${qualifyingevent.Id}`,
        qualifyingevent
      )
      .toPromise();
  }

  delete(id: string) {
    return this.http
      .delete(`${environment.baseURL}/qualifyingevents/${id}`)
      .toPromise();
  }
}
