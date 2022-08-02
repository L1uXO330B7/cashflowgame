import { TestBed } from '@angular/core/testing';

import { GlobalToastService } from './global-toast.service';

describe('GlobalToastService', () => {
  let service: GlobalToastService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GlobalToastService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
