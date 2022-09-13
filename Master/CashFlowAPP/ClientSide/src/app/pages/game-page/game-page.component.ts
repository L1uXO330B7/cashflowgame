import { SharedService } from './../../service/shared.service';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalToastService } from 'src/app/components/toast/global-toast.service';
import { ApiRequest } from 'src/app/models/ApiRequest';
import { ApiService } from 'src/app/service/api.service';
import { SignalrHubService } from 'src/app/service/signalr-hub.service';


@Component({
  selector: 'app-game-page',
  templateUrl: './game-page.component.html',
  styleUrls: ['./game-page.component.scss']
})
export class GamePageComponent implements OnInit {

  constructor(
    public _HttpClient: HttpClient,
    private _Router: Router,
    public _ApiService: ApiService,
    public _ToastService: GlobalToastService,
    private _SharedService: SharedService,
    public _Signalr: SignalrHubService
  ) {

  }

  ngOnInit(): void {
    this.GetFiInfo();
    this.LoginToUserInfo();
    this.InitServer();
    this.RoundStart();
  }

  Flag = true;
  RoundStart() {
    let Round = setInterval(() => {
      this.Time = new Date();
      let Second = this.Time.getSeconds();
      if (Second % 60 == 0) {
        this.Flag = false;
        setInterval(() => { this.GameTimeRound(); }, 60000);
        clearInterval(Round);
      }
    }, 1000)

  }

  UserToken: any;
  Param: any;
  InitServer() {
    this.UserToken = localStorage.getItem("Token");
    this.Param = `token=${this.UserToken}`
    if (this.UserToken == null || undefined || "") {
      this.UserToken = localStorage.getItem("StrangerName");
      this.Param = `stranger=${this.UserToken}`
    }
    this._Signalr.Connect(`${this.Param}`);
  }

  OpenChatRoom: boolean = false;
  ToggleChatRoom() {
    this.OpenChatRoom = !this.OpenChatRoom;
  }

  OpenInfo: boolean = true;
  ToggleInfo() {
    this.OpenInfo = !this.OpenInfo;
  }

  OpenCard: boolean = true;
  ToggleCard() {
    this.OpenCard = !this.OpenCard;
  }
  IsLogin: boolean = false;
  UserData: any;
  UserName: any;
  UserId = localStorage.getItem('UserId');
  LoginToUserInfo() {

    if (this.UserId != "" && this.UserId != null) {
      this._SharedService.SharedData.subscribe((Res) => {
        this.UserData = Res;
      })
      this.IsLogin = true;
    }
    else {
      this.UserData.Name = localStorage.getItem("StrangerName");
    }
  }

  UserFiInfo: any = {}
  GetFiInfo() {
    let Req = new ApiRequest<any>();
    Req.Args = this.UserId;
    this._ApiService.GetFiInfo(Req).subscribe((Res) => {
      if (Res.Success) {
        this.UserFiInfo = Res.Data;
      }
    });
  }


  Time = new Date();
  JoinRound = (this.Time.getHours() * 60) + this.Time.getMinutes();
  CurrentRound = (this.Time.getHours() * 60) + this.Time.getMinutes();
  GameTimeRound() {
    this.Time = new Date();
    this.CurrentRound = (this.Time.getHours() * 60) + this.Time.getMinutes();
    this.ToggleCard();
    setTimeout(() => {
      this.ToggleCard();
    }, 1800);
  }
}
