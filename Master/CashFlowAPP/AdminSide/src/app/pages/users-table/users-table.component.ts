import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ApiRequest } from 'src/app/common/models/ApiRequest';
import { ApiService } from 'src/app/common/services/api.service';

@Component({
  selector: 'app-users-table',
  templateUrl: './users-table.component.html',
  styleUrls: ['./users-table.component.scss']
})
export class UsersTableComponent implements OnInit {

  constructor(private _HttpClient: HttpClient, private _ApiService: ApiService) { }

  ngOnInit(): void {
    this.UsersRead();
  }

  UsersData:any;

  UsersRead(){
    let Req = new ApiRequest<any>();
    Req.Args=[];
    this._ApiService.UsersRead(Req).subscribe((Res)=>{
      if(Res.Success){
        console.log("Res",Res);
        this.UsersData = Res.Data;
      }
    })
  }
}
