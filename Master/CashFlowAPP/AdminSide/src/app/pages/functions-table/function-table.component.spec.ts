

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FunctionsTableComponent } from './function-table.component';

describe('#UserTableComponent', () => {
  let component: FunctionsTableComponent;
  let fixture: ComponentFixture<FunctionsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FunctionsTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FunctionsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

