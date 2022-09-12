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

  CardData:any={Name:"人生，就是由一連s串的選擇所組成，一連串的巧合，成就現在的你"};
  FirstTime = false;
  OnCard() {
    this._Signalr.OnObservable("Okay").subscribe((Res: any) => {
      console.log(Res,"CardResponese");
      this.CardData=Res[0];
    });
  }
  Agree(){

  }
  DisAgree(){

  }
}
