import { TestBed } from '@angular/core/testing';

import { AlliancesService } from './alliances.service';

describe('AlliancesService', () => {
  let service: AlliancesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AlliancesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
