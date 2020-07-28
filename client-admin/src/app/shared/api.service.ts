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
export class ApiService {
  errorData: {};

  constructor(private http: HttpClient) {}

  //#region APPLICATION

  applicationByKey(key: string) {
    return this.http
      .get(`${environment.baseURL}/application/${key}`)
      .toPromise();
  }

  //#endregion

  //#region AUTH

  login(payload: any) {
    return this.http
      .post(`${environment.baseURL}/auth/token`, payload)
      .toPromise();
  }

  //#endregion

  //#region USERS

  users() {
    return this.http.get(`${environment.baseURL}/users/`);
  }

  user(id) {
    return this.http.get(`${environment.baseURL}/users/${id}`).toPromise();
  }

  changePassword(payload) {
    return this.http
      .patch(`${environment.baseURL}/users/ChangePassword`, payload)
      .toPromise();
  }

  userUpdate(id, payload) {
    return this.http
      .put(`${environment.baseURL}/users/${id}`, payload)
      .toPromise();
  }

  userCreate(id, payload) {
    return this.http.post(`${environment.baseURL}/users/`, payload).toPromise();
  }

  roles() {
    return this.http.get(`${environment.baseURL}/roles/`).toPromise();
  }

  //#endregion

  //#region FILES

  files(
    filter = '',
    sort = 'asc',
    order = '',
    pageNumber = 0,
    pageSize = 3
  ): Observable<any[]> {
    return this.http.get<any[]>(`${environment.baseURL}/files/`, {
      params: new HttpParams()
        .set('filter', filter)
        .set('sort', sort)
        .set('order', order)
        .set('pageNumber', pageNumber.toString())
        .set('pageSize', pageSize.toString()),
    });
  }

  file(id) {
    return this.http.get(`${environment.baseURL}/files/${id}`).toPromise();
  }

  //#endregion

  //#region COMMENTS

  comments(fileId) {
    return this.http
      .get(`${environment.baseURL}/files/${fileId}/comments/`)
      .toPromise();
  }

  commentCreate(fileId, payload) {
    return this.http
      .post(`${environment.baseURL}/files/${fileId}/comments/`, payload)
      .toPromise();
  }

  //#endregion

  //#region DOCUMENTS

  fileDocuments(fileId, onlyActive: boolean) {
    return this.http
      .get(
        `${environment.baseURL}/files/${fileId}/documents/?onlyActive=${onlyActive}`
      )
      .toPromise();
  }

  fileDocument(id) {
    return this.http
      .get(`${environment.baseURL}/files/documents/${id}`)
      .toPromise();
  }

  fileDocumentCreate(fileId, payload) {
    return this.http.post(
      `${environment.baseURL}/files/${fileId}/documents/`,
      payload
    );
  }

  fileDocumentsCreate(fileId, payload) {
    return this.http
      .post<any>(`${environment.baseURL}/files/${fileId}/documents/`, payload, {
        reportProgress: true,
        observe: 'events',
      })
      .pipe(
        map((event) => {
          switch (event.type) {
            case HttpEventType.UploadProgress:
              const progress = Math.round((100 * event.loaded) / event.total);
              return { progress: true, message: progress };

            case HttpEventType.Response:
              return event.body;
            default:
              return `Unhandled event: ${event.type}`;
          }
        })
      );
  }

  fileDocumentUpdate(id, payload) {
    return this.http
      .put(`${environment.baseURL}/files/documents/${id}`, payload)
      .toPromise();
  }

  documentTypes() {
    return this.http.get(`${environment.baseURL}/documents/types`).toPromise();
  }

  documentType(id) {
    return this.http
      .get(`${environment.baseURL}/documents/types/${id}`)
      .toPromise();
  }

  updateDocumentType(id, payload) {
    return this.http
      .put(`${environment.baseURL}/documents/types/${id}`, payload)
      .toPromise();
  }

  createDocumentType(payload) {
    return this.http
      .post(`${environment.baseURL}/documents/types`, payload)
      .toPromise();
  }

  //#endregion

  //#region TASKS

  tasks(allTasks: boolean): Promise<Task[]> {
    return <Promise<Task[]>>(
      this.http
        .get(`${environment.baseURL}/tasks?allTasks=${allTasks}`)
        .toPromise()
    );
  }

  tasksByFile(key: string): Promise<Task[]> {
    return <Promise<Task[]>>(
      this.http.get(`${environment.baseURL}/tasks/file/${key}`).toPromise()
    );
  }

  task(id: string) {
    return this.http.get(`${environment.baseURL}/tasks/${id}`).toPromise();
  }

  taskCreate(payload: Task, templateFile: File, lag: string) {
    let formData: FormData = new FormData();

    if (templateFile) {
      formData.append(
        'file',
        templateFile,
        templateFile.name.replace(' ', '-')
      );
    }

    formData.append('task', JSON.stringify(payload));
    return this.http
      .post(`${environment.baseURL}/tasks?lan=${lag}`, formData)
      .toPromise();
  }

  taskPatch(id: number, payload: Task): Promise<Task> {
    return <Promise<Task>>(
      this.http.patch(`${environment.baseURL}/tasks/${id}`, payload).toPromise()
    );
  }

  documentDownload(id: string, fileName: string): Observable<Download> {
    return this.http
      .get(`${environment.baseURL}/documents/${id}/download`, {
        reportProgress: true,
        observe: 'events',
        responseType: 'blob',
      })
      .pipe(this.download((blob) => saveAs(blob, fileName)));
  }

  download(
    saver?: (b: Blob) => void
  ): (source: Observable<HttpEvent<Blob>>) => Observable<Download> {
    return (source: Observable<HttpEvent<Blob>>) =>
      source.pipe(
        scan(
          (previous: Download, event: HttpEvent<Blob>): Download => {
            if (this.isHttpProgressEvent(event)) {
              return {
                progress: event.total
                  ? Math.round((100 * event.loaded) / event.total)
                  : previous.progress,
                state: 'IN_PROGRESS',
                content: null,
              };
            }
            if (this.isHttpResponse(event)) {
              if (saver && event.body) {
                saver(event.body);
              }
              return {
                progress: 100,
                state: 'DONE',
                content: event.body,
              };
            }
            return previous;
          },
          { state: 'PENDING', progress: 0, content: null }
        )
      );

    //#endregion
  }

  isHttpResponse<T>(event: HttpEvent<T>): event is HttpResponse<T> {
    return event.type === HttpEventType.Response;
  }

  isHttpProgressEvent(event: HttpEvent<unknown>): event is HttpProgressEvent {
    return (
      event.type === HttpEventType.DownloadProgress ||
      event.type === HttpEventType.UploadProgress
    );
  }
}
