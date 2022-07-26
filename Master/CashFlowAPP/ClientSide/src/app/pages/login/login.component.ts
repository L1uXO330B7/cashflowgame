import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
  change=false;
  windowWidth=window.innerWidth;
  distance=0;
  Card="";
  Info="";

  CardMove() {
    console.log("windowWidth",this.windowWidth);
    console.log("change",this.change);
    if (this.windowWidth <= 968) {
      if (this.change) {
        this.Card = `translateY(10rem) !important`;
      } else {
        this.Card = `translateX(0em) !important`;
      }
    } else {
      if (this.change) {
        this.Card = `translateX(${this.distance}px) !important`;
        console.log(this.Card);
      } else {
        this.Card = `translateX(0px) !important`;
      }
    }
  }
  InfoMove() {
    console.log("barDOM",this.barDOM);
    console.log("distance",this.distance);
    console.log("InfoWidth",this.windowWidth);
    if (this.windowWidth <= 968) {
      if (this.change) {
        this.Info = `translateY(-20rem) !important`;
      } else {
        this.Info = `translateY(0em) !important`;
      }
    } else {
      if (this.change) {
        this.Info= `translateX(-${this.distance}px) !important`;
      } else {
        this.Info = `translateX(0em) !important`;
      }
    }
  }
  @ViewChild('bar', { static: true }) barDOM: any;
  @ViewChild('card', { static: true }) cardDOM: any;
  changeLogin() {
    this.change = !this.change;
    this.distance =
      this.barDOM.nativeElement.clientWidth - (this.cardDOM.nativeElement.clientWidth + 100);
      console.log("changeLogin",this.barDOM.nativeElement.clientWidth);
    this.CardMove();
    this.InfoMove();
  }

}

