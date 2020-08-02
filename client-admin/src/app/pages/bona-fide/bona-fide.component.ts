import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
} from '@angular/forms';
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
      var editBonafide: any = await this.bonafideService.bonafide(this.id);
      this.bonafide = editBonafide.name;
      this.reactiveForm = this.fb.group({
        Id: [editBonafide.id],
        Name: [editBonafide.name, [Validators.required]],
        Code: [editBonafide.code],
        Siglas: [editBonafide.siglas],
        Phone: [editBonafide.phone],
        Email: [editBonafide.email, [Validators.email]],
        Benefits: [editBonafide.benefits],
        Disclaimer: [editBonafide.disclaimer],
      });
    } else {
      this.reactiveForm = this.fb.group({
        Name: ['', [Validators.required], this.checkName.bind(this)],
        Code: [''],
        Siglas: [''],
        Phone: [''],
        Email: ['', [Validators.email], this.checkEmail.bind(this)],
        Benefits: [''],
        Disclaimer: [''],
      });
    }
    this.loading = false;
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

  async checkName(name: FormControl) {
    if (name.value) {
      const res: any = await this.bonafideService.checkName({
        name: name.value,
      });
      if (res) return { nameTaken: true };
    }
  }

  async checkEmail(email: FormControl) {
    if (email.value) {
      const res: any = await this.bonafideService.checkEmail({
        name: email.value,
      });
      if (res) return { emailTaken: true };
    }
  }
}
