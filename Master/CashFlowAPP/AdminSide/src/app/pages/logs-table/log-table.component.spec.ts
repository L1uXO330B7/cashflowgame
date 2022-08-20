

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LogsTableComponent } from './log-table.component';

describe('#UserTableComponent', () => {
  let component: LogsTableComponent;
  let fixture: ComponentFixture<LogsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LogsTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LogsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

