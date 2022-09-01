

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnswerQuestionsTableComponent } from './answerquestion-table.component';

describe('#UserTableComponent', () => {
  let component: AnswerQuestionsTableComponent;
  let fixture: ComponentFixture<AnswerQuestionsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AnswerQuestionsTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AnswerQuestionsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

