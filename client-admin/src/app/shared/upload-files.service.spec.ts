import { TestBed } from '@angular/core/testing';

import{UploadfilesService} from './upload-files.service';
import { from } from 'rxjs';

describe('UploadfilesService', () => {
  let service: UploadfilesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UploadfilesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
