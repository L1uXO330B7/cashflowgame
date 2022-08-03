import { Component, OnInit } from '@angular/core';
import * as signalR from "@microsoft/signalr"

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
    this.
  }
  connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

  //與Server建立連線
  connect(){
    this.connection.start().then(function () {
      console.log("Hub 連線完成");
  }).catch(function (err:any) {
      alert('連線錯誤: ' + err.toString());
  });
  }


// 更新連線 ID 列表事件
IDList:string = "";
  update(){
      this.connection.on("UpdList", (jsonList)=>{
      var list = JSON.parse(jsonList);
      // $("#IDList li").remove();
      this.IDList = "";
      list.forEach((value:any, index:number, array:any) => {
        this.IDList+=`<li class='list-group-item'>${list[index]}</li>`;
      })
  });
  }
  SelfID="";
  // 更新用戶個人連線 ID 事件
  UpdSelfID(){
    this.connection.on("UpdSelfID",(id)=>{
      // $('#SelfID').html(id);
      this.SelfID=id;
  });
  }

  Content="";
  // 更新聊天內容事件
  UpdContent(){
    this.connection.on("UpdContent", (msg)=>{
      this.Content += `
      <li class="list-group-item">${msg}</li>
      `
      // $("#Content").append($("<li></li>").attr("class", "list-group-item").text(msg));
  });
  }
  message="";
  sendToID="";
  SendMsg(){
      let selfID = document.querySelector('#SelfID')?.innerHTML;
      this.connection.invoke("SendMessage", selfID, this.message, this.sendToID).catch(function (err) {
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
