import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root',
})
export class HealthPlanService {
  constructor(private http: HttpClient) {}

  GetAll() {
    return this.http.get(`${environment.baseURL}/HealthPlan`);
  }

  GetAllAddOns(id: string) {
    return this.http.get(`${environment.baseURL}/HealthPlan/${id}/addons`);
  }
}
