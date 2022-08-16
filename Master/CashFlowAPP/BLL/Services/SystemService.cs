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

        public void CreateTemplateByTableName(string RootDirectoryPath)
        {
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

            foreach (var type in types)
            {
                if (!type.FullName.ToLower().Contains("context"))
                {
                    // XxxController.cs
                    var XxxController = Templates.BackEnd.XxxController
                        .Replace("#UsersController", $"{type.Name}sController")
                        .Replace("#CreateUserArgs", $"Create{type.Name}Args")
                        .Replace("#ReadUserArgs", $"Read{type.Name}Args")
                        .Replace("#UpdateUserArgs", $"Update{type.Name}Args")
                        .Replace("#UserService", $"{type.Name}sService")
                        .Replace("#IUsersService", $"I{type.Name}sService")
                        ;

                    var FileName = $@"{ControllersRoot}\{type.Name}sController.cs";

                    File.WriteAllText(FileName, XxxController, Encoding.UTF8);

                    // IXxxService.cs
                    var IXxxService = Templates.BackEnd.IXxxService
                        .Replace("#UsersController", $"{type.Name}sController")
                        .Replace("#CreateUserArgs", $"Create{type.Name}Args")
                        .Replace("#ReadUserArgs", $"Read{type.Name}Args")
                        .Replace("#UpdateUserArgs", $"Update{type.Name}Args")
                        .Replace("#UserService", $"{type.Name}sService")
                        .Replace("#IUsersService", $"I{type.Name}sService")
                        ;

                    FileName = $@"{IServicesRoot}\{type.Name}sService.cs";

                    File.WriteAllText(FileName, IXxxService, Encoding.UTF8);

                    // XxxService.cs

                    // XxxModel.cs
                }
            }

            #endregion

            #region 前端

            // 前端目錄
            var FrontEndRoot = $@"{ScriptRoot}\FrontEnd";
            Method.CreateWithoutDirectory(FrontEndRoot);

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
           
            [Route(""api//[controller]/[action]"")]
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

        using BLL.IServices;
        using Common.Model;
        using DPL.EF;
        using Common.Enum;
        using Common.Model.AdminSide;
        using Microsoft.EntityFrameworkCore;
        using Newtonsoft.Json;
        using System.Linq;

        namespace BLL.Services.AdminSide
        {
            public class UsersService :IUsersService<
                    List<CreateUserArgs>, 
                    List<ReadUserArgs>, 
                    List<UpdateUserArgs>, 
                    List<int?>
            >
            {
                    private readonly CashFlowDbContext _CashFlowDbContext;

                    public UsersService(CashFlowDbContext cashFlowDbContext)
                    {
                            _CashFlowDbContext = cashFlowDbContext;
                    }

                    public async Task<ApiResponse> Create(ApiRequest<List<CreateUserArgs>> Req)
                    {
                        var users = new List<User>();

                        var SussList = new List<int>();

                        foreach (var Arg in Req.Args)
                        {
                            var user = new User();
                            user.Email = Arg.Email;
                            user.Password = Arg.Password;
                            user.Name = Arg.Name;
                            user.Status = Arg.Status;
                            user.RoleId = Arg.RoleId;
                            users.Add(user);
                        }
            
                        _CashFlowDbContext.AddRange(users);
                        _CashFlowDbContext.SaveChanges();
                        // 不做銷毀 Dispose 動作，交給 DI 容器處理

                        // 此處 SaveChanges 後 SQL Server 會 Tracking 回傳新增後的 Id
                        SussList = users.Select(x => x.Id).ToList();

                        var Res = new ApiResponse();
                        Res.Data = $@""SussList：[{string.Join(',', SussList)}]"";
                        Res.Success = true;
                        Res.Code = (int) ResponseStatusCode.Success;
                        Res.Message = ""成功新增"";

                        return Res;
                    }

                    public async Task<ApiResponse> Read(ApiRequest<List<ReadUserArgs>> Req)
                    {
                        var Res = new ApiResponse();

                        if (Req.Args.Count() <= 0)
                        {
                            Res.Success = true;
                            Res.Code = (int)ResponseStatusCode.Success;
                            Res.Message = ""成功讀取"";
                            Res.Data = _CashFlowDbContext.Users.ToList();
                        }
                        else
                        {
                            // 查詢不追蹤釋放連線，避免其餘線程衝到
                            var user = _CashFlowDbContext.Users
                                        .AsNoTracking();

                            foreach (var Arg in Req.Args)
                            {
                                if (Arg.Key == ""Id"") // Id 篩選條件
                                {
                                    var Ids = JsonConvert
                                                .DeserializeObject<List<int>>(Arg.JsonString);

                                    user = user.Where(x => Ids.Contains(x.Id));
                                }

                                if (Arg.Key == ""Status"") // 狀態篩選條件
                                {
                                    var Status = JsonConvert
                                                   .DeserializeObject<byte>(Arg.JsonString);

                                    user = user.Where(x => x.Status == Status);
                                }
                             }

                             var Data = user
                            // 後端分頁
                            // 省略幾筆 ( 頁數 * 每頁幾筆 )
                            .Skip(((int)Req.PageIndex - 1) * (int)Req.PageSize)
                            // 取得幾筆
                            .Take((int)Req.PageSize)
                            .ToList();

                            Res.Data = Data;
                            Res.Success = true;
                            Res.Code = (int)ResponseStatusCode.Success;
                            Res.Message = ""成功讀取"";
                        }
        
                        return Res;
                    }

                    public async Task<ApiResponse> Update(ApiRequest<List<UpdateUserArgs>> Req)
                    {
                            var Res = new ApiResponse();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                    var user = _CashFlowDbContext.Users
                                                    .FirstOrDefault(x => x.Id == Arg.Id);

                                    if (user == null)
                                    {
                                        Res.Success = false;
                                        Res.Code = (int)ResponseStatusCode.CannotFind;
                                        Res.Message += $@""Id：{Arg.Id} 無此用戶\n"";
                                    }
                                    else
                                    {
                                        user.Email = Arg.Email;
                                        user.Password = Arg.Password;
                                        user.Name = Arg.Name;
                                        user.Status = Arg.Status;
                                        user.RoleId = Arg.RoleId;
                                        _CashFlowDbContext.SaveChanges();
                                        SussList.Add(user.Id);
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
                                    var user = _CashFlowDbContext.Users
                                                    .FirstOrDefault(x => x.Id == Arg);

                                    if (user == null)
                                    {
                                            Res.Success = false;
                                            Res.Code = (int)ResponseStatusCode.CannotFind;
                                            Res.Message = ""無此用戶"";
                                    }
                                    else
                                    {
                                            _CashFlowDbContext.Users.Remove(user);
                                            _CashFlowDbContext.SaveChanges();
                                            SussList.Add(user.Id);
                                    }
                            }

                            Res.Data = $@""SussList：[{string.Join(',', SussList)}]""; ;
                            Res.Success = true;
                            Res.Code = (int)ResponseStatusCode.Success;
                            Res.Message = ""成功刪除"";

                            return Res;
                    }

             }
        }

";

            public static string XxxService = @"
";

            public static string XxxModel = @"
";
        }

        public static class FrontEnd
        {
        }
    }
}
