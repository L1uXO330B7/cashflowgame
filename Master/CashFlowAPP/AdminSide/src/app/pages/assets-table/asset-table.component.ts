﻿

import { MatTableDataSource } from '@angular/material/table';
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ApiRequest } from 'src/app/common/models/ApiRequest';
import { ApiService } from 'src/app/common/services/api.service';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { BaseComponent } from 'src/app/common/components/base.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AssetArgs } from 'src/app/common/models/AssetModel';

@Component({
  selector: 'app-asset-table',
  templateUrl: './asset-table.component.html',
  styleUrls: ['./asset-table.component.scss']
})
export class AssetsTableComponent extends BaseComponent implements OnInit {

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
    this.Items = new MatTableDataSource();
    this.AssetsRead(this.pageIndex, this.pageSize, []);
  }

  ngAfterViewInit() {
    this.Items.paginator = this.paginator;
    this.paginator.page.subscribe((page: PageEvent) => {
      this.pageIndex = page.pageIndex;
      this.pageSize = page.pageSize;
      this.AssetsRead(page.pageIndex, page.pageSize, []);
    }, (err: any) => {
      console.log(err);
    });
  }

  @ViewChild('Dialog', { static: true }) Dialog: TemplateRef<any> | any;
  DialogRef: MatDialogRef<any> | any;
  OpenDiaglog(IsNew: boolean, Id: any) {
    if (IsNew) {
      this.Item = new AssetArgs();
      this.Item.Status = 1;
    } else {
      // 取單筆
      let listInt = [Id];
      let Arg =
      {
        "Key": "Id",
        "JsonString": JSON.stringify(listInt)
      };
      this.AssetsRead(1, 5, [Arg]);
    }
    this.DialogRef = this.dialog.open(this.Dialog);
  }

  @ViewChild('CloseDialog', { static: true }) CloseDialog: TemplateRef<any> | any;
  CloseDialogRef: MatDialogRef<any> | any;
  OpenDeleteDialog(Id: any) {
    this.CloseDialogRef = this.dialog.open(this.CloseDialog);
    this.Item = new AssetArgs();
    this.Item.Id = Id;
  }

  Items: any;
  FilterItems: any;
  Item = new AssetArgs();
  AssetCategorys : any;
  AssetsRead(PageIndex: any, PageSize: any, Args: any) {
    let Req = new ApiRequest<any>();
    Req.Args = Args;
    Req.PageIndex = PageIndex > 0 ? PageIndex : 1;
    Req.PageSize = PageSize;
    this._ApiService.AssetsRead(Req).subscribe((Res) => {

      if (Res.Success) {
        console.log(Res);

        this.AssetCategorys = Res.Data.AssetCategorys;

        if (Args.length <= 0) {
          this.Items = Res.Data.Assets;
          this.FilterItems = Res.Data.Assets;
          this.totalDataCount = Res.TotalDataCount;
        }
        else {
          this.Item = Res.Data.Assets[0];
        }
      }
    }, (err: any) => {
      console.log(err);
    });
  }

  ApplyFilterFrontEnd(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;

    if (filterValue === '') {
      this.Items = this.FilterItems;
    }
    else {
      // 前端搜尋
      this.Items = [];
      this.FilterItems
        .forEach(
          (data: any) => {
            let Check = false;
            for (const key in data) {
              if (data[key].toString().match(filterValue)) {
                Check = true;
              }
            }

            if (Check) {
              this.Items.push(data);
            }
          }
        );
    }
  }

  AssetsDelete(Id: any) {
    let Req = new ApiRequest();
    Req.Args = [Id];
    this._ApiService.AssetsDelete(Req).subscribe((Res) => {
      if (Res.Success) {
        this.AssetsRead(this.pageIndex, this.pageSize, []);
        this.CloseDialogRef.close();
      }
    }, (err: any) => {
      console.log(err);
    });
  }

  AssetsUpdate() {
    let Req = new ApiRequest();
    Req.Args = [this.Item];
    this._ApiService.AssetsUpdate(Req).subscribe((Res) => {
      if (Res.Success) {
        this.DialogRef.close();
        this.AssetsRead(this.pageIndex, this.pageSize, []);
      }
    }, (err: any) => {
      console.log(err);
    });
  }

  AssetsCreate() {
    let Req = new ApiRequest();
    Req.Args = [this.Item];
    this._ApiService.AssetsCreate(Req).subscribe((Res) => {
      if (Res.Success) {
        this.DialogRef.close();
        this.AssetsRead(this.pageIndex, this.pageSize, []);
      }
    }, (err: any) => {
      console.log(err);
    });
  }

}

