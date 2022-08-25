import { AnswerQuestion } from './../../models/AnswerQuestion';
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


// 取出問卷
  QuestionList:Question|any;
  GetQuestions(){
    let Req = new ApiRequest<any>();
   [Req.Args,Req.PageIndex,Req.PageSize]=
   [[],1,15];

    this._ApiService.GetQuestions(Req).subscribe((Res) => {
      if (Res.Success) {
        this.QuestionList = Res.Data;
        this.QuestionList.forEach((question:any) => {
          question.Answer = question.Answer.split(",");
          // 新增答案置放欄位
          question.UserAnswer=[];
        })
      }
    });
  }

// 多選塞值
CheckboxChange(evt:Event|any){
  console.log(evt.target.checked,"tar");
  console.log(evt.target.value,"tar");
  console.log(evt.target.name,"tar");
  if(evt.target.checked){
    this.QuestionList.forEach((question:any) => {
      //塞到題目之中
      if(question.Name==evt.target.name){
        console.log("push");
        question.UserAnswer.push(evt.target.value);
      }
    })
  }
  else{
    this.QuestionList.forEach((question:any) => {
      //篩掉題目之中
      if(question.Name==evt.target.name){
        console.log("push");
        question.UserAnswer.splice(question.UserAnswer.indexOf(evt.target.value), 1);
      }
    })
  }
}


BeautifyData(){
  let UserAnswerArgs:any = [];
  let UserId = localStorage.getItem("UserId");
  console.log(this.QuestionList,"list");
  this.QuestionList.forEach((question:any) => {
    let UserAnswerArg = new AnswerQuestion();
    UserAnswerArg.UserId = UserId;
    UserAnswerArg.Answer = question.UserAnswer.join(',');
    UserAnswerArg.QusetionId = question.Id;
    UserAnswerArgs.push(UserAnswerArg);
  });
  console.log(UserAnswerArgs,"list");
  this.SaveUserAnswerArgs(UserAnswerArgs);
}
SaveUserAnswerArgs(UserAnswerArgs:any){

  let Req = new ApiRequest<any>();
  Req.Args = UserAnswerArgs;
  this._ApiService.SaveUserAnswerArgs(Req).subscribe((Res) => {
    if (Res.Success) {
      console.log(Res);
    }
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

