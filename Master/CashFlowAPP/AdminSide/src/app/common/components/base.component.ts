import { Injectable, OnInit } from '@angular/core';

@Injectable()
export class BaseComponent {

  pageIndex = 0;
  pageSize = 5;
  totalDataCount:any = 0;

  constructor(
  ) {
  }

  IsNullOrEmpty(value: any) {
    return (value === undefined || value == null || value === '');
  }
}
