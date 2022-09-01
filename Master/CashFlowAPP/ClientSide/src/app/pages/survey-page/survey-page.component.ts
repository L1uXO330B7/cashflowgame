import { Question } from './../../models/Question';
import { AnswerQuestion } from './../../models/AnswerQuestion';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, Type, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalToastService } from 'src/app/components/toast/global-toast.service';
import { ApiRequest } from 'src/app/models/ApiRequest';
import { ApiService } from 'src/app/service/api.service';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { ReadArgs } from 'src/app/models/ReadAnswerQuestionArgs';

@Component({
  selector: 'app-survey-page',
  templateUrl: './survey-page.component.html',
  styleUrls: ['./survey-page.component.scss']
})
export class SurveyPageComponent implements OnInit {

  constructor(public _HttpClient: HttpClient, private _Router: Router, public _ApiService: ApiService, public _ToastService: GlobalToastService, private modalService: NgbModal,) { }

  ngOnInit(): void {
    this.GetUserAnswer();
    this.open(this.modalDOM);
  }

  IsLoading=false;

  ShowToast(Msg:string,CssClass:string,Header:string) {
    this._ToastService.show(Msg,{
      className: CssClass,
      delay: 10000,
      HeaderTxt:Header,
    });
  }
  // 取出問卷
  QuestionList: Question | any;
  GetQuestions() {
    let Req = new ApiRequest<any>();
    [Req.Args, Req.PageIndex, Req.PageSize] =
      [[], 1, 15];
    // 要先拉用戶答案才可以取問卷資料比對
    this._ApiService.GetQuestions(Req).subscribe((Res) => {

      if (Res.Success) {
        this.QuestionList = Res.Data;
        this.QuestionList.forEach((question: any) => {
          question.Answer = question.Answer.split(",");
          // 新增答案置放欄位
          question.UserAnswer = [];
          if (this.UserId != null && this.UserId != "") {
            question.Answer.forEach((value: any, index: any, array: any) => {
              this.UserAnswerData.forEach((item: any) => {
                if (question.Id == item.QusetionId) {
                  if (item.Answer.indexOf(value) != -1) {
                    question.SaveAnswer = [];
                    if (question.Type !== 2) {
                      question.SaveAnswer[0] = value;
                      question.UserAnswer[0] = value;
                    }
                    else {
                      question.SaveAnswer = item.Answer.split(",");
                      question.UserAnswer = question.SaveAnswer;
                    }
                  }
                }
              })
            })
          }
        })
        this.IsLoading=false;
      }
    });
  }


  // 檢查
  CheckBoxForAnswer(QuestionAnswer: any, SaveAnswer: any) {
    if (SaveAnswer == [] || SaveAnswer == undefined) {
      return false;
    }
    else {
      if (SaveAnswer.indexOf(QuestionAnswer) != -1) {
        return true;
      }
      else {
        return false;
      }
    }
  }

  // 多選塞值
  CheckboxChange(evt: Event | any) {

    if (evt.target.checked) {
      this.QuestionList.forEach((question: any) => {
        //塞到題目之中
        if (question.Name == evt.target.name) {
          question.UserAnswer.push(evt.target.value);
        }
      })
    }
    else {
      this.QuestionList.forEach((question: any) => {
        //篩掉題目之中
        if (question.Name == evt.target.name) {
          question.UserAnswer.splice(question.UserAnswer.indexOf(evt.target.value), 1);
        }
      })
    }
  }

  // 將答案整理成回傳格式
  BeautifyData() {
    let UserAnswerArgs: any = [];
    let required = true;
    let UserId = localStorage.getItem("UserId");
    console.log(this.QuestionList, "list");
    this.QuestionList.forEach((question: any) => {
      let UserAnswerArg = new AnswerQuestion();
      UserAnswerArg.UserId = UserId;
      UserAnswerArg.Answer = question.UserAnswer.join(',');
      console.log(question.UserAnswer.length,"lenghth");
      console.log(question.UserAnswer,"useranswer");
      if(question.UserAnswer.length<=0){
        required = false;
      }
      UserAnswerArg.QusetionId = question.Id;
      UserAnswerArgs.push(UserAnswerArg);
    });
    if(required){
      this.SaveUserAnswerArgs(UserAnswerArgs);
    }
    else{
      this.ShowToast("所有欄位都為必填","bg-danger text-light","錢董提醒您")
    }
  }
  // call create api
  SaveUserAnswerArgs(UserAnswerArgs: any) {

    let Req = new ApiRequest<any>();
    Req.Args = UserAnswerArgs;
    this._ApiService.SaveUserAnswerArgs(Req).subscribe((Res) => {
      if (Res.Success) {
        this._Router.navigate(['/', 'game']);
      }
    });
  }
  UserAnswerData: any = [];
  // 將使用者答案讀出
  UserId = localStorage.getItem("UserId");

  GetUserAnswer() {
    this.IsLoading=true;
    if (this.UserId != "" && this.UserId != null) {
      let Req = new ApiRequest<any>();
      let listInt = [this.UserId];
      let Arg =
      {
        "Key": "UserId",
        "JsonString": JSON.stringify(listInt)
      };

      Req.Args = [Arg];
      Req.PageIndex = 1;
      Req.PageSize = 15;
      this._ApiService.GetUserAnswer(Req).subscribe((Res) => {
        if (Res.Success) {
          this.UserAnswerData = Array.from(Res.Data);
          this.GetQuestions();
        }
      });
    }
    else{
      this.GetQuestions();
    }
  }





  // modal
  @ViewChild('content', { static: true }) modalDOM: any;
  closeResult = '';
  open(content: any) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
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

