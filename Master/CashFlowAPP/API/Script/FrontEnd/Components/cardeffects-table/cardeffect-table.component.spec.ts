

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardEffectsTableComponent } from './cardeffect-table.component';

describe('#UserTableComponent', () => {
  let component: CardEffectsTableComponent;
  let fixture: ComponentFixture<CardEffectsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CardEffectsTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardEffectsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

