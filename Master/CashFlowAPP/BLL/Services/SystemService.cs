using Common.Methods;
using DPL.EF;
using System.Reflection;
using System.Text;

namespace BLL.Services
{
    public class SystemService
    {
        private readonly CashFlowDbContext _CashFlowDbContext;

        public SystemService(CashFlowDbContext cashFlowDbContext)
        {
            _CashFlowDbContext = cashFlowDbContext;
        }

        public void CreateTemplateByTableName()
        {
            var RootDirectoryPath = System.IO.Directory.GetCurrentDirectory();
            var ScriptRoot = $@"{RootDirectoryPath}\Script";
            var IsExists = Directory.Exists(ScriptRoot); // 檢查是否有該路徑的檔案或資料夾
            if (!IsExists)
            {
                //存檔資料夾不存在，新增資料夾
                Directory.CreateDirectory(ScriptRoot);
            }

            // 取出 EF 的 Class 做後續應用
            // 參考 https://stackoverflow.com/questions/79693/getting-all-types-in-a-namespace-via-reflection
            var types = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .Where(t => t.IsClass && t.Namespace == "DPL.EF")
                       .ToList();

            #region 後端

            // 後端目錄
            var BackEndRoot = $@"{ScriptRoot}\BackEnd";
            Method.CreateWithoutDirectory(BackEndRoot); // 上面抽去共用

            // XxxController.cs
            var ControllersRoot = $@"{BackEndRoot}\Controllers";
            Method.CreateWithoutDirectory(ControllersRoot);

            // IXxxService.cs
            var IServicesRoot = $@"{BackEndRoot}\IServices";
            Method.CreateWithoutDirectory(IServicesRoot);

            // XxxService.cs
            var ServicesRoot = $@"{BackEndRoot}\Services";
            Method.CreateWithoutDirectory(ServicesRoot);

            // XxxModel.cs
            var ModelsRoot = $@"{BackEndRoot}\Models";
            Method.CreateWithoutDirectory(ModelsRoot);

            var ProgramAddScoped = "";

            // Script
            foreach (var type in types)
            {
                if (!type.FullName.ToLower().Contains("context"))
                {
                    string FilePath = "";
                    var Properties = type.GetTypeInfo().DeclaredProperties;
                    var Props = Properties.Select(x => new
                    {
                        PropName = x.Name,
                        PropType = Method.GetSqlDataTypeString(x.PropertyType.FullName, 2)
                    })
                    .ToList();

                    // XxxController.cs
                    {
                        var XxxController = Templates.BackEnd.XxxController
                            .Replace("#UsersController", $"{type.Name}sController")
                            .Replace("#CreateUserArgs", $"Create{type.Name}Args")
                            .Replace("#ReadUserArgs", $"Read{type.Name}Args")
                            .Replace("#UpdateUserArgs", $"Update{type.Name}Args")
                            .Replace("#UserService", $"{type.Name}sService")
                            .Replace("#IUsersService", $"I{type.Name}sService")
                            ;

                        FilePath = $@"{ControllersRoot}\{type.Name}sController.cs";

                        File.WriteAllText(FilePath, XxxController, Encoding.UTF8);
                    }

                    // IXxxService.cs
                    {
                        var IXxxService = Templates.BackEnd.IXxxService
                            .Replace("#IUsersService", $"I{type.Name}sService")
                            ;

                        FilePath = $@"{IServicesRoot}\I{type.Name}sService.cs";

                        File.WriteAllText(FilePath, IXxxService, Encoding.UTF8);
                    }

                    // XxxService.cs
                    {
                        var LowerVariable = type.Name.FirstCharToLower();

                        string UserArgAdd = "";
                        foreach (var Prop in Props)
                        {
                            UserArgAdd += $"{LowerVariable}.{Prop.PropName} = Arg.{Prop.PropName};\n";
                        }

                        var XxxService = Templates.BackEnd.XxxService
                            .Replace("#UserArgAdd", $"{UserArgAdd}")
                            .Replace("#UserArgUpdate", $"{UserArgAdd}")
                            .Replace("#UsersService", $"{type.Name}sService")
                            .Replace("#IUsersService", $"I{type.Name}sService")
                            .Replace("#CreateUserArgs", $"Create{type.Name}Args")
                            .Replace("#ReadUserArgs", $"Read{type.Name}Args")
                            .Replace("#UpdateUserArgs", $"Update{type.Name}Args")
                            .Replace("#user", $"{LowerVariable}")
                            .Replace("#User", $"{type.Name}")
                            ;

                        FilePath = $@"{ServicesRoot}\{type.Name}sService.cs";

                        File.WriteAllText(FilePath, XxxService, Encoding.UTF8);
                    }

                    // XxxModel.cs
                    {
                        string CreateUserArgsModel = "";
                        foreach (var Prop in Props)
                        {
                            CreateUserArgsModel += $"public {Prop.PropType} {Prop.PropName} {{ get;set; }} \n";
                        }

                        var XxxModel = Templates.BackEnd.XxxModel
                            .Replace("#CreateUserArgsModel", $"{CreateUserArgsModel}")
                            .Replace("#UpdateUserArgsModel", $"{CreateUserArgsModel}")
                            .Replace("#ReadUserArgsModel", $"")
                            .Replace("#User", $"{type.Name}")
                            .Replace("#CreateUserArgs", $"Create{type.Name}Args")
                            .Replace("#UpdateUserArgs", $"Update{type.Name}Args")
                            .Replace("#ReadUserArgs", $"Read{type.Name}Args")
                            ;

                        FilePath = $@"{ModelsRoot}\{type.Name}sModel.cs";

                        File.WriteAllText(FilePath, XxxModel, Encoding.UTF8);
                    }

                    // AddScoped
                    ProgramAddScoped += Templates.BackEnd.ProgramAddScoped
                            .Replace("#IUsersService", $"I{type.Name}sService")
                            .Replace("#CreateUserArgs", $"Create{type.Name}Args")
                            .Replace("#ReadUserArgs", $"Read{type.Name}Args")
                            .Replace("#UpdateUserArgs", $"Update{type.Name}Args")
                            .Replace("#UsersService", $"{type.Name}sService")
                            + "\n";
                    ;
                }
            }

            // ProgramAddScoped.cs
            File.WriteAllText($@"{BackEndRoot}\ProgramAddScoped.txt", ProgramAddScoped, Encoding.UTF8);

            #endregion

            #region 前端

            // 前端目錄
            var FrontEndRoot = $@"{ScriptRoot}\FrontEnd";
            Method.CreateWithoutDirectory(FrontEndRoot);

            var ComponentsRoot = $@"{FrontEndRoot}\Components";
            Method.CreateWithoutDirectory(ComponentsRoot);

            var TsModelsRoot = $@"{FrontEndRoot}\Models";
            Method.CreateWithoutDirectory(TsModelsRoot);

            var ApiServices = "";

            // Script
            foreach (var type in types)
            {
                // xxx-table 目錄
                var ComponentRoot = $@"{FrontEndRoot}\Components\{type.Name.ToLower()}s-table";
                Method.CreateWithoutDirectory(ComponentRoot);

                // xxx-table.component.html
                // xxx-table.component.scss
                // xxx-table.component.spec.ts
                // xxx-table.component.ts

                // XxxModel.ts

                // api.service.ts
            }

            // api.service.ts
            File.WriteAllText($@"{FrontEndRoot}\api.service.ts", ApiServices, Encoding.UTF8);

            #endregion
        }
    }

