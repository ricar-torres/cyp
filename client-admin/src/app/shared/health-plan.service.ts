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
    return this.http.get(`${environment.baseURL}/InsurancePlans/${id}`).toPromise();
  }


  insurancePlanDelete(id:string){ 
    return this.http.delete(`${environment.baseURL}/InsurancePlans/${id}`).toPromise();
  }
  
  insurancePlanSave(id:string, item: any){

    return this.http.put(`${environment.baseURL}/InsurancePlans/${id}`,item).toPromise();
  
  }

  insurancePlanCreate(item: any){

    return this.http.post(`${environment.baseURL}/InsurancePlans`,item).toPromise();
  
  }

  
  BenefitTypesList(){ 
    return this.http.get(`${environment.baseURL}/InsuranceBenefitTypes/`).toPromise();
  }

  
  addBenefitTypes(InsurancePlanId:string, item: any){

    return this.http.post(`${environment.baseURL}/InsurancePlans/${InsurancePlanId}/BenefitType/`,item).toPromise();
  
  }



  insurancePlanRateById(id:string){ 
    return this.http.get(`${environment.baseURL}/InsurancePlans/${id}/Rate`).toPromise();
  }


  addonAvailable(insuranceCompanyId:string){ 
    return this.http.get(`${environment.baseURL}/HealthPlan/${insuranceCompanyId}/addons/`).toPromise();
  }


  addonsUpdate(id:string, payload:any){
    return this.http.put(`${environment.baseURL}/HealthPlan/Addons/${id}`,payload).toPromise();
  }

  addonsCreate(InsuranceCompanyId:string, payload:any){ 
    return this.http.post(`${environment.baseURL}/HealthPlan/${InsuranceCompanyId}/Addons`,payload).toPromise();
  }

  addonsDelete(id:string){
    return this.http.delete(`${environment.baseURL}/HealthPlan/Addons/${id}`).toPromise();
  }

  planAddOnsDelete(InsurancePlanId:string, InsuranceAddOnsId:string){
    return this.http.delete(`${environment.baseURL}/InsurancePlans/${InsurancePlanId}/AddOns/${InsuranceAddOnsId}`).toPromise();
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



}
