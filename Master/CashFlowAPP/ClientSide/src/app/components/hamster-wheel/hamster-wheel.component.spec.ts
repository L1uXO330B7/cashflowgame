import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HamsterWheelComponent } from './hamster-wheel.component';

describe('HamsterWheelComponent', () => {
  let component: HamsterWheelComponent;
  let fixture: ComponentFixture<HamsterWheelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HamsterWheelComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HamsterWheelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
