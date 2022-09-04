import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiRequest } from './models/ApiRequest';
import { ApiService } from './service/api.service';
import { SharedService } from './service/shared.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  ngOnInit(): void {
    this.ReadUserArg();
    }
  constructor(public _SharedService:SharedService,public _Router:Router,private _ApiService:ApiService){}
  title = 'ClientSide';

  IsLogin:boolean=false;
  UserId = localStorage.getItem("UserId");
  UserData:any;
  UserDataName:string|any;
  ReadUserArg(){
    if(this.UserId!=null&&this.UserId!=""){
      this.IsLogin = true;
      let Req = new ApiRequest<any>();
      let listInt = [this.UserId];
      let Arg =
      {
        "Key": "Id",
        "JsonString": JSON.stringify(listInt)
      };
      Req.Args = [Arg];
      Req.PageIndex = 1;
      Req.PageSize = 15;
      this._ApiService.GetUserData(Req).subscribe((Res) => {
        console.log(Res);
        if (Res.Success) {
        this.UserData = Res.Data.Users[0];
        this.UserDataName = this.UserData.Name;
        this._SharedService.SetShareData(this.UserData);
        console.log(this.UserData);
        }
      });
    }
  }
}
