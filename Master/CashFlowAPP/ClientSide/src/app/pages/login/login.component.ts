import { ApiRequest } from './../../models/ApiRequest';
import { ApiResponse } from './../../models/ApiResponse';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ClientUserLogin } from 'src/app/models/ClientUserLogin';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/service/api.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(public _HttpClient: HttpClient, private _Router: Router, public _ApiService: ApiService) {
  }

  ngOnInit(): void {
  }
  change = false;
  windowWidth = window.innerWidth;
  distance = 0;
  Card = "";
  Info = "";

  CardMove() {
    console.log("windowWidth", this.windowWidth);
    console.log("change", this.change);
    if (this.windowWidth <= 968) {
      if (this.change) {
        this.Card = `translateY(10rem) !important`;
      } else {
        this.Card = `translateX(0em) !important`;
      }
    } else {
      if (this.change) {
        this.Card = `translateX(${this.distance}px) !important`;
        console.log(this.Card);
      } else {
        this.Card = `translateX(0px) !important`;
      }
    }
  }
  InfoMove() {
    console.log("barDOM", this.barDOM);
    console.log("distance", this.distance);
    console.log("InfoWidth", this.windowWidth);
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
    console.log("changeLogin", this.barDOM.nativeElement.clientWidth);
    this.CardMove();
    this.InfoMove();
  }

  _HttpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  _ClientUserLogin: any = {};
  UserLogin() {
    let Req = new ApiRequest<ClientUserLogin>();
    Req.Args = this._ClientUserLogin;
    this._ApiService.UserLogin(Req).subscribe((Res) => { console.log(Res);
      if(Res.Success){
        localStorage.setItem('token',Res.Data);
      }
    });
    }

  VerificationCode: any = {};
  GetVerificationCode() {
    this._ApiService.GetVerificationCode()
      .subscribe((Res) => {
        console.log(Res);
        this.VerificationCode = Res.Data;
      });
  }

  UserSignUp() {
    if (this._ClientUserLogin.CheckPassword != this._ClientUserLogin.Password) {
      alert("請確認密碼");
      return
    }
    let ApiUrl = `${environment.ApiRoot}/ClientSide/UserSignUp`;
    let Req = new ApiRequest<any>();
    this._ClientUserLogin.JwtCode = this.VerificationCode.JwtCode;
    Req.Args = this._ClientUserLogin;
    // this._HttpClient.post<ApiResponse>(ApiUrl, Req, this._HttpOptions)  改為使用 Service
    this._ApiService.UserSignUp(Req)
      .subscribe((Res) => {
        console.log(Res);
        alert("okay");
        this.CardMove();
      });
  }
  test(){
    let ApiUrl = `http://localhost:46108/api/System/GetMiniProfilerScript`;
        this._HttpClient.get(ApiUrl, this._HttpOptions)
        .subscribe((Res) => {
          console.log(Res);
          alert("okay");
        });
  }
}


