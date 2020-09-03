import { LanguageService } from '../../shared/Language.service';
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
import { DialogSuccessComponent } from '@app/components/dialog-success/dialog-success.component';

@Component({
  selector: 'app-docs-calls-list',
  templateUrl: './docs-calls-list.component.html',
  styleUrls: ['./docs-calls-list.component.css'],
})
export class DocsCallsListComponent implements OnInit, AfterViewInit {
  threads: any[] = [];

  loading: boolean = true;
  @Output()
  isLoadingEvent = new EventEmitter<boolean>();
  isloading: boolean;
  @Input()
  clientId: string;
  constructor(
    private dialog: MatDialog,
    private apiDocCall: DocumentationCallAPIService,
    private app: AppService
  ) {}
  async ngAfterViewInit() {}

  async ngOnInit() {
    await this.loadData();
  }

  async createThread(masterThreadId?: string | number) {
    if (masterThreadId == null) {
      masterThreadId = '000000000000';
    }
    const dialogRef = this.dialog.open(DocumentationCallComponent, {
      data: {
        confirmationNumber: masterThreadId,
        clientId: this.clientId,
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
    this.isloading = true;
    this.isLoadingEvent.emit(this.isloading);
    try {
      this.apiDocCall.getClientDocCalls(this.clientId).subscribe(
        (data: any) => {
          this.isloading = true;
          this.isLoadingEvent.emit(this.isloading);
          this.threads = data;
          this.isloading = false;
          this.isLoadingEvent.emit(this.isloading);
        },
        (error: any) => {
          this.isloading = false;
          this.isLoadingEvent.emit(this.isloading);
          if (error.status != 401) {
            //console.error('error', error);
            this.app.showErrorMessage('Error interno');
          }
        }
      );
    } catch (error) {
      this.isloading = false;
      this.isLoadingEvent.emit(this.isloading);
    } finally {
      this.isloading = false;
      this.isLoadingEvent.emit(this.isloading);
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
