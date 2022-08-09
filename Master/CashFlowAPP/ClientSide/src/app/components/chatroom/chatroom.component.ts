import { Component, OnInit } from '@angular/core';
import { GlobalToastService } from '../toast/global-toast.service';
import * as signalR from "@microsoft/signalr";
import { FromClientChat } from 'src/app/models/FromClientChat';


@Component({
  selector: 'app-chatroom',
  templateUrl: './chatroom.component.html',
  styleUrls: ['./chatroom.component.scss']
})
export class ChatroomComponent implements OnInit {

  constructor(public _ToastService : GlobalToastService) { }

  ngOnInit(): void {

    this.DistinguishUser()
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
  DistinguishUser(){
    let Param = "";
    this.UserToken = localStorage.getItem("Token");
    Param = `token=${this.UserToken}`
    console.log("this.UserToken",this.UserToken);
    if(this.UserToken==null||undefined||""){
      this.UserToken = localStorage.getItem("StrangerName");
      console.log("this.UserToken",this.UserToken);
      Param = `stranger=${this.UserToken}`
    }

    this.connection= new signalR.HubConnectionBuilder()
    .withUrl(`http://localhost:46108/chatHub?${Param}`)
    .withAutomaticReconnect()
    .build();
    this.connect();
    this.update();
    this.UpdSelfID();
    this.UpdContent();
  }


  //與Server建立連線
  connect() {
    this.connection.start().then(()=> {
      console.log("Hub 連線完成");
      this.ShowToast("成功進入",'bg-success text-light','成功通知 From 錢董')
    }).catch((err: any)=>{
      this.ShowToast(err.toString(),'bg-danger text-light','失敗通知 From 錢董')
    });
  }

  // 更新連線 ID 列表事件
  IDList: string = "";
  update() {
    this.connection.on("UpdList", (jsonList: any,UserList:any) => {

      var list = JSON.parse(jsonList);
      console.log("UserList",UserList);
      console.log("UserList",jsonList)
      // $("#IDList li").remove();
      this.IDList = "";
      list.forEach((value: any, index: number, array: any) => {
        this.IDList += `<li class='list-group-item'>${UserList[index]}</li>`;
      })
    });
  }
  SelfID = "";
  // 更新用戶個人連線 ID 事件
  UpdSelfID() {
    this.connection.on("UpdSelfID", (ConnectId: any,SelfObj:any) => {
      // $('#SelfID').html(id);
      console.log('SelfName',SelfObj);
      this.SelfID = SelfObj.name;
    });
  }

  Content = "";
  // 更新聊天內容事件
  UpdContent() {
    console.log("UpdContent1");
    this.connection.on("UpdContent", (msg: any) => {
      this.Content += `
      <li class="list-group-item">${msg}</li>
      `
      console.log("UpdContent2", this.Content);
      // $("#Content").append($("<li></li>").attr("class", "list-group-item").text(msg));
    });
  }

  FromClientChat = new FromClientChat()
  SendMsg() {
    console.log("SendMsg", this.FromClientChat.message);
    let selfID:any = document.querySelector('#SelfID')?.innerHTML;
    this.FromClientChat.selfID = selfID;
    let UserToken: any = localStorage.getItem("Token");
    this.FromClientChat.Token = UserToken;
    // invoke 去 call 後端 function 靠 名字
    this.connection.invoke("SendMessage", this.FromClientChat).catch(function (err: any) {
      alert('傳送錯誤: ' + err.toString());
    });
  }
}
