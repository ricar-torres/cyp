import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpParams,
  HttpEventType,
  HttpEvent,
  HttpResponse,
  HttpProgressEvent,
} from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { map, scan } from 'rxjs/operators';
import { Task } from '@app/models/task';
import { saveAs } from 'file-saver';
import { Download } from '@app/models/document';

@Injectable({
  providedIn: 'root',
})
export class CampaignApiSerivce {
  private readonly _apiName = 'Campaigns';
  /**
   *
   */
  constructor(private http: HttpClient) {}

  getAll(): Observable<any> {
    return this.http.get(`${environment.baseURL}/${this._apiName}`);
  }
  deleteCampaign(campaignId: number) {
    try {
      return this.http
        .delete(`${environment.baseURL}/${this._apiName}/${campaignId}`)
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
  create(campaign) {
    return this.http
      .post(`${environment.baseURL}/${this._apiName}`, campaign)
      .toPromise();
  }
  update(id, campaign) {
    return this.http
      .put(`${environment.baseURL}/${this._apiName}/${id}`, campaign)
      .toPromise();
  }
}
