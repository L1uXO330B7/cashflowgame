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
  Card= {
    transition: "all 1s",
    transform: "",
  };
  Info= {
    transition: "all 1s",
    transform: "",
    textAlign: "center",
  };

  CardMove() {
    console.log(this.windowWidth);
    if (this.windowWidth <= 968) {
      if (this.change) {
        this.Card.transform = `translateY(10rem) !important`;
      } else {
        this.Card.transform = `translateX(0em) !important`;
      }
    } else {
      if (this.change) {
        this.Card.transform = `translateX(${this.distance}px) !important`;
      } else {
        this.Card.transform = `translateX(0em) !important`;
      }
    }
  }
  InfoMove() {
    console.log(this.distance);
    console.log(this.windowWidth);
    if (this.windowWidth <= 968) {
      if (this.change) {
        this.Info.transform = `translateY(-20rem) !important`;
      } else {
        this.Info.transform = `translateY(0em) !important`;
      }
    } else {
      if (this.change) {
        this.Info.transform = `translateX(-${this.distance}px) !important`;
      } else {
        this.Info.transform = `translateX(0em) !important`;
      }
    }
  }
  @ViewChild('bar', { static: true }) barDOM?: any;
  @ViewChild('card', { static: true }) cardDOM?: any;
  changeLogin() {

    this.change = !this.change;
    this.distance =
      this.barDOM?.clientWidth - (this.cardDOM?.clientWidth + 100);
    this.CardMove();
    this.InfoMove();
  }

}

