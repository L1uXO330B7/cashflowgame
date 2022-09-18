import { ApiRequest } from '../../models/ApiRequest';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component,OnInit,ViewChild } from '@angular/core';
import { ClientUserLogin } from 'src/app/models/ClientUserLogin';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/service/api.service';
import { GlobalToastService } from 'src/app/components/toast/global-toast.service';
import { SharedService } from 'src/app/service/shared.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {

  constructor(
    public _HttpClient: HttpClient,
    private _Router: Router,
    public _ApiService: ApiService,
    public _ToastService : GlobalToastService,
    public _SharedService:SharedService
    ) {
  }

  ngOnInit(): void {
    setTimeout(() => {
      this.IsLoading=false;
    }, 2000);
  }
  IsLoading:boolean=true;
  change = false;
  windowWidth = window.innerWidth;
  distance = 0;
  Card = "";
  Info = "";

  CardMove() {
    if (this.windowWidth <= 968) {
      if (this.change) {
        this.Card = `translateY(10rem) !important`;
      } else {
        this.Card = `translateX(0em) !important`;
      }
    } else {
      if (this.change) {
        this.Card = `translateX(${this.distance}px) !important`;
      } else {
        this.Card = `translateX(0px) !important`;
      }
    }
  }

  InfoMove() {
    if (this.windowWidth <= 968) {
      if (this.change) {
        this.Info = `translateY(-20rem) !important`;
      } else {
        this.Info = `translateY(0em) !important`;
      }
    } else {
      if (this.change) {
        this.Info = `translateX(-${this.distance}px) !important`;
      } else {
        this.Info = `translateX(0em) !important`;
      }
    }
  }

  @ViewChild('bar', { static: true }) barDOM: any;
  @ViewChild('card', { static: true }) cardDOM: any;
  changeLogin() {
    this.change = !this.change;
    this.distance =
      this.barDOM.nativeElement.clientWidth - (this.cardDOM.nativeElement.clientWidth + 100);
    this.CardMove();
    this.InfoMove();
    this.VerificationCode={};
    if(this.change){
      this._ClientUserLogin={};
    }
  }

  _HttpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  _ClientUserLogin: any = {};
  UserLogin() {
    let Req = new ApiRequest<ClientUserLogin>();
    Req.Args = this._ClientUserLogin;
    this._ApiService.UserLogin(Req).subscribe((Res) => {
      if (Res.Success) {
        localStorage.setItem('Token', Res.Data.JwtCode);
        localStorage.setItem('UserId', Res.Data.UserId);
        localStorage.removeItem('StrangerName');
        this.UserId = localStorage.getItem("UserId");
        this.ReadUserArg();
        this._Router.navigate(['/', 'survey']);
      }
    });
  }

  VerificationCode: any = {};
  GetVerificationCode() {
    this._ApiService.GetVerificationCode()
      .subscribe((Res) => {
        this.VerificationCode = Res.Data;
      });
  }
  ShowToast(Msg: string, CssClass: string, Header: string) {
    this._ToastService.show(Msg, {
      className: CssClass,
      delay: 10000,
      HeaderTxt: Header,
    });
  }
  UserSignUp() {
    this.IsLoading=true;
    if (this._ClientUserLogin.CheckPassword != this._ClientUserLogin.Password) {
      this.ShowToast("兩次密碼不相同","註冊通知","bg-danger")
      this.IsLoading=false;
      return
    }
    let Req = new ApiRequest<any>();
    this._ClientUserLogin.JwtCode = this.VerificationCode.JwtCode;
    Req.Args = this._ClientUserLogin;
    // this._HttpClient.post<ApiResponse>(ApiUrl, Req, this._HttpOptions)  改為使用 Service
    this._ApiService.UserSignUp(Req).subscribe((Res) => {
        if(Res.Success){
          this.changeLogin();
          this.IsLoading=false;
        }
        else{
          this.IsLoading=false;
        }
      });
  }
  // test() {
  //   let ApiUrl = `http://localhost:46108/api/System/GetMiniProfilerScript`;

  //   // https://stackoverflow.com/questions/62046090/angular-unexpected-token-c-in-json-at-position-0-at-json-parse-when-expecting-a
  //   this._HttpClient.get(ApiUrl, { responseType: 'text' })
  //     .subscribe((Res) => {
  //     });
  // }

  IsLogin:boolean=false;
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
        if (Res.Success) {
        this.UserData = Res.Data.Users[0];
        this.UserDataName = this.UserData.Name;
        this._SharedService.SetShareData(this.UserData);
        }
      });
    }
    else{
      let  StrangerName:any = localStorage.getItem("StrangerName");
      this._SharedService.SetShareData(StrangerName);
    }
  }
  ngOnDestroy(): void {
    this._ToastService.clear();
  }
}


