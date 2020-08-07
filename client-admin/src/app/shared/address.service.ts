import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AddressService {
  constructor(private http: HttpClient) {}

  getClientAddress(clientId: string) {
    return this.http
      .get(`${environment.baseURL}/address/${clientId}`)
      .toPromise();
  }

  updateClientAddress(payload: any) {
    return this.http
      .put(`${environment.baseURL}/address/${payload.Id}`, payload)
      .toPromise();
  }
}
