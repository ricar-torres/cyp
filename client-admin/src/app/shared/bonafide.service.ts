import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root',
})
export class bonaFideservice {
  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get(`${environment.baseURL}/bonaFides`);
  }

  create(banafide) {
    return this.http
      .post(`${environment.baseURL}/bonaFides`, banafide)
      .toPromise();
  }

  bonafide(id: string) {
    return this.http.get(`${environment.baseURL}/bonaFides/${id}`).toPromise();
  }

  update(banafide: any) {
    return this.http
      .put(`${environment.baseURL}/bonaFides/${banafide.Id}`, banafide)
      .toPromise();
  }

  delete(id: string) {
    return this.http
      .delete(`${environment.baseURL}/bonaFides/${id}`)
      .toPromise();
  }

  checkName(obj: { name: string }): any {
    return this.http
      .get(`${environment.baseURL}/bonaFides/CheckName/${obj.name}`)
      .toPromise();
  }
  checkEmail(obj: { name: string }): any {
    console.log(obj.name);
    return this.http
      .get(`${environment.baseURL}/bonaFides/CheckEmail/${obj.name}`)
      .toPromise();
  }
}
