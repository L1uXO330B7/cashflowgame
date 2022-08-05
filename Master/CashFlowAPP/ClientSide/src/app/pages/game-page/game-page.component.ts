import { Component, OnInit } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { FromClientChat } from 'src/app/models/FromClientChat';

@Component({
  selector: 'app-game-page',
  templateUrl: './game-page.component.html',
  styleUrls: ['./game-page.component.scss']
})
export class GamePageComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
    this.connect();
    this.update();
    this.UpdSelfID();
    this.UpdContent();
  }

  UserToken: any = localStorage.getItem("Token")
  connection = new signalR.HubConnectionBuilder()
    .withUrl(`http://localhost:46108/chatHub?token=${this.UserToken}`)
    .withAutomaticReconnect()
    .build();

  //與Server建立連線
  connect() {
    this.connection.start().then(function () {
      console.log("Hub 連線完成");
    }).catch(function (err: any) {
      alert('連線錯誤: ' + err.toString());
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
    this.connection.on("UpdSelfID", (ConnectId: any,SelfName) => {
      // $('#SelfID').html(id);
      this.SelfID = SelfName;
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

  //傳送訊息
  // $('#sendButton').on('click', function() {
  //     let selfID = $('#SelfID').html();
  //     let message = $('#message').val();
  //     let sendToID = $('#sendToID').val();
  //     connection.invoke("SendMessage", selfID, message, sendToID).catch(function (err) {
  //         alert('傳送錯誤: ' + err.toString());
  //     });
  // });

}
