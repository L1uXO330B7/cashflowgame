

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CashFlowCategorysTableComponent } from './cashflowcategory-table.component';

describe('#UserTableComponent', () => {
  let component: CashFlowCategorysTableComponent;
  let fixture: ComponentFixture<CashFlowCategorysTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CashFlowCategorysTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CashFlowCategorysTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

