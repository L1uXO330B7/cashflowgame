import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/service/shared.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.components.scss']
})
export class NavbarComponent implements OnInit {

  constructor(public _SharedService:SharedService,public _Router:Router){ }

  ngOnInit(): void {
    this.LoginToUserInfo();
  }
  IsLogin:boolean=false;
  UserData:any;
  LoginToUserInfo(){
    let UserId = localStorage.getItem('UserId');
    console.log(UserId);
    this._SharedService.SharedData.subscribe((Res)=>{
      this.UserData = Res;
      console.log(this.UserData,"a");
    })
    if (UserId!=""&&UserId!=null){

      this.IsLogin=true;
    }
  }
  UserLogOut(){
    localStorage.removeItem("UserId");
    localStorage.removeItem("Token");
    this._Router.navigate(['/', 'login']);
  }
}
