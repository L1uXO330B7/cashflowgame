

import { MatTableDataSource } from '@angular/material/table';
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ApiRequest } from 'src/app/common/models/ApiRequest';
import { ApiService } from 'src/app/common/services/api.service';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { BaseComponent } from 'src/app/common/components/base.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { QuestionArgs } from 'src/app/common/models/QuestionModel';

@Component({
  selector: 'app-question-table',
  templateUrl: './question-table.component.html',
  styleUrls: ['./question-table.component.scss']
})
export class QuestionsTableComponent extends BaseComponent implements OnInit {

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
    this.QuestionsRead(this.pageIndex, this.pageSize, []);
  }

  ngAfterViewInit() {
    this.Items.paginator = this.paginator;
    this.paginator.page.subscribe((page: PageEvent) => {
      this.pageIndex = page.pageIndex;
      this.pageSize = page.pageSize;
      this.QuestionsRead(page.pageIndex, page.pageSize, []);
    }, (err: any) => {
      console.log(err);
    });
  }

  @ViewChild('Dialog', { static: true }) Dialog: TemplateRef<any> | any;
  DialogRef: MatDialogRef<any> | any;
  OpenDiaglog(IsNew: boolean, Id: any) {
    if (IsNew) {
      this.Item = new QuestionArgs();
      this.Item.Status = 1;
    } else {
      // 取單筆
      let listInt = [Id];
      let Arg =
      {
        "Key": "Id",
        "JsonString": JSON.stringify(listInt)
      };
      this.QuestionsRead(1, 5, [Arg]);
    }
    this.DialogRef = this.dialog.open(this.Dialog);
  }

  @ViewChild('CloseDialog', { static: true }) CloseDialog: TemplateRef<any> | any;
  CloseDialogRef: MatDialogRef<any> | any;
  OpenDeleteDialog(Id: any) {
    this.CloseDialogRef = this.dialog.open(this.CloseDialog);
    this.Item = new QuestionArgs();
    this.Item.Id = Id;
  }

  Items: any;
  FilterItems: any;
  Item = new QuestionArgs();
  QuestionsRead(PageIndex: any, PageSize: any, Args: any) {
    let Req = new ApiRequest<any>();
    Req.Args = Args;
    Req.PageIndex = PageIndex > 0 ? PageIndex : 1;
    Req.PageSize = PageSize;
    this._ApiService.QuestionsRead(Req).subscribe((Res) => {
      if (Res.Success) {
        if (Args.length <= 0) {
          this.Items = Res.Data;
          this.FilterItems = Res.Data;
          this.totalDataCount = Res.TotalDataCount;
        }
        else {
          // Preserve newlines, etc. - use valid JSON
          // let answer = Res.Data[0].Answer;
          //  answer = answer.replace(/\\n/g, "\\n")
          //   .replace(/\\'/g, "\\'")
          //   .replace(/\\"/g, '\\"')
          //   .replace(/\\&/g, "\\&")
          //   .replace(/\\r/g, "\\r")
          //   .replace(/\\t/g, "\\t")
          //   .replace(/\\b/g, "\\b")
          //   .replace(/\\f/g, "\\f");
          // // Remove non-printable and other non-valid JSON characters
          // answer = answer.replace(/[\u0000-\u0019]+/g, "");
          // console.log(Res.Data[0], "data");
          // answer = JSON.parse(Res.Data[0].Answer)
          // // console.log(JSON.parse(answer), "answer");
          // console.log(answer,"answer");
          let ans = Res.Data[0].Answer;
          console.log(ans,"ans");
          let arr = ans.split(",");
          console.log(arr,"arr");
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

  QuestionsDelete(Id: any) {
    let Req = new ApiRequest();
    Req.Args = [Id];
    this._ApiService.QuestionsDelete(Req).subscribe((Res) => {
      if (Res.Success) {
        this.QuestionsRead(this.pageIndex, this.pageSize, []);
        this.CloseDialogRef.close();
      }
    }, (err: any) => {
      console.log(err);
    });
  }

  QuestionsUpdate() {
    let Req = new ApiRequest();
    Req.Args = [this.Item];
    this._ApiService.QuestionsUpdate(Req).subscribe((Res) => {
      if (Res.Success) {
        this.DialogRef.close();
        this.QuestionsRead(this.pageIndex, this.pageSize, []);
      }
    }, (err: any) => {
      console.log(err);
    });
  }

  QuestionsCreate() {
    let Req = new ApiRequest();
    console.log(this.Item.Answer, "initial");
    console.log(this.Item.Answer, "answer");
    Req.Args = [this.Item];
    this._ApiService.QuestionsCreate(Req).subscribe((Res) => {
      if (Res.Success) {
        this.DialogRef.close();
        this.QuestionsRead(this.pageIndex, this.pageSize, []);
      }
    }, (err: any) => {
      console.log(err);
    });
  }

}

