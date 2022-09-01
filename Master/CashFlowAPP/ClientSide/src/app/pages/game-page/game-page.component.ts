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
    console.log("clickCard",this.OpenCard)
    this.OpenCard = !this.OpenCard;
  }
}
