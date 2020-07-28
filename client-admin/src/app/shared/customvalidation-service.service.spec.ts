import { TestBed } from '@angular/core/testing';

import { CustomvalidationServiceService } from './customvalidation-service.service';

describe('CustomvalidationServiceService', () => {
  let service: CustomvalidationServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CustomvalidationServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
