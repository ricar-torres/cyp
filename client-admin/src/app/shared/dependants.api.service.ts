import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DependantsAPIService {
  private readonly _apiName = 'Dependants';
  /**
   *
   */
  constructor(private http: HttpClient) {}

  getAll(): Observable<any> {
    return this.http.get(`${environment.baseURL}/${this._apiName}`);
  }
  delete(id: string) {
    try {
      return this.http
        .delete(`${environment.baseURL}/${this._apiName}/${id}`)
        .toPromise();
    } catch (error) {}
  }
  getById(id: string): Promise<any> {
    try {
      return this.http
        .get(`${environment.baseURL}/${this._apiName}/${id}`)
        .toPromise();
    } catch (error) {}
  }
  create(payload) {
    return this.http
      .post(`${environment.baseURL}/${this._apiName}`, payload)
      .toPromise();
  }
  update(id, campaign) {
    return this.http
      .put(`${environment.baseURL}/${this._apiName}/${id}`, campaign)
      .toPromise();
  }

  getAllByClient(id: number | string): Observable<any> {
    return this.http.get(
      `${environment.baseURL}/${this._apiName}/GetAllByClient/${id}`
    );
  }
  getRelationTypes() {
    return this.http
      .get(`${environment.baseURL}/${this._apiName}/GetRelationTypes`)
      .toPromise();
  }
}
