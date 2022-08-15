import { ApiRequest } from '../../common/models/ApiRequest';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiService } from 'src/app/common/services/api.service';

@Component({
  selector: 'app-dashboard-page',
  templateUrl: './dashboard-page.component.html',
  styleUrls: ['./dashboard-page.component.scss']
})
export class DashboardPageComponent implements OnInit {

  constructor(private _HttpClient: HttpClient, private _ApiService: ApiService) { }

  ngOnInit(): void {
    this.UsersRead();
  }
  UsersRead(){
    let Req = new ApiRequest<any>();
    Req.Args=[];
    this._ApiService.UsersRead(Req).subscribe((Res)=>{
      if(Res.Success){
        console.log("Res",Res);
      }
    })
  }
}
