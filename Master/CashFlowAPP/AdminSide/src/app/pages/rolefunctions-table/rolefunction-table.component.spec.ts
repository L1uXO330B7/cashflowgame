

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoleFunctionsTableComponent } from './rolefunction-table.component';

describe('#UserTableComponent', () => {
  let component: RoleFunctionsTableComponent;
  let fixture: ComponentFixture<RoleFunctionsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RoleFunctionsTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoleFunctionsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

