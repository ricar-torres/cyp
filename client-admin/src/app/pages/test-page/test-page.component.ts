import { DocCall } from './../../models/DocCall';
import { Observable } from 'rxjs';
import { AppService } from '@app/shared/app.service';
import { DocumentationCallAPIService } from './../../shared/documentation-call.api.service';
import { GenericSucessModel } from './../../models/GenericSuccessModel';
import { DocumentationCallComponent } from './../../components/documentation-call/documentation-call.component';
import { Component, OnInit } from '@angular/core';
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

@Component({
  selector: 'app-test-page',
  templateUrl: './test-page.component.html',
  styleUrls: ['./test-page.component.css'],
})
export class TestPageComponent implements OnInit {
  threads: any[] = [];
  loading: boolean;
  clientId: string;
  constructor(
    private dialog: MatDialog,
    private apiDocCall: DocumentationCallAPIService,
    private app: AppService
  ) {}

  ngOnInit() {
    this.loadData();
  }

  newDocCall() {
    const dialogRef = this.dialog.open(DocumentationCallComponent, {
      data: {
        confirmationNumber: '000000000000',
        clientId: 1,
      },
    });

    dialogRef.afterClosed().subscribe((dialogResult) => {
      if (dialogResult) {
        console.log(dialogResult);
        this.dialog.open(DialogGenericSuccessComponent, {
          width: '250px',
          height: '250px',
          data: new GenericSucessModel(
            'Success',
            '<h4>' + dialogResult + '</h4>'
          ),
        });
      }
    });
  }

  loadData() {
    try {
      this.loading = true;
      this.apiDocCall.getClientDocCalls('1').subscribe(
        (data: any) => {
          //this.dataSource = new MatTableDataSource();
          this.threads = data;
          this.loading = false;
        },
        (error: any) => {
          this.loading = false;

          if (error.status != 401) {
            console.error('error', error);
            this.app.showErrorMessage('Error interno');
          }
        }
      );
    } catch (error) {
      this.loading = false;
    } finally {
    }
  }
}
