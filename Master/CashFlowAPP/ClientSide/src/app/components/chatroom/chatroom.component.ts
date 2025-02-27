import { SignalrHubService } from './../../service/signalr-hub.service';
import { Component, OnInit } from '@angular/core';
import { GlobalToastService } from '../toast/global-toast.service';
import { FromClientChat } from 'src/app/models/FromClientChat';
import { HttpClient } from '@angular/common/http';
import { SharedService } from 'src/app/service/shared.service';


@Component({
  selector: 'app-chatroom',
  templateUrl: './chatroom.component.html',
  styleUrls: ['./chatroom.component.scss']
})
export class ChatroomComponent implements OnInit {

  constructor(public _ToastService: GlobalToastService, private http: HttpClient, public _Signalr: SignalrHubService,private _SharedService:SharedService) { }

  ngOnInit(): void {
    this.DistinguishUser();
    this.LoginToUserInfo();
  }

  ShowToast(Msg: string, CssClass: string, Header: string) {
    this._ToastService.show(Msg, {
      className: CssClass,
      delay: 10000,
      HeaderTxt: Header,
    });
  }

  UserToken: any = "";
  connection: any;
  Param = "";

  DistinguishUser() {
    this.Update();
    this.UpdSelfID();
    this.UpdContent();
  }

  // 更新連線 ID 列表事件
  IDList: string[] = [];
  NameList: string[] = [];
  Update() {
    this._Signalr.OnObservable("UpdList").subscribe((Res: any) => {
      let List = JSON.parse(Res[0])
      this.IDList = List;
      this.NameList = Res[1];
    });
  }

  SelfID = "";
  SelfObj: any;
  // 更新用戶個人連線 ID 事件
  UpdSelfID() {
    this._Signalr.OnObservable("UpdSelfID").subscribe((Res: any) => {
      this.SelfID = Res[0];
      this.SelfObj = Res[1];
    });
  }

  Content = "";
  // 更新聊天內容事件
  UpdContent() {
    this._Signalr.OnObservable("UpdContent").subscribe((Res: any) => {
      this.Content += `<li  class="list-group-item">${Res[0]}</li>`;
    });
  }

  FromClientChat = new FromClientChat()
  IsLogin: boolean = false;
  UserData: any = { Name: "" };
  // UserName: any;
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
  SendMsg() {
    let UserToken: any = localStorage.getItem("Token");
    this.FromClientChat.UserName = this.UserData.Name;
    this.FromClientChat.Token = UserToken;
    // invoke 去 call 後端 function 靠 名字
    this._Signalr.Invoke("SendMessage", this.FromClientChat);

    this.FromClientChat.message="";
  }
}
