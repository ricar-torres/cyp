import {
  Component,
  OnInit,
  ViewChild,
  AfterViewInit,
  ElementRef,
} from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { ApiService } from '../../shared/api.service';
import { AppService } from '../../shared/app.service';
import { faEye } from '@fortawesome/free-solid-svg-icons';
import { FilesDataSource } from './FilesDataSource';
import { tap, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { merge, fromEvent } from 'rxjs';

@Component({
  selector: 'app-file-list',
  templateUrl: './file-list.component.html',
  styleUrls: ['./file-list.component.css'],
})
export class FileListComponent implements AfterViewInit, OnInit {
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('inputLastName') inputLastName: ElementRef;
  @ViewChild('inputFirstName') inputFirstName: ElementRef;
  @ViewChild('inputReference1') inputReference1: ElementRef;
  @ViewChild('inputKey') inputKey: ElementRef;

  faEye = faEye;

  length;
  pageSize = 25;
  pageSizeOptions: number[] = [this.pageSize];
  pageIndex = 0;

  displayedColumns: string[] = [
    'key',
    'fullName',
    'ref1',
    'ref2',
    'type',
    'status',
  ];
  loading = false;
  onlyActive = true;

  dataSource: FilesDataSource;

  constructor(
    private router: Router,
    private api: ApiService,
    private app: AppService
  ) {}

  ngAfterViewInit() {
    // server-side search
    merge(
      fromEvent(this.inputLastName.nativeElement, 'keyup'),
      fromEvent(this.inputFirstName.nativeElement, 'keyup'),
      fromEvent(this.inputReference1.nativeElement, 'keyup'),
      fromEvent(this.inputKey.nativeElement, 'keyup')
    )
      .pipe(
        debounceTime(150),
        distinctUntilChanged(),
        tap(() => {
          this.paginator.pageIndex = this.pageIndex;
          this.loadFilesPage();
        })
      )
      .subscribe();

    this.dataSource.counter$
      .pipe(
        tap((count) => {
          this.paginator.length = count;
        })
      )
      .subscribe();

    this.dataSource.loading$
      .pipe(
        tap((loading) => {
          this.loading = loading;
        })
      )
      .subscribe();

    this.sort.sortChange.subscribe(
      () => (this.paginator.pageIndex = this.pageIndex)
    );

    merge(this.sort.sortChange, this.paginator.page)
      .pipe(tap(() => this.loadFilesPage()))
      .subscribe();

    this.paginator._intl.itemsPerPageLabel = '';
  }

  ngOnInit() {
    this.dataSource = new FilesDataSource(this.api);
    this.dataSource.loadFiles(
      '',
      'asc',
      this.pageIndex,
      this.pageSize,
      '',
      '',
      '',
      '',
      this.onlyActive
    );
  }

  loadFilesPage() {
    this.dataSource.loadFiles(
      this.sort.active,
      this.sort.direction,
      this.paginator.pageIndex,
      this.paginator.pageSize,
      this.inputLastName.nativeElement.value,
      this.inputFirstName.nativeElement.value,
      this.inputReference1.nativeElement.value,
      this.inputKey.nativeElement.value,
      this.onlyActive
    );
  }

  goToDetail(id) {
    this.router.navigate(['/home/file', id]).then((e) => {
      if (e) {
      } else {
      }
    });
  }
}
