

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EffectTablesTableComponent } from './effecttable-table.component';

describe('#UserTableComponent', () => {
  let component: EffectTablesTableComponent;
  let fixture: ComponentFixture<EffectTablesTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EffectTablesTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EffectTablesTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

