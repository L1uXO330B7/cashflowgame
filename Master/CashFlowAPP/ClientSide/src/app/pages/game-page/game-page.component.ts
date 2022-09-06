import { SharedService } from './../../service/shared.service';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalToastService } from 'src/app/components/toast/global-toast.service';
import { ApiRequest } from 'src/app/models/ApiRequest';
import { ApiService } from 'src/app/service/api.service';


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
    private _SharedService: SharedService) {

  }

  ngOnInit(): void {
    this.GetFiInfo();
    this.LoginToUserInfo();
    setInterval(()=>this.GameTimeRound(), 60000);
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
    console.log("clickCard", this.OpenCard)
    this.OpenCard = !this.OpenCard;
  }
  IsLogin: boolean = false;
  UserData: any;
  UserName: any;
  UserId = localStorage.getItem('UserId');
  LoginToUserInfo() {
    this._SharedService.SharedData.subscribe((Res) => {
      this.UserData = Res;;
      console.log(this.UserData, "a");
    })
    if (this.UserId != "" && this.UserId != null) {

      this.IsLogin = true;
    }
  }
  UserFiInfo: any = {}

  GetFiInfo() {
    let Req = new ApiRequest<any>();
    Req.Args = this.UserId;
    this._ApiService.GetFiInfo(Req).subscribe((Res) => {
      if (Res.Success) {
        this.UserFiInfo = Res.Data.Result;

        console.log(this.UserFiInfo, "Res");
      }
    });
  }


  Time = new Date();
  JoinRound =(this.Time.getHours()*60)+this.Time.getMinutes();
  CurrentRound=(this.Time.getHours()*60)+this.Time.getMinutes();
  GameTimeRound(){
    this.Time=new Date();
    this.CurrentRound = (this.Time.getHours()*60)+this.Time.getMinutes();
  }
}
