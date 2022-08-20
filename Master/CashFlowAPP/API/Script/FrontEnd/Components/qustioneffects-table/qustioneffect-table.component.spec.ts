

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QustionEffectsTableComponent } from './qustioneffect-table.component';

describe('#UserTableComponent', () => {
  let component: QustionEffectsTableComponent;
  let fixture: ComponentFixture<QustionEffectsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QustionEffectsTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QustionEffectsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

