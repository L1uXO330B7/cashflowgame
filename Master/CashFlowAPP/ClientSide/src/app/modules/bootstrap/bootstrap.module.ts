import { NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    NgbToastModule
  ],
  exports: [
    NgbToastModule,
  ],
})
export class BootstrapModule { }
