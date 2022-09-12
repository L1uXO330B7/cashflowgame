import { Component, OnInit } from '@angular/core';
import { SignalrHubService } from 'src/app/service/signalr-hub.service';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.scss']
})
export class CardComponent implements OnInit {

  constructor(public _Signalr: SignalrHubService) { }

  ngOnInit(): void {
    this.OnCard();
  }

  OnCard() {
    this._Signalr.OnObservable("Okay").subscribe((Res: any) => {
      console.log(Res,"CardResponese");
    });
  }
}
