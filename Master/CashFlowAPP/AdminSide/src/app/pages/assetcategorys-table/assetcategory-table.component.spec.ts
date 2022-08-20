

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssetCategorysTableComponent } from './assetcategory-table.component';

describe('#UserTableComponent', () => {
  let component: AssetCategorysTableComponent;
  let fixture: ComponentFixture<AssetCategorysTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AssetCategorysTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssetCategorysTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

