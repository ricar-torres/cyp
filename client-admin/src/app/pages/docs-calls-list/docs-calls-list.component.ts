import { LanguageService } from '../../shared/Language.service';
import { DocCall } from '../../models/DocCall';
import { Observable } from 'rxjs';
import { AppService } from '@app/shared/app.service';
import { DocumentationCallAPIService } from '../../shared/documentation-call.api.service';
import { GenericSucessModel } from '../../models/GenericSuccessModel';
import { DocumentationCallComponent } from '../../components/documentation-call/documentation-call.component';
import {
  Component,
  OnInit,
  Input,
  AfterViewInit,
  Output,
  EventEmitter,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {
  ConfirmDialogModel,
  ConfirmDialogComponent,
} from '@app/components/confirm-dialog/confirm-dialog.component';
import { title } from 'process';
import { Console } from 'console';
import { DialogGenericSuccessComponent } from '@app/components/dialog-generic-success/dialog-generic-success.component';
import { MatTableDataSource, PageEvent } from '@angular/material';
import { map } from 'rxjs/operators';
import { DialogSuccessComponent } from '@app/components/dialog-success/dialog-success.component';

@Component({
  selector: 'app-docs-calls-list',
  templateUrl: './docs-calls-list.component.html',
  styleUrls: ['./docs-calls-list.component.css'],
})
export class DocsCallsList implements OnInit, AfterViewInit {
  threads: any[] = [];

  loading: boolean;
  @Output()
  isLoadingEvent = new EventEmitter<boolean>();

  @Input()
  clientId: string;
  constructor(
    private dialog: MatDialog,
    private apiDocCall: DocumentationCallAPIService,
    private app: AppService,
    private lang: LanguageService
  ) {}
  async ngAfterViewInit() {}

  async ngOnInit() {
    await this.loadData();
  }

  async createThread(masterThreadId) {
    if (masterThreadId == null) {
      masterThreadId = '000000000000';
    }
    const dialogRef = this.dialog.open(DocumentationCallComponent, {
      data: {
        confirmationNumber: masterThreadId,
        clientId: 1,
      },
    });
    dialogRef.afterClosed().subscribe(async (dialogResult) => {
      await this.loadData();
      if (dialogResult) {
        this.dialog.open(DialogSuccessComponent, {
          width: '300px',
          height: '200px',
          data: new GenericSucessModel(
            'DOCUMENTATION_CALL.SUCCESS',
            dialogResult
          ),
        });
      }
    });
  }

  async loadData() {
    try {
      this.isLoadingEvent.emit(true);
      await this.apiDocCall.getClientDocCalls(this.clientId).subscribe(
        (data: any) => {
          this.threads = data;
          this.isLoadingEvent.emit(false);
        },
        (error: any) => {
          this.isLoadingEvent.emit(false);
          if (error.status != 401) {
            console.error('error', error);
            this.app.showErrorMessage('Error interno');
          }
        }
      );
    } catch (error) {
      this.isLoadingEvent.emit(false);
    }
  }

  callSuccessDialog() {
    this.dialog.open(DialogSuccessComponent, {
      width: '350px',
      height: '200px',
      data: new GenericSucessModel('SUCCESS', '0000000000'),
    });
  }
  doFilter(filter) {
    this.threads = this.threads.filter(
      filter.toString().trim().toLocaleLowerCase()
    );
  }
}
