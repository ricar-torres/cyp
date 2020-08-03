import { TestBed } from '@angular/core/testing';

import { BonafideService } from './bonafide.service';

describe('BonafideService', () => {
  let service: BonafideService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BonafideService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
