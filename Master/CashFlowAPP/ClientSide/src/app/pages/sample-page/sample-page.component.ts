import { SignalrHubService } from './../../service/signalr-hub.service';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-sample-page',
  templateUrl: './sample-page.component.html',
  styleUrls: ['./sample-page.component.scss']
})
export class SamplePageComponent implements OnInit {

  constructor(public _Signalr:SignalrHubService) { }

  ngOnInit(): void {
    this.test();
  }
  UserToken:any=localStorage.getItem("Token");
  Param:any="";
  test(){
    this.Param = `token=${this.UserToken}`
    console.log("this.UserToken",this.UserToken);
    if(this.UserToken==null||undefined||""){
      this.UserToken = localStorage.getItem("StrangerName");
      console.log("this.UserToken",this.UserToken);
      this.Param = `stranger=${this.UserToken}`
    }
    this._Signalr.Connect(`${this.Param}`);
    this._Signalr.OnObservable("UpdList").subscribe((res:any)=>{
      console.log(res,"1");
    });
    this._Signalr.OnObservable("UpdContent").subscribe((res:any)=>{
      console.log(res,"2");
    });
 }

}
