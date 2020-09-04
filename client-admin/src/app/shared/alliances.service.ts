import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AlliancesService {
  checkName(obj: { name: string }): any {
    return this.http
      .get(`${environment.baseURL}/Alliance/CheckName/${obj.name}`)
      .toPromise();
  }

  constructor(private http: HttpClient) {}

  getAll(clientId: string) {
    if (clientId) {
      return this.http.get<[]>(
        `${environment.baseURL}/Alliance/getall/${clientId}`
      );
    } else {
      return this.http.get<[]>(`${environment.baseURL}/Alliance/getall`);
    }
  }

  create(alliance) {
    return this.http
      .post(`${environment.baseURL}/Alliance`, alliance)
      .toPromise();
  }

  alliance(id: string) {
    return this.http.get(`${environment.baseURL}/Alliance/${id}`).toPromise();
  }

  AlianceRequest(request) {
    return this.http.get(
      `${environment.baseURL}/Alliance/AlianceRequest/${request}`
    );
  }

  iselegible(clientid) {
    return this.http.get(
      `${environment.baseURL}/Alliance/client/${clientid}/iselegible`
    );
  }

  update(alliance: any) {
    return this.http
      .put(`${environment.baseURL}/Alliance/${alliance.Id}`, alliance)
      .toPromise();
  }

  delete(id: string) {
    return this.http
      .delete(`${environment.baseURL}/Alliance/${id}`)
      .toPromise();
  }
}
