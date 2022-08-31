import { Router } from '@angular/router';

import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Chart } from 'chart.js';


@Component({
  selector: 'app-dashboard-page',
  templateUrl: './dashboard-page.component.html',
  styleUrls: ['./dashboard-page.component.scss']
})
export class DashboardPageComponent implements AfterViewInit  {

  constructor(private _Router:Router) { }
  ngAfterViewInit(){
     this.context=this.buyers.nativeElement.getContext('2d');
    new Chart(this.context,this.buyerData);
  }
  ngOnInit(): void {

  }

  AdminLogout(){
    localStorage.removeItem("UserId");
    localStorage.removeItem("Token");
    this._Router.navigate(['/', 'login']);
  }

  @ViewChild('buyers', { static: true }) buyers: ElementRef<HTMLCanvasElement>|any;
  public context: CanvasRenderingContext2D | any;

  buyerData:any = {
    labels : ["January","February","March","April","May","June"],
    datasets : [
    {
        fillColor : "rgba(172,194,132,0.4)",
        strokeColor : "#ACC26D",
        pointColor : "#fff",
        pointStrokeColor : "#9DB86D",
        data : [203,156,99,251,305,247]
    }
]
}
}
