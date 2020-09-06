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

<<<<<<< HEAD


  insuranceCompaniesList(){
    return this.http.get(`${environment.baseURL}/HealthPlan/`);
  }


  insuranceCompaniesDelete(id:string){
    return this.http.delete(`${environment.baseURL}/HealthPlan/${id}`).toPromise();
  }


  insuranceCompanyById(id:string){
    return this.http.get(`${environment.baseURL}/HealthPlan/${id}`).toPromise();
  }
  
  
  insuranceCompanySave(id:string, item: any){
    return this.http.put(`${environment.baseURL}/HealthPlan/${id}`,item).toPromise();
  }


  addOptionalCover(id:string, item: any){

    return this.http.post(`${environment.baseURL}/HealthPlan/${id}/OptionalCover/${item.id}`,item).toPromise();
  
  }


  insurancePlanById(id:string){ 
    return this.http.get(`${environment.baseURL}/Covers/${id}`).toPromise();
  }


  insurancePlanDelete(id:string){ 
    return this.http.delete(`${environment.baseURL}/Covers/${id}`).toPromise();
  }
  
  InsurancePlansave(id:string, item: any){

    return this.http.put(`${environment.baseURL}/Covers/${id}`,item).toPromise();
  
  }

  insurancePlanCreate(item: any){

    return this.http.post(`${environment.baseURL}/Covers`,item).toPromise();
  
  }

  
  BenefitTypesList(){ 
    return this.http.get(`${environment.baseURL}/HealthPlan/InsuranceBenefitTypes/`).toPromise();
  }

  
  addBenefitTypes(InsurancePlanId:string, item: any){

    return this.http.post(`${environment.baseURL}/Covers/${InsurancePlanId}/BenefitType/`,item).toPromise();
  
  }



  insurancePlanRateById(id:string){ 
    return this.http.get(`${environment.baseURL}/Covers/${id}/Rate`).toPromise();
  }


  addonAvailable(healthPlanId:string){ 
    return this.http.get(`${environment.baseURL}/HealthPlan/${healthPlanId}/addons/`).toPromise();
  }


  addonsUpdate(id:string, payload:any){
    return this.http.put(`${environment.baseURL}/HealthPlan/Addons/${id}`,payload).toPromise();
  }

  addonsCreate(healthPlanId:string, payload:any){ 
    return this.http.post(`${environment.baseURL}/HealthPlan/${healthPlanId}/Addons`,payload).toPromise();
  }

  addonsDelete(id:string){
    return this.http.delete(`${environment.baseURL}/HealthPlan/Addons/${id}`).toPromise();
  }

  planAddOnsDelete(InsurancePlanId:string, InsuranceAddOnsId:string){
    return this.http.delete(`${environment.baseURL}/Covers/${InsurancePlanId}/AddOns/${InsuranceAddOnsId}`).toPromise();
  }

  insuranceEstimatesDownload(id:string){

    const httpOptions = {
      responseType: 'blob' as 'json',
      //headers: this.headers
    };

    return this.http.get(`${environment.baseURL}/insuranceEstimates/${id}/export/excel`,httpOptions);
  }

  insuranceCompanies(){
    return this.http.get(`${environment.baseURL}/HealthPlan/`).toPromise();
  }



=======
  GetAllAddOns(id: string) {
    return this.http.get(`${environment.baseURL}/HealthPlan/${id}/addons`);
  }
>>>>>>> ca88cee6fecd415628af5a50c4d5db8b71400807
}
