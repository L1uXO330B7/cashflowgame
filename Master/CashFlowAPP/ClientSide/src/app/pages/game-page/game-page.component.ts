import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-game-page',
  templateUrl: './game-page.component.html',
  styleUrls: ['./game-page.component.scss']
})
export class GamePageComponent implements OnInit {

  constructor() {

  }

  ngOnInit(): void {
  }

  OpenChatRoom:boolean=false;
  OpenInfo:boolean=true;
  ToggleChatRoom(){
    this.OpenChatRoom=!this.OpenChatRoom;
  }
  ToggleInfo(){
    this.OpenInfo=!this.OpenInfo;
  }
}
