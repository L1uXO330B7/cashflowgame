import { SignalrHubService } from './../../service/signalr-hub.service';
import { Component, OnInit } from '@angular/core';
import { GlobalToastService } from '../toast/global-toast.service';
import * as signalR from "@microsoft/signalr";
import { FromClientChat } from 'src/app/models/FromClientChat';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-chatroom',
  templateUrl: './chatroom.component.html',
  styleUrls: ['./chatroom.component.scss']
})
export class ChatroomComponent implements OnInit {

  constructor(public _ToastService : GlobalToastService,private http:HttpClient,public _Signalr:SignalrHubService) { }

  ngOnInit(): void {
    this.DistinguishUser();
  }
  ShowToast(Msg:string,CssClass:string,Header:string) {
    this._ToastService.show(Msg,{
      className: CssClass,
      delay: 10000,
      HeaderTxt:Header,
    });
  }
  UserToken: any="";
  connection:any;
  Param = "";
  DistinguishUser(){
    this.UserToken = localStorage.getItem("Token");
    this.Param = `token=${this.UserToken}`
    console.log("this.UserToken",this.UserToken);
    if(this.UserToken==null||undefined||""){
      this.UserToken = localStorage.getItem("StrangerName");
      console.log("this.UserToken",this.UserToken);
      this.Param = `stranger=${this.UserToken}`
    }
    this._Signalr.Connect(`${this.Param}`);

    this.Update();
    this.UpdSelfID();
    this.UpdContent();
  }


  // 更新連線 ID 列表事件
  IDList: string[] = [];
  NameList: string[]=[];
  Update() {
    this._Signalr.OnObservable("UpdList").subscribe((Res:any)=>{
      let List = JSON.parse(Res[0])
      this.IDList = List;
      this.NameList = Res[1];
    });
  }
  SelfID = "";
  SelfObj:any;
  // 更新用戶個人連線 ID 事件
  UpdSelfID() {
    this._Signalr.OnObservable("UpdSelfID").subscribe((Res:any)=>{
      this.SelfID = Res[0];
      this.SelfObj = Res[1];
    });
  }

  Content = "";
  // 更新聊天內容事件
  UpdContent() {
    this._Signalr.OnObservable("UpdContent").subscribe((Res:any)=>{
      this.Content+=`<li  class="list-group-item">${Res[0]}</li>`;
    });
  }
draw(){
  console.log(55);
  setTimeout(() => {
    this._Signalr.Invoke("SetTimer", 5);
  this._Signalr.OnObservable("Okay").subscribe((Res:any)=>{
    console.log(Res,"timer");
  });
  }, 20000);

}
  FromClientChat = new FromClientChat()
  SendMsg() {
    let selfID:any = document.querySelector('#SelfID')?.innerHTML;
    this.FromClientChat.selfID = selfID;
    let UserToken: any = localStorage.getItem("Token");
    this.FromClientChat.Token = UserToken;
    // invoke 去 call 後端 function 靠 名字
    this._Signalr.Invoke("SendMessage", this.FromClientChat);
  }
}
