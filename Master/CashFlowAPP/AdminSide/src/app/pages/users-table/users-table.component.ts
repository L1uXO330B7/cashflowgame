import { MatTableDataSource } from '@angular/material/table';
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ApiRequest } from 'src/app/common/models/ApiRequest';
import { ApiService } from 'src/app/common/services/api.service';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { BaseComponent } from 'src/app/common/components/base.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { UserArgs } from 'src/app/common/models/UserModel';

@Component({
  selector: 'app-users-table',
  templateUrl: './users-table.component.html',
  styleUrls: ['./users-table.component.scss']
})
export class UsersTableComponent extends BaseComponent implements OnInit {

  constructor(
    private _HttpClient: HttpClient,
    private _ApiService: ApiService,
    public dialog: MatDialog
  ) {
    super();
  }



  @ViewChild('paginator') paginator: MatPaginator | any;
  @ViewChild('filter') filter: ElementRef | any;

  ngOnInit(): void {
    this.UsersData = new MatTableDataSource();
    this.UsersRead(this.pageIndex, this.pageSize,[]);
  }

  ngAfterViewInit() {
    this.UsersData.paginator = this.paginator;
    this.paginator.page.subscribe((page: PageEvent) => {
      this.pageIndex = page.pageIndex;
      this.pageSize = page.pageSize;
      this.UsersRead(page.pageIndex, page.pageSize,[]);
    }, (err: any) => {
      console.log(err);
    });
  }

  @ViewChild('Dialog', { static: true }) Dialog: TemplateRef<any> | any;



  DialogRef:MatDialogRef<any> | any;
  OpenDiaglog(IsNew: boolean, Id: any) {
    if (IsNew) {
      this.UserData = new UserArgs();
    } else {
      // 取單筆
      let listInt = [Id];
      let Arg =
        {
          "Key": "Id",
          "JsonString": JSON.stringify(listInt)
        };
      this.UsersRead(0, 5,[Arg]);
    }
    this.DialogRef=this.dialog.open(this.Dialog);

  }
  @ViewChild('CloseDialog', { static: true }) CloseDialog: TemplateRef<any> | any;
  CloseDialogRef:MatDialogRef<any> | any;
  OpenCloseDialog(Id:any){
    this.CloseDialogRef=this.dialog.open(this.CloseDialog);
    this.UserData = new UserArgs();
    this.UserData.Id = Id;
  }
  UsersData: any;
  FilterUsersData: any;
  UserData  = new UserArgs();
  UsersRead(PageIndex: any, PageSize: any,Args:any) {
    let Req = new ApiRequest<any>();
    Req.Args = Args;
    Req.PageIndex = PageIndex;
    Req.PageSize = PageSize;
    this._ApiService.UsersRead(Req).subscribe((Res) => {
      if (Res.Success) {
        if(Args.length<=0){
          this.UsersData = Res.Data;
          this.FilterUsersData = Res.Data;
          this.totalDataCount = Res.TotalDataCount;
        }
        else{
          this.UserData = Res.Data[0];
          console.log(this.UserData);
        }
      }
    }, (err: any) => {
      console.log(err);
    });
  }

  ApplyFilterFrontEnd(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;

    if (filterValue === '') {
      this.UsersData = this.FilterUsersData;
    }
    else {

      // 前端搜尋
      this.UsersData = [];
      this.FilterUsersData
        .forEach(
          (data: any) => {
            let Check = false;
            for (const key in data) {
              if (data[key].toString().match(filterValue)) {
                Check = true;
              }
            }

            if (Check) {
              this.UsersData.push(data);
            }
          }
        );
    }
  }

  UserDelete(Id: any) {
    let Req = new ApiRequest();
    Req.Args = [Id];
    this._ApiService.UserDelete(Req).subscribe((Res) => {
      if (Res.Success) {
        this.UsersRead(this.pageIndex, this.pageSize,[]);
        this.CloseDialogRef.close();
      }
    }, (err: any) => {
      console.log(err);
    });
  }
  UserUpdate(){
    console.log(this.UserData.Id);
    let Req = new ApiRequest();
    Req.Args = [this.UserData];
    this._ApiService.UserUpdate(Req).subscribe((Res) => {
      if (Res.Success) {
        console.log(Res);
        this.DialogRef.close();
        this.UsersRead(this.pageIndex, this.pageSize,[]);
      }
    }, (err: any) => {
      console.log(err);
    });
  }
  UserCreate(){
    console.log(this.UserData.Id);
    let Req = new ApiRequest();
    Req.Args = [this.UserData];
    this._ApiService.UserCreate(Req).subscribe((Res) => {
      if (Res.Success) {
        console.log(Res);
        this.DialogRef.close();
        this.UsersRead(this.pageIndex, this.pageSize,[]);
      }
    }, (err: any) => {
      console.log(err);
    });
  }
}
