

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssetsTableComponent } from './asset-table.component';

describe('#UserTableComponent', () => {
  let component: AssetsTableComponent;
  let fixture: ComponentFixture<AssetsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AssetsTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssetsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

