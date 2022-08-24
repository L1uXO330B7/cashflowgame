import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalToastService } from 'src/app/components/toast/global-toast.service';
import { ApiRequest } from 'src/app/models/ApiRequest';
import { Question } from 'src/app/models/Question';
import { ApiService } from 'src/app/service/api.service';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-survey-page',
  templateUrl: './survey-page.component.html',
  styleUrls: ['./survey-page.component.scss']
})
export class SurveyPageComponent implements OnInit {

  constructor(public _HttpClient: HttpClient, private _Router: Router, public _ApiService: ApiService,public _ToastService : GlobalToastService,private modalService: NgbModal) { }

  ngOnInit(): void {
    this.GetQuestions();
    this.open(this.modalDOM);
  }



  QuestionList:Question|any;
  GetQuestions(){
    let Req = new ApiRequest<any>();
   [Req.Args,Req.PageIndex,Req.PageSize]=
   [[],1,15];

    this._ApiService.GetQuestions(Req).subscribe((Res) => {
      if (Res.Success) {
        console.log(Res);
        this.QuestionList = Res.Data;
        this.QuestionList.forEach((Question:any) => {
          Question.Answer = Question.Answer.split(",");
        })
      }
      console.log(this.QuestionList,"QuestionList");
    });
  }


// modal
@ViewChild('content', { static: true }) modalDOM: any;
  closeResult = '';
  open(content:any) {
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }
}

