import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '@app/shared/app.service';
import { bonaFideservice } from '@app/shared/bonafide.service';
import { merge, fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
@Component({
  selector: 'app-bona-fide',
  templateUrl: './bona-fide.component.html',
  styleUrls: ['./bona-fide.component.css'],
})
export class BonaFideComponent implements OnInit {
  reactiveForm: FormGroup;
  id: string;
  loading = false;

  bonafide: string;
  NameExists: boolean = false;
  EmailExists: boolean = false;
  @ViewChild('inputEmail', { static: true }) inputEmail: ElementRef;
  @ViewChild('inputName', { static: true }) inputName: ElementRef;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private bonafideService: bonaFideservice,
    private app: AppService
  ) {}

  async ngOnInit() {
    this.loading = true;
    this.id = this.route.snapshot.paramMap.get('id');

    if (this.id) {
      this.reactiveForm = this.fb.group({
        Id: [this.id],
        Name: ['', [Validators.required]],
        Code: [''],
        Siglas: [''],
        Phone: [''],
        Email: ['', [Validators.email]],
        Benefits: [''],
        Disclaimer: [''],
      });
      var editBonafide: any = await this.bonafideService.bonafide(this.id);
      this.bonafide = editBonafide.name;
      this.reactiveForm.get('Id').setValue(editBonafide.id);
      this.reactiveForm.get('Name').setValue(editBonafide.name);
      this.reactiveForm.get('Code').setValue(editBonafide.code);
      this.reactiveForm.get('Siglas').setValue(editBonafide.siglas);
      this.reactiveForm.get('Phone').setValue(editBonafide.phone);
      this.reactiveForm.get('Email').setValue(editBonafide.email);
      this.reactiveForm.get('Benefits').setValue(editBonafide.benefits);
      this.reactiveForm.get('Disclaimer').setValue(editBonafide.disclaimer);
    } else {
      this.reactiveForm = this.fb.group({
        Name: ['', [Validators.required]],
        Code: [''],
        Siglas: [''],
        Phone: [''],
        Email: ['', [Validators.email]],
        Benefits: [''],
        Disclaimer: [''],
      });
      this.setUpSubscriptions();
    }
    this.loading = false;
  }
  setUpSubscriptions() {
    merge(fromEvent(this.inputName.nativeElement, 'keydown'))
      .pipe(
        debounceTime(150),
        distinctUntilChanged(),
        tap(async () => {
          await !this.checkName(this.inputName.nativeElement.value);
        })
      )
      .subscribe();

    merge(fromEvent(this.inputEmail.nativeElement, 'keydown'))
      .pipe(
        debounceTime(150),
        distinctUntilChanged(),
        tap(async () => {
          await !this.checkEmail(this.inputEmail.nativeElement.value);
        })
      )
      .subscribe();
  }

  onBack() {
    this.router.navigate(['home/bonafides']);
  }

  async onSubmit() {
    try {
      this.loading = true;
      if (this.id) {
        await this.bonafideService.update(this.reactiveForm.value);
        this.onBack();
      } else {
        await this.bonafideService.create(this.reactiveForm.value);
        this.onBack();
      }
    } catch (error) {
      this.loading = false;
      if (error.status != 401) {
        console.error('error', error);
        this.app.showErrorMessage('Error');
      }
    } finally {
      this.loading = false;
    }
  }

  async checkName(name: string) {
    try {
      if (name) {
        const res: any = await this.bonafideService.checkName({
          name: name,
        });
        this.NameExists = res;
      }
    } catch (error) {
      this.NameExists = false;
    }
  }

  async checkEmail(email: string) {
    try {
      if (email) {
        const res: any = await this.bonafideService.checkEmail({
          name: email,
        });
        this.EmailExists = res;
      }
    } catch (error) {
      this.EmailExists = false;
    }
  }
}
