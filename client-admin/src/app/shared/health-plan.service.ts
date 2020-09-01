import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root',
})
export class HealthPlanService {
  readonly controllerName = 'HealthPlan';
  constructor(private http: HttpClient) {}

  GetAll() {
    return this.http.get(`${environment.baseURL}/HealthPlan`);
  }
  GetAllMultiAssist() {
    return this.http.get(
      `${environment.baseURL}/${this.controllerName}/GetAllMultiAssist`
    );
  }
}
