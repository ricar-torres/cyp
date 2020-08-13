import { TestBed } from '@angular/core/testing';

import { ClientWizardService } from './client-wizard.service';

describe('ClientWizardService', () => {
  let service: ClientWizardService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClientWizardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
