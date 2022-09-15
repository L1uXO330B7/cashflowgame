import { Component, OnInit } from '@angular/core';
import { Card } from 'src/app/models/CardModel';
import { SignalrHubService } from 'src/app/service/signalr-hub.service';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.scss']
})
export class CardComponent implements OnInit {

  constructor(public _Signalr: SignalrHubService) { }

  ngOnInit(): void {
    this.CardData.Name="人生，就是由一連串的選擇所組成，一連串的巧合，成就現在的你";
    this.CardData.Type="強迫中獎";
    this.OnCard();
  }

  CardData:Card=new Card();
  CardValue:any;
  FirstTime = false;
  OnCard() {
    this._Signalr.OnObservable("DrawCard").subscribe((Res: any) => {
      this.CardData=Res[0];
      if(typeof(Res[0])==typeof(1)){
        this.CardValue=Math.round(Res[1]);
      }
      else{
              this.CardValue=Res[1];
      }
    });
  }
  Agree(){
    this._Signalr.Invoke("ChoiceOfCard",5);
  }
  DisAgree(){

  }
}
