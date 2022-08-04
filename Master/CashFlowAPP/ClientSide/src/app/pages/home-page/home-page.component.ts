import { Component, DoCheck, OnInit } from '@angular/core';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit,DoCheck {

  constructor() {}

  ngDoCheck(): void {
    //Called every time that the input properties of a component or a directive are checked. Use it to extend change detection by performing a custom check.
    //Add 'implements DoCheck' to the class.

  }

  ngOnInit(): void {  setTimeout(() => this.TypeWriter(), 1000);
  }
  ngAfterViewInit(): void{}
  SloganText:string[]=[];
  Index=0;
  AnimatedText:string="";
  TypeWriter() {
    this.SloganText="錢董讓你懂錢".split('');
    console.log("type",this.SloganText.length);
    const speed = 200;
    if (this.Index < this.SloganText.length) {
      this.AnimatedText += this.SloganText[this.Index];
      this.Index++;
      console.log("index",this.Index);
      setTimeout(()=>this.TypeWriter(), speed);
    }
    else{
      setTimeout(() => {
        //注意這裡必須使用arrow function才能讓this被正確指向
        this.Index = 0;
        this.AnimatedText = "";
        this.TypeWriter();
      }, 5000);
    }
  }
}