    public static class Templates
    {
        public static class BackEnd
        {
            public static string XxxController = @"

        using Microsoft.AspNetCore.Mvc;
        using Common.Model;
        using BLL.IServices;
        using Common.Model.AdminSide;

        namespace API.Controllers.AdminSide
        {
           
            [Route(""api/[controller]/[action]"")]
            [ApiController]
            public class #UsersController : Controller,ICrudController<List<#CreateUserArgs>, List<#ReadUserArgs>, List<#UpdateUserArgs>, List<int?>>
            {
                private #IUsersService<List<#CreateUserArgs>, List<#ReadUserArgs>, List<#UpdateUserArgs>, List<int?>> _#UserService;

                public #UsersController(
                    #IUsersService<List<#CreateUserArgs>, List<#ReadUserArgs>, List<#UpdateUserArgs>, List<int?>> #IUsersService
                )
                {
                    _#UserService = #IUsersService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<#CreateUserArgs>> Req)
                {
                    return await _#UserService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _#UserService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<#ReadUserArgs>> Req)
                {
                    return await _#UserService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<#UpdateUserArgs>> Req)
                {
                    return await _#UserService.Update(Req);
                }
            }
        }
";

            public static string IXxxService = @"
    namespace BLL.IServices
    {
        public interface #IUsersService<CreateArgs, ReadArgs, UpdateArgs, DeleteArgs> : ICrudService<CreateArgs, ReadArgs, UpdateArgs, DeleteArgs> { }
    }
";

            public static string XxxService = @"

        using BLL.IServices;
        using Common.Model;
        using DPL.EF;
        using Common.Enum;
        using Common.Model.AdminSide;
        using Newtonsoft.Json;

        namespace BLL.Services.AdminSide
        {
            public class #UsersService : #IUsersService<
                    List<#CreateUserArgs>,
                    List<#ReadUserArgs>,
                    List<#UpdateUserArgs>,
                    List<int?>
                >
            {
                    private readonly CashFlowDbContext _CashFlowDbContext;

                    public #UsersService(CashFlowDbContext cashFlowDbContext)
                    {
                            _CashFlowDbContext = cashFlowDbContext;
                    }

                    public async Task<ApiResponse> Create(ApiRequest<List<#CreateUserArgs>> Req)
                    {
                            var #users = new List<#User>();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var #user = new #User();        

                                #UserArgAdd

                                #users.Add(#user);
                            }

                            _CashFlowDbContext.AddRange(#users);
                            _CashFlowDbContext.SaveChanges();
                            // 不做銷毀 Dispose 動作，交給 DI 容器處理

                            // 此處 SaveChanges 後 SQL Server 會 Tracking 回傳新增後的 Id
                            SussList = #users.Select(x => x.Id).ToList();

                            var Res = new ApiResponse();
                            Res.Data = $@""已新增以下筆數(Id)：[{string.Join(',', SussList)}]"";
                            Res.Success = true;
                            Res.Code = (int) ResponseStatusCode.Success;
                            Res.Message = ""成功新增"";

                            return Res;
                    }

                    public async Task<ApiResponse> Read(ApiRequest<List<#ReadUserArgs>> Req)
                    {
                            var Res = new ApiResponse();
                            var #users = _CashFlowDbContext.#Users.AsQueryable();

                            foreach (var Arg in Req.Args)
                            {
                                if (Arg.Key == ""Id"") // Id 篩選條件
                                {
                                    var Ids = JsonConvert
                                            .DeserializeObject<List<int>>(Arg.JsonString);

                                    #users = #users.Where(x => Ids.Contains(x.Id));
                                }

                                if (Arg.Key == ""Status"") // 狀態篩選條件
                                {
                                    var Status = JsonConvert
                                               .DeserializeObject<byte>(Arg.JsonString);

                                    #users = #users.Where(x => x.Status == Status);
                                }
                            }

                            var Data = #users
                            // 後端分頁
                            // 省略幾筆 ( 頁數 * 每頁幾筆 )
                            .Skip(((int)Req.PageIndex -1) * (int)Req.PageSize)
                            // 取得幾筆，
                            .Take((int)Req.PageSize)
                            .ToList();

                            Res.Data = Data;
                            Res.Success = true;
                            Res.Code = (int)ResponseStatusCode.Success;
                            Res.Message = ""成功讀取"";
                            Res.TotalDataCount = #users.ToList().Count;

                            return Res;
                    }

                    public async Task<ApiResponse> Update(ApiRequest<List<#UpdateUserArgs>> Req)
                    {
                            var Res = new ApiResponse();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var #user = _CashFlowDbContext.#Users
                                         .FirstOrDefault(x => x.Id == Arg.Id);

                                if (#user == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message += $@""Id：{Arg.Id} 無此Id\n"";
                                }
                                else
                                {
                                    #UserArgUpdate                                    

                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(#user.Id);
                                }
                            }

                            Res.Data = $@""SussList：[{string.Join(',', SussList)}]"";
                            Res.Success = true;
                            Res.Code = (int)ResponseStatusCode.Success;
                            Res.Message = ""成功更改"";

                            return Res;
                    }

                    public async Task<ApiResponse> Delete(ApiRequest<List<int?>> Req)
                    {
                            var Res = new ApiResponse();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var #user = _CashFlowDbContext.#Users
                                         .FirstOrDefault(x => x.Id == Arg);

                                if (#user == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message = ""無此Id"";
                                }
                                else
                                {
                                    _CashFlowDbContext.#Users.Remove(#user);
                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(#user.Id);
                                }
                            }

                            Res.Data = $@""SussList：[{string.Join(',', SussList)}]"";
                            Res.Success = true;
                            Res.Code = (int)ResponseStatusCode.Success;
                            Res.Message = ""成功刪除"";

                            return Res;
                    }

            }
        }

";

            public static string XxxModel = @"

        using DPL.EF;
        using System;
        using System.Collections.Generic;
        using System.ComponentModel.DataAnnotations;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;

        namespace Common.Model.AdminSide
        {
            public class #CreateUserArgs : #User
            {
                #CreateUserArgsModel
            }

            public class #UpdateUserArgs : #User
            {
                #UpdateUserArgsModel
            }

            /// <summary>
            /// 查詢條件
            /// </summary>
            public class #ReadUserArgs
            {
                public string Key { get; set; } = ""Id"";
                public string JsonString { get; set; } = ""[1,2,3]"";
            }
        }

";

            public static string ProgramAddScoped = @"

        builder.Services.AddScoped<
            #IUsersService<List<#CreateUserArgs>, List<#ReadUserArgs>, List<#UpdateUserArgs>, List<int?>>,
            #UsersService
        >();

";
        }

        public static class FrontEnd
        {
            public static string xxxhtml = @"

<div class=""UsersTable"">
    <!-- ------------------------------- -->
    <div class=""TableHeader"">
      <mat-form-field appearance=""standard"">
        <mat-label>全欄位前端搜尋</mat-label>
        <input matInput (keyup)=""ApplyFilterFrontEnd($event)"" placeholder=""Ex. 樸續俊"" #filter>
      </mat-form-field>
      <button color=""accent"" mat-raised-button mat-icon-button (click)=""OpenDiaglog(true,null)"">
        <mat-icon>add circle</mat-icon>
      </button>
    </div>
    <!-- ------------------------------- -->
    <div class=""mat-elevation-z8"">
      <!-- ------------------------------- -->
      <table mat-table [dataSource]=""UsersData"">
        <!-- Position Column -->
        <ng-container matColumnDef=""User"">
          <th mat-header-cell *matHeaderCellDef class=""text-center"" color=""primary""> 編號 </th>
          <td mat-cell *matCellDef=""let element"" class=""text-center""> {{element.Id}} </td>
        </ng-container>
  
        <!-- Name Column -->
        <ng-container matColumnDef=""Account"">
          <th mat-header-cell *matHeaderCellDef class=""text-center"" mat-sort-header> 帳號 </th>
          <td mat-cell *matCellDef=""let element"" class=""text-center""> {{element.Email}} </td>
        </ng-container>
  
        <!-- Weight Column -->
        <ng-container matColumnDef=""Name"">
          <th mat-header-cell *matHeaderCellDef class=""text-center"" mat-sort-header> 暱稱 </th>
          <td mat-cell *matCellDef=""let element"" class=""text-center""> {{element.Name}} </td>
        </ng-container>
  
        <!-- Symbol Column -->
        <ng-container matColumnDef=""Status"">
          <th mat-header-cell *matHeaderCellDef class=""text-center"" mat-sort-header> 狀態 </th>
          <td mat-cell *matCellDef=""let element"" class=""text-center""> {{element.Status}} </td>
        </ng-container>
  
        <!-- <mat-form-field class=""example-full-width"" appearance=""fill""> -->
        <!-- <mat-label>Email</mat-label>
        <input type=""email"" matInput [formControl]=""emailFormControl"" [errorStateMatcher]=""matcher""
               placeholder=""Ex. pat@example.com""> -->
        <!-- <mat-hint>Errors appear instantly!</mat-hint>
        <mat-error *ngIf=""emailFormControl.hasError('email') && !emailFormControl.hasError('required')"">
          Please enter a valid email address
        </mat-error>
        <mat-error *ngIf=""emailFormControl.hasError('required')"">
          Email is <strong>required</strong>
        </mat-error> -->
        <!-- </mat-form-field> -->
  
        <!-- ------------------------------- -->
        <ng-container matColumnDef=""button"">
          <th mat-header-cell *matHeaderCellDef class=""text-center""> 管理 </th>
          <td mat-cell *matCellDef=""let element"" class=""text-center"">
            <button mat-raised-button mat-icon-button color=""primary"" (click)=""OpenDiaglog(false,element.Id)"">
              <mat-icon>edit</mat-icon>
            </button>
            <button mat-raised-button mat-icon-button color=""warn"" class=""ml-5"" (click)=""OpenCloseDialog(element.Id)"">
              <mat-icon>delete</mat-icon>
            </button>
          </td>
        </ng-container>
  
        <!-- ------------------------------- -->
        <tr mat-header-row *matHeaderRowDef=""['User', 'Account', 'Name', 'Status','button']""></tr>
        <tr mat-row *matRowDef=""let row; columns:['User', 'Account', 'Name', 'Status','button']""></tr>
  
        <!-- Row shown when there is no matching data. -->
        <!-- <tr class=""mat-row"" *matNoDataRow>
          <td class=""mat-cell"" colspan=""4"">No data matching the filter ""{{input.value}}""</td>
        </tr> -->
  
      </table>
      <!-- ------------------------------- -->
      <mat-paginator #paginator [length]=""totalDataCount"" [pageIndex]=""pageIndex"" [pageSize]=""pageSize""
        [pageSizeOptions]=""[5, 10, 15]"">
      </mat-paginator>
  
    </div>
  
    <!-- ------------------------------- -->
    <!-- Dialog template #2 -->
    <ng-template #Dialog>
      <h2 matDialogTitle>新增資料</h2>
  
      <mat-dialog-content class=""Dialog"">
        <form>
  
          <mat-form-field class=""full-width"" appearance=""fill"">
            <mat-label>帳號</mat-label>
            <input matInput placeholder=""Ex. abc@gmail.com"" [(ngModel)]=""UserData.Email"" type=""email"" name=""email""
              required>
          </mat-form-field>
          <mat-form-field class=""full-width"" appearance=""fill"">
            <mat-label>暱稱</mat-label>
            <input matInput placeholder=""社畜1234"" [(ngModel)]=""UserData.Name"" type=""text"" name=""Name"" required>
          </mat-form-field>
          <mat-form-field class=""full-width"" appearance=""fill"">
            <mat-label color=""warn"">密碼(盡量不做更改)</mat-label>
            <input matInput placeholder=""abc123"" [(ngModel)]=""UserData.Password"" type=""text"" name=""Password"" required>
          </mat-form-field>
          <mat-form-field class=""full-width"" appearance=""fill"">
            <mat-label>狀態(Number)</mat-label>
            <input matInput [(ngModel)]=""UserData.Status"" type=""number"" name=""Status"" required>
          </mat-form-field>
          <mat-form-field class=""full-width"" appearance=""fill"">
            <mat-label>權限角色(Number)</mat-label>
            <input matInput [(ngModel)]=""UserData.RoleId"" type=""number"" name=""RoleId"" required>
          </mat-form-field>
        </form>
      </mat-dialog-content>
  
      <!-- ------------------------------- -->
      <mat-dialog-actions align=""end"">
        <button mat-raised-button mat-icon-button color=""primary"" type=""submit"" (click)=""UserCreate()""
          *ngIf=""!UserData.Id"">
          <mat-icon>done</mat-icon>
        </button>
        <button mat-raised-button mat-icon-button color=""primary"" type=""submit"" (click)=""UserUpdate()""
          *ngIf=""UserData.Id"">
          <mat-icon>done</mat-icon>
        </button>
        <button mat-raised-button mat-icon-button matDialogClose color=""warn"">
          <mat-icon>close</mat-icon>
        </button>
      </mat-dialog-actions>
  
    </ng-template>
  
    <!-- ------------------------------- -->
    <ng-template #CloseDialog>
      <h2 matDialogTitle color=""warn"">即將刪除</h2>
      <mat-dialog-content>
        確定要刪除 ID: {{UserData.Id}} 的用戶資料嗎?
      </mat-dialog-content>
      <mat-dialog-actions align=""end"">
        <button mat-raised-button mat-icon-button color=""warn"" type=""submit"" (click)=""UserDelete(UserData.Id)"">
          <mat-icon>done</mat-icon>
        </button>
        <button mat-raised-button mat-icon-button matDialogClose color=""primary"">
          <mat-icon>close</mat-icon>
        </button>
      </mat-dialog-actions>
    </ng-template>
    <!-- ------------------------------- -->
  
  </div>
  

";

            public static string xxxspec = @"

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsersTableComponent } from './users-table.component';

describe('UserTableComponent', () => {
  let component: UsersTableComponent;
  let fixture: ComponentFixture<UsersTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UsersTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UsersTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

";

            public static string xxxts = @"

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
    this.UsersRead(this.pageIndex, this.pageSize, []);
  }

  ngAfterViewInit() {
    this.UsersData.paginator = this.paginator;
    this.paginator.page.subscribe((page: PageEvent) => {
      this.pageIndex = page.pageIndex;
      this.pageSize = page.pageSize;
      this.UsersRead(page.pageIndex, page.pageSize, []);
    }, (err: any) => {
      console.log(err);
    });
  }

  @ViewChild('Dialog', { static: true }) Dialog: TemplateRef<any> | any;
  DialogRef: MatDialogRef<any> | any;
  OpenDiaglog(IsNew: boolean, Id: any) {
    if (IsNew) {
      this.UserData = new UserArgs();
    } else {
      // 取單筆
      let listInt = [Id];
      let Arg =
      {
        ""Key"": ""Id"",
        ""JsonString"": JSON.stringify(listInt)
      };
      this.UsersRead(0, 5, [Arg]);
    }
    this.DialogRef = this.dialog.open(this.Dialog);
  }

  @ViewChild('CloseDialog', { static: true }) CloseDialog: TemplateRef<any> | any;
  CloseDialogRef: MatDialogRef<any> | any;
  OpenCloseDialog(Id: any) {
    this.CloseDialogRef = this.dialog.open(this.CloseDialog);
    this.UserData = new UserArgs();
    this.UserData.Id = Id;
  }

  UsersData: any;
  FilterUsersData: any;
  UserData = new UserArgs();
  UsersRead(PageIndex: any, PageSize: any, Args: any) {
    let Req = new ApiRequest<any>();
    Req.Args = Args;
    Req.PageIndex = PageIndex;
    Req.PageSize = PageSize;
    this._ApiService.UsersRead(Req).subscribe((Res) => {
      if (Res.Success) {
        if (Args.length <= 0) {
          this.UsersData = Res.Data;
          this.FilterUsersData = Res.Data;
          this.totalDataCount = Res.TotalDataCount;
        }
        else {
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
        this.UsersRead(this.pageIndex, this.pageSize, []);
        this.CloseDialogRef.close();
      }
    }, (err: any) => {
      console.log(err);
    });
  }

  UserUpdate() {
    console.log(this.UserData.Id);
    let Req = new ApiRequest();
    Req.Args = [this.UserData];
    this._ApiService.UserUpdate(Req).subscribe((Res) => {
      if (Res.Success) {
        console.log(Res);
        this.DialogRef.close();
        this.UsersRead(this.pageIndex, this.pageSize, []);
      }
    }, (err: any) => {
      console.log(err);
    });
  }

  UserCreate() {
    console.log(this.UserData.Id);
    let Req = new ApiRequest();
    Req.Args = [this.UserData];
    this._ApiService.UserCreate(Req).subscribe((Res) => {
      if (Res.Success) {
        console.log(Res);
        this.DialogRef.close();
        this.UsersRead(this.pageIndex, this.pageSize, []);
      }
    }, (err: any) => {
      console.log(err);
    });
  }

}


";
            public static string xxxmodelts = @"

        export class #UserArgs {
                #Props
        }
";
        }
    }
}
