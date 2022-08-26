import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.components.scss']
})
export class NavbarComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
    this.LoginToUserInfo();
  }
  IsLogin:boolean=false;
  LoginToUserInfo(){
    let UserId = localStorage.getItem('UserId');
    console.log(UserId);
    if (UserId!=""&&UserId!=null){
      this.IsLogin=true;
    }
  }
}
