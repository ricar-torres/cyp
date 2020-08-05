import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ClientService } from '@app/shared/client.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css'],
})
export class ClientComponent implements OnInit {
  clientid: string;
  reactiveForm: FormGroup;
  clientId: string;
  constructor(
    private fb: FormBuilder,
    private clientsService: ClientService,
    private route: ActivatedRoute
  ) {}

  onSubmit() {}

  async ngOnInit() {
    this.clientId = this.route.snapshot.paramMap.get('id');
    var client = this.clientsService.client(this.clientId);

    if (this.clientId) {
      this.reactiveForm = this.fb.group({
        Name: ['', [Validators.required]],
        LastName1: ['', [Validators.required]],
        LastName2: ['', [Validators.required]],
        Email: ['', [Validators.email]],
      });
    } else {
    }
    this.reactiveForm = this.fb.group({
      Name: ['', [Validators.required]],
      LastName1: ['', [Validators.required]],
      LastName2: ['', [Validators.required]],
      Email: ['', [Validators.email]],
    });
  }
}
