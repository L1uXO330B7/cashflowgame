import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalToastService } from 'src/app/components/toast/global-toast.service';
import { ApiRequest } from 'src/app/models/ApiRequest';
import { Question } from 'src/app/models/Question';
import { ApiService } from 'src/app/service/api.service';

@Component({
  selector: 'app-survey-page',
  templateUrl: './survey-page.component.html',
  styleUrls: ['./survey-page.component.scss']
})
export class SurveyPageComponent implements OnInit {

  constructor(public _HttpClient: HttpClient, private _Router: Router, public _ApiService: ApiService,public _ToastService : GlobalToastService) { }

  ngOnInit(): void {
    this.GetQuestions();
  }



  QuestionList:Question|any;
  GetQuestions(){
    let Req = new ApiRequest<any>();
   [Req.Args,Req.PageIndex,Req.PageSize]=
   [[],1,15];

    this._ApiService.GetQuestions(Req).subscribe((Res) => {
      if (Res.Success) {
        console.log(Res);
        Res.Data.forEach((value:any) => {
          this.QuestionList = Res.Data;
          console.log(JSON.parse(value.Answer));
        })
      }
    });
  }
}
