

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserBoardsTableComponent } from './userboard-table.component';

describe('#UserTableComponent', () => {
  let component: UserBoardsTableComponent;
  let fixture: ComponentFixture<UserBoardsTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserBoardsTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserBoardsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

