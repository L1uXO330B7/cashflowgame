import { TestBed } from '@angular/core/testing';

import { SignalrHubService } from './signalr-hub.service';

describe('SignalrHubService', () => {
  let service: SignalrHubService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SignalrHubService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
