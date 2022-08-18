import { MatTableDataSource } from '@angular/material/table';
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ApiRequest } from 'src/app/common/models/ApiRequest';
import { ApiService } from 'src/app/common/services/api.service';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'app-users-table',
  templateUrl: './users-table.component.html',
  styleUrls: ['./users-table.component.scss']
})
export class UsersTableComponent implements OnInit {

  constructor(
    private _HttpClient: HttpClient,
    private _ApiService: ApiService,
  ) {

  }

  @ViewChild('paginator') paginator: MatPaginator | any;
  @ViewChild('filter') filter: ElementRef | any;

  ngOnInit(): void {
    this.UsersData = new MatTableDataSource();
    this.UsersRead(0, 5);
  }

  UsersData: any;
  FilterUsersData: any;
  UsersRead(pageIndex: any, pageSize: any) {
    let Req = new ApiRequest<any>();
    Req.Args = [];
    Req.PageIndex = pageIndex + 1;
    Req.PageSize = pageSize;
    this._ApiService.UsersRead(Req).subscribe((Res) => {
      if (Res.Success) {
        this.UsersData = Res.Data;
        this.FilterUsersData = Res.Data;
      }
    }, (err: any) => {
      console.log(err);
    });
  }

  // GetPages(pageIndex: any, pageSize: any) {
  //   let Req = new ApiRequest<any>();
  //   Req.Args = [];
  //   Req.PageIndex = pageIndex;
  //   Req.PageSize = pageSize;
  //   this._ApiService.UsersRead(Req).subscribe((Res) => {
  //     if (Res.Success) {
  //       console.log("Res", Res);
  //       this.UsersData = Res.Data;
  //     }
  //   })
  // }

  applyFilterFrontEnd(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;

    if (filterValue === '') {
      this.UsersData = this.FilterUsersData;
    }
    else {

      // 前端搜尋
      this.UsersData = [];
      this.FilterUsersData
        .forEach(
          (res: any) => {
            let Check = false;
            for (const key in res) {
              console.log('res[key]',res[key]);
              if (res[key].toString().match(filterValue)) {
                Check = true;
              }
            }

            if(Check)
            {
              this.UsersData.push(res);
            }
          }
        );
    }
  }
}
