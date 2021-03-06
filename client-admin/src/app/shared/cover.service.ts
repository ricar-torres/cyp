import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class CoverService {
  GetByCover(res: any) {
    throw new Error('Method not implemented.');
  }
  constructor(private http: HttpClient) {}

  GetAll() {
    return this.http.get<[]>(`${environment.baseURL}/Covers`).toPromise();
  }
  GetByPlan(planId: number) {
    return this.http.get<[]>(`${environment.baseURL}/Covers/Healthplan/${planId}`);
  }

  GetPlanByCover(coverId: number) {
    return this.http
      .get<any>(`${environment.baseURL}/Covers/plan/${coverId}`)
      .toPromise();
  }

  GetAllAddOns(id: string) {
    return this.http.get(`${environment.baseURL}/Covers/${id}/addons`);
  }
}
