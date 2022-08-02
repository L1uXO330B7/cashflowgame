import { SafeHtmlPipe } from './../../pipes/safe-html.pipe';

import { Component, OnInit, TemplateRef } from '@angular/core';
import { GlobalToastService } from './global-toast.service';


@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.scss']
})
export class ToastComponent implements OnInit {

  constructor(public _ToastService : GlobalToastService) { }
  ngOnInit(): void {
  }
  show = true;
  autohide = false;
  animation = true;
  isTemplate(toast:any) {
    console.log(toast.textOrTpl instanceof TemplateRef);
    return toast.textOrTpl instanceof TemplateRef;}

}
