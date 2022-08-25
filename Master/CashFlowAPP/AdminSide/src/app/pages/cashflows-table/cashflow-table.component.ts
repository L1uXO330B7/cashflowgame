

import { MatTableDataSource } from '@angular/material/table';
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ApiRequest } from 'src/app/common/models/ApiRequest';
import { ApiService } from 'src/app/common/services/api.service';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { BaseComponent } from 'src/app/common/components/base.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { CashFlowArgs } from 'src/app/common/models/CashFlowModel';

@Component({
  selector: 'app-cashflow-table',
  templateUrl: './cashflow-table.component.html',
  styleUrls: ['./cashflow-table.component.scss']
})
export class CashFlowsTableComponent extends BaseComponent implements OnInit {

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
    this.CashFlowsRead(this.pageIndex, this.pageSize, []);
  }

  ngAfterViewInit() {
    this.Items.paginator = this.paginator;
    this.paginator.page.subscribe((page: PageEvent) => {
      this.pageIndex = page.pageIndex;
      this.pageSize = page.pageSize;
      this.CashFlowsRead(page.pageIndex, page.pageSize, []);
    }, (err: any) => {
      console.log(err);
    });
  }

  @ViewChild('Dialog', { static: true }) Dialog: TemplateRef<any> | any;
  DialogRef: MatDialogRef<any> | any;
  OpenDiaglog(IsNew: boolean, Id: any) {
    if (IsNew) {
      this.Item = new CashFlowArgs();
      this.Item.Status = 1;
    } else {
      // 取單筆
      let listInt = [Id];
      let Arg =
      {
        "Key": "Id",
        "JsonString": JSON.stringify(listInt)
      };
      this.CashFlowsRead(1, 5, [Arg]);
    }
    this.DialogRef = this.dialog.open(this.Dialog);
  }

  @ViewChild('CloseDialog', { static: true }) CloseDialog: TemplateRef<any> | any;
  CloseDialogRef: MatDialogRef<any> | any;
  OpenDeleteDialog(Id: any) {
    this.CloseDialogRef = this.dialog.open(this.CloseDialog);
    this.Item = new CashFlowArgs();
    this.Item.Id = Id;
  }

  Items: any;
  FilterItems: any;
  Item = new CashFlowArgs();
  CashFlowsRead(PageIndex: any, PageSize: any, Args: any) {
    let Req = new ApiRequest<any>();
    Req.Args = Args;
    Req.PageIndex = PageIndex > 0 ? PageIndex : 1;
    Req.PageSize = PageSize;
    this._ApiService.CashFlowsRead(Req).subscribe((Res) => {
      if (Res.Success) {
        if (Args.length <= 0) {
          this.Items = Res.Data;
          this.FilterItems = Res.Data;
          this.totalDataCount = Res.TotalDataCount;
        }
        else {
          this.Item = Res.Data[0];
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

  CashFlowsDelete(Id: any) {
    let Req = new ApiRequest();
    Req.Args = [Id];
    this._ApiService.CashFlowsDelete(Req).subscribe((Res) => {
      if (Res.Success) {
        this.CashFlowsRead(this.pageIndex, this.pageSize, []);
        this.CloseDialogRef.close();
      }
    }, (err: any) => {
      console.log(err);
    });
  }

  CashFlowsUpdate() {
    let Req = new ApiRequest();
    Req.Args = [this.Item];
    this._ApiService.CashFlowsUpdate(Req).subscribe((Res) => {
      if (Res.Success) {
        this.DialogRef.close();
        this.CashFlowsRead(this.pageIndex, this.pageSize, []);
      }
    }, (err: any) => {
      console.log(err);
    });
  }

  CashFlowsCreate() {
    let Req = new ApiRequest();
    Req.Args = [this.Item];
    this._ApiService.CashFlowsCreate(Req).subscribe((Res) => {
      if (Res.Success) {
        this.DialogRef.close();
        this.CashFlowsRead(this.pageIndex, this.pageSize, []);
      }
    }, (err: any) => {
      console.log(err);
    });
  }

}

