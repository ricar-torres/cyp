import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { ApiService } from '../../shared/api.service';
import { catchError, finalize } from 'rxjs/operators';
import { User } from '@app/models/User';
//import { User } from './models/user.model';

export class FilesDataSource implements DataSource<any> {
  // add variables to hold the data and number of total records retrieved asynchronously
  // BehaviourSubject type is used for this purpose
  private filesSubject = new BehaviorSubject<any[]>([]);
  private loadingSubject = new BehaviorSubject<boolean>(false);

  public loading$ = this.loadingSubject.asObservable();

  // to show the total number of records
  private countSubject = new BehaviorSubject<number>(0);
  public counter$ = this.countSubject.asObservable();

  constructor(private api: ApiService) {}

  loadFiles(
    sort: string,
    order: string,
    pageIndex: number,
    pageSize: number,
    lastName: string,
    firstName: string,
    reference1: string,
    key: string,
    onlyActive: boolean
  ) {
    this.loadingSubject.next(true);

    // use pipe operator to chain functions with Observable type
    this.api
      .files(
        `{lastName: "${lastName}",firstName: "${firstName}", reference:"${reference1}", key: "${key}", onlyActive: ${
          onlyActive ? 'true' : 'false'
        }}`,
        sort,
        order,
        pageIndex + 1,
        pageSize
      )
      .pipe(
        catchError(() => of([])),
        finalize(() => this.loadingSubject.next(false))
      )
      // subscribe method to receive Observable type data when it is ready
      .subscribe((result: any) => {
        this.filesSubject.next(result.data);
        this.countSubject.next(result.totalPages);
      });
  }

  connect(collectionViewer: CollectionViewer): Observable<User[]> {
    return this.filesSubject.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer): void {
    this.filesSubject.complete();
    this.countSubject.complete();
    this.loadingSubject.complete();
  }
}
