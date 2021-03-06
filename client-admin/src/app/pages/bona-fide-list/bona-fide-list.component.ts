import {
  Component,
  OnInit,
  AfterViewInit,
  ViewChild,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';
import {
  PageEvent,
  MatSort,
  MatPaginator,
  MatTableDataSource,
  MatDialog,
} from '@angular/material';
import { AppService } from '@app/shared/app.service';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { bonaFideservice } from '@app/shared/bonafide.service';
import { MenuRoles } from '@app/models/enums';
import { LanguageService } from '@app/shared/Language.service';
import {
  ConfirmDialogModel,
  ConfirmDialogComponent,
} from '@app/components/confirm-dialog/confirm-dialog.component';
import { BonafidesAssociatorComponent } from '../bonafides-associator/bonafides-associator.component';
import { ChapterServiceService } from '@app/shared/chapter-service.service';
import { ClientWizardService } from '@app/shared/client-wizard.service';

@Component({
  selector: 'app-bona-fide-list',
  templateUrl: './bona-fide-list.component.html',
  styleUrls: ['./bona-fide-list.component.css'],
})
export class BonaFideListComponent implements OnInit {
  editAccess: boolean = false;
  createAccess: boolean = false;
  deleteAccess: boolean = false;
  deceased: boolean = false;
  dataSource;
  displayedColumns: string[] = [
    'id',
    'name',
    'code',
    'siglas',
    'phone',
    'createdAt',
    'updatedAt',
    'actions',
  ];
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 25, 100];
  pageEvent: PageEvent;
  loading = true;
  availableBonafides: [] = [];
  @Output() isLoadingEvent = new EventEmitter<boolean>();
  @Input() clientId: string;
  @Input() fromWizard: string;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(
    private app: AppService,
    private fb: FormBuilder,
    private bonafidesService: bonaFideservice,
    private chapterService: ChapterServiceService,
    private router: Router,
    private languageService: LanguageService,
    private dialog: MatDialog,
    private clietnWizard: ClientWizardService
  ) {}

  ngOnInit(): void {
    //TODO: Acces
    // this.editAccess = this.app.checkMenuRoleAccess(MenuRoles.BONAFIDE_UPDATE);
    // this.createAccess = this.app.checkMenuRoleAccess(MenuRoles.BONAFIDE_CREATE);
    // this.deleteAccess = this.app.checkMenuRoleAccess(MenuRoles.BONAFIDE_DELETE);
    this.editAccess = true;
    this.createAccess = true;
    this.deleteAccess = true;
    this.loadBonafides();
  }
  private loadBonafides() {
    this.loading = true;
    this.isLoadingEvent.emit(this.loading);
    if (this.fromWizard) {
      this.loading = true;
      this.dataSource = new MatTableDataSource();
      this.dataSource.data = this.clietnWizard.BonafideList;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.loading = false;
      this.isLoadingEvent.emit(this.loading);
    } else {
      this.loading = true;
      this.bonafidesService.getAll(this.clientId).subscribe(
        (res) => {
          this.loading = true;
          this.isLoadingEvent.emit(this.loading);
          this.dataSource = new MatTableDataSource();
          this.dataSource.data = res;
          this.availableBonafides = res;
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
          this.loading = false;
          this.isLoadingEvent.emit(this.loading);
        },
        (error) => {
          this.loading = false;
          this.isLoadingEvent.emit(this.loading);
          // console.log('error', error);
          this.languageService.translate
            .get('GENERIC_ERROR')
            .subscribe((res) => {
              this.app.showErrorMessage(res);
            });
        },
        () => {
          this.loading = false;
          this.isLoadingEvent.emit(this.loading);
        }
      );
    }
  }

  onBack() {}

  setPageSizeOptions(setPageSizeOptionsInput: string) {
    if (setPageSizeOptionsInput) {
      this.pageSizeOptions = setPageSizeOptionsInput
        .split(',')
        .map((str) => +str);
    }
  }

  async deleteConfirm(id: string) {
    try {
      const message = await this.languageService.translate
        .get('BONAFIDE.ARE_YOU_SURE_DELETE')
        .toPromise();

      const title = await this.languageService.translate
        .get('COMFIRMATION')
        .toPromise();

      const dialogData = new ConfirmDialogModel(
        title,
        message,
        true,
        true,
        false
      );

      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        maxWidth: '400px',
        data: dialogData,
      });

      dialogRef.afterClosed().subscribe(async (dialogResult) => {
        if (dialogResult) {
          //if from the wizard delete from the local list
          if (this.fromWizard) {
            var index = this.clietnWizard.BonafideList.findIndex(
              (x) => x.id == id
            );
            this.clietnWizard.BonafideList.splice(index, 1);
            this.dataSource.data = this.clietnWizard.BonafideList;
          }
          //if from the client, delete the client_chapter association
          else if (this.clientId) {
            await this.chapterService.deleteChapterClient(id, this.clientId);
            this.loadBonafides();
          }
          // if form bonafides list, delete the bonafides entirely
          else {
            await this.bonafidesService.delete(id);
            this.loadBonafides();
          }
        }
      });
    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
        console.error('error', error);
        this.languageService.translate.get('GENERIC_ERROR').subscribe((res) => {
          this.app.showErrorMessage(res);
        });
      }
    } finally {
      this.loading = false;
    }
  }

  editBonafide(id: number) {
    if (this.clientId) {
      var bonafides = this.availableBonafides.find((bn) => (<any>bn).id == id);
      const dialogRef = this.dialog.open(BonafidesAssociatorComponent, {
        width: '70%',
        height: '45%',
        disableClose: true,
        data: { clientId: this.clientId, bonafide: bonafides },
      });

      dialogRef.afterClosed().subscribe((result) => {
        this.loadBonafides();
      });
    } else if (this.fromWizard) {
      const dialogRef = this.dialog.open(BonafidesAssociatorComponent, {
        width: '70%',
        height: '45%',
        disableClose: true,
        data: { clientId: this.clientId, listItem: id, fromWizard: true },
      });

      dialogRef.afterClosed().subscribe((result) => {
        this.loadBonafides();
      });
    } else {
      this.router.navigate(['/home/bonafide', id]);
    }
  }

  public goToNew() {
    if (this.clientId) {
      const dialogRef = this.dialog.open(BonafidesAssociatorComponent, {
        width: '70%',
        height: '45%',
        disableClose: true,
        data: { clientId: this.clientId, bonafideId: null },
      });
      dialogRef.afterClosed().subscribe((result) => {
        this.loadBonafides();
      });
    } else if (this.fromWizard) {
      const pepe = this.dialog.open(BonafidesAssociatorComponent, {
        width: '70%',
        height: '45%',
        disableClose: true,
        data: { clientId: null, bonafideId: null, fromWizard: true },
      });
      pepe.afterClosed().subscribe((result) => {
        this.loadBonafides();
      });
    } else {
      this.router.navigate(['/home/bonafide']);
    }
  }

  doFilter(value: any) {
    this.dataSource.filter = value.toString().trim().toLocaleLowerCase();
  }
}
