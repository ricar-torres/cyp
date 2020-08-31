import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class CoverService {
  constructor(private http: HttpClient) {}

  GetAll() {
    return this.http.get<[]>(`${environment.baseURL}/Covers`).toPromise();
  }
  GetByPlan(planId: number) {
    return this.http.get<[]>(`${environment.baseURL}/Covers/${planId}`);
  }

  GetPlanByCover(coverId: number) {
    return this.http
      .get<any>(`${environment.baseURL}/Covers/plan/${coverId}`)
      .toPromise();
  }
}
