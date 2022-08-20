

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionsTableComponent } from './question-table.component';

describe('#UserTableComponent', () => {
  let component: QuestionsTableComponent;
  let fixture: ComponentFixture<QuestionsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QuestionsTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QuestionsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

