import { ReadArgs } from './../../models/ReadAnswerQuestionArgs';
import { ApiRequest } from './../../models/ApiRequest';
import { SharedService } from './../../service/shared.service';
import { Component, DoCheck, OnInit } from '@angular/core';
import { GlobalToastService } from 'src/app/components/toast/global-toast.service';
import { ApiService } from 'src/app/service/api.service';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit {

  constructor(public _ToastService : GlobalToastService,private _Shared:SharedService,private _ApiService: ApiService) {}

  UserName:string="";
  ShowToast(Msg:string,CssClass:string,Header:string) {
    this._ToastService.show(Msg,{
      className: CssClass,
      delay: 10000,
      HeaderTxt:Header,
    });
  }
  StartGame(){
    if (this.UserName==""&&this.UserId==null){
      this.ShowToast("暱稱不得為空","bg-warning text-dark","提醒通知 From 錢董")
    }
    localStorage.setItem("StrangerName",this.UserName);
  }

  UserId = localStorage.getItem("UserId");
  UserData:any;
  UserDataName:string|any;
  ReadUserArg(){
    if(this.UserId!=null&&this.UserId!=""){
      this.IsLogin = true;
      let Req = new ApiRequest<any>();
      let listInt = [this.UserId];
      let Arg =
      {
        "Key": "Id",
        "JsonString": JSON.stringify(listInt)
      };
      Req.Args = [Arg];
      Req.PageIndex = 1;
      Req.PageSize = 15;
      this._ApiService.GetUserData(Req).subscribe((Res) => {
        console.log(Res);
        if (Res.Success) {
        this.UserData = Res.Data.Users[0];
        this.UserDataName = this.UserData.Name;
        console.log(this.UserData);
        }
      });
    }
  }

  IsLogin=false;
  ngOnInit(): void {
    this.ReadUserArg();
    // setTimeout(() => this.TypeWriter(), 1000);
  }
//   ngAfterViewInit(): void{}
//   // SloganText:string[]=[];
//   // Index=0;
//   // AnimatedText:string="";
//   // TypeWriter() {
//   //   this.SloganText="錢董讓你懂錢".split('');
//   //   console.log("type",this.SloganText.length);
//   //   const speed = 200;
//   //   if (this.Index < this.SloganText.length) {
//   //     this.AnimatedText += this.SloganText[this.Index];
//   //     this.Index++;
//   //     console.log("index",this.Index);
//   //     setTimeout(()=>this.TypeWriter(), speed);
//   //   }
//   //   else{
//   //     setTimeout(() => {
//   //       //注意這裡必須使用arrow function才能讓this被正確指向
//   //       this.Index = 0;
//   //       this.AnimatedText = "";
//   //       this.TypeWriter();
//   //     }, 5000);
//   //   }
//   // }
 }

