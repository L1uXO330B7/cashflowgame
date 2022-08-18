import { MatTableDataSource } from '@angular/material/table';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ApiRequest } from 'src/app/common/models/ApiRequest';
import { ApiService } from 'src/app/common/services/api.service';

import { MatPaginator, PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-users-table',
  templateUrl: './users-table.component.html',
  styleUrls: ['./users-table.component.scss']
})
export class UsersTableComponent implements OnInit {

  constructor(private _HttpClient: HttpClient, private _ApiService: ApiService) { }
  @ViewChild('paginator') paginator: MatPaginator | any;

  ngOnInit(): void {
    this.UsersData = new MatTableDataSource();

    this.UsersRead(0,5);
  }


  ngAfterViewInit() {
    this.UsersData.paginator = this.paginator;
    this.paginator.page.subscribe((page: PageEvent) => {
      this.UsersRead(page.pageIndex, page.pageSize);
      console.log(page.pageIndex, page.pageSize);
    });
  }

  UsersData: any;
  totalCount: any;
  UsersRead(pageIndex: any, pageSize: any) {
    let Req = new ApiRequest<any>();
    Req.Args = [];
    Req.PageIndex = pageIndex+1;
    Req.PageSize = pageSize;
    this._ApiService.UsersRead(Req).subscribe((Res) => {
      if (Res.Success) {
        console.log("Res", Res);
        this.UsersData = Res.Data;
        this.totalCount = Res.TotalDataCount;
      }
    })
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

  // displayedColumns: string[] = ['User', 'Account', 'Name', 'Status'];
  // applyFilter(event: Event) {
  //   const filterValue = (event.target as HTMLInputElement).value;
  //   this.UsersData.filter = filterValue.trim().toLowerCase();

  //   if (this.UsersData.paginator) {
  //     this.UsersData.paginator.firstPage();
  //   }
  // }
}
