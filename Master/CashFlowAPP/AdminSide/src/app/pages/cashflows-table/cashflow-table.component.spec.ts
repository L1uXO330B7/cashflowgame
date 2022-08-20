

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CashFlowsTableComponent } from './cashflow-table.component';

describe('#UserTableComponent', () => {
  let component: CashFlowsTableComponent;
  let fixture: ComponentFixture<CashFlowsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CashFlowsTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CashFlowsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

