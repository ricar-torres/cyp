import { TestBed } from '@angular/core/testing';

import { QualifyingEventService } from './qualifying-event.service';

describe('QualifyingEventService', () => {
  let service: QualifyingEventService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(QualifyingEventService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
