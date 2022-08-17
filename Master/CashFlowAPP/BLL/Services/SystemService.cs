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

                    var FilePath = $@"{ControllersRoot}\{type.Name}sController.cs";

                    File.WriteAllText(FilePath, XxxController, Encoding.UTF8);

                    // IXxxService.cs
                    var IXxxService = Templates.BackEnd.IXxxService
                        .Replace("#IUsersService", $"I{type.Name}sService")
                        ;

                    FilePath = $@"{IServicesRoot}\{type.Name}sService.cs";

                    File.WriteAllText(FilePath, IXxxService, Encoding.UTF8);

                    // XxxService.cs
                    var Properties = type.GetTypeInfo().DeclaredProperties;
                    var Props = Properties.Select(x => new
                    {
                        PropName = x.Name,
                        PropType = Templates
                        .GetSqlDataTypeString(x.PropertyType.FullName, 2)
                    })
                    .ToList();

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
        using Microsoft.EntityFrameworkCore;
        using Newtonsoft.Json;
        using System.Linq;

        namespace BLL.Services.AdminSide
        {
            public class #UsersService :#IUsersService<
                    List<#CreateUserArgs>, 
                    List<#ReadUserArgs>, 
                    List<#UpdateUserArgs>, 
                    List<int?>
            >
            {
                    private readonly CashFlowDbContext _CashFlowDbContext;

                    public UsersService(CashFlowDbContext cashFlowDbContext)
                    {
                            _CashFlowDbContext = cashFlowDbContext;
                    }

                    public async Task<ApiResponse> Create(ApiRequest<List<#CreateUserArgs>> Req)
                    {
                        var users = new List<#User>();

                        var SussList = new List<int>();

                        foreach (var Arg in Req.Args)
                        {
                            var #user = new #User();
                            user.Email = Arg.Email;
                            user.Password = Arg.Password;
                            user.Name = Arg.Name;
                            user.Status = Arg.Status;
                            user.RoleId = Arg.RoleId;
                            #users.Add(#user);
                        }
            
                        _CashFlowDbContext.AddRange(#users);
                        _CashFlowDbContext.SaveChanges();
                        // 不做銷毀 Dispose 動作，交給 DI 容器處理

                        // 此處 SaveChanges 後 SQL Server 會 Tracking 回傳新增後的 Id
                        SussList = #users.Select(x => x.Id).ToList();

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

            public static string XxxModel = @"
";
        }

        public static class FrontEnd
        {
        }

        /// <summary>
        /// 轉換資料型態字串
        /// </summary>
        /// <param name="dataType">資料型態</param>
        /// <param name="returnType">回傳型態 1.SQL型態 2.變數宣告 3.變數預設值 7.Angular型態 8.Angular InputType</param>
        /// <returns></returns>
        public static string GetSqlDataTypeString(string dataType, int returnType)
        {
            string typeClassString = "";
            string typeDetailsString = "";
            string typeDefaultString = "";
            string returnString = "";
            string typeAngularString = "";
            string typeAngularInputString = "";

            switch (dataType)
            {
                case ("System.Byte"):
                    typeClassString = "SqlDbType.TinyInt"; // SQL 型態
                    typeDetailsString = "byte"; // 變數宣告
                    typeDefaultString = "Default.MyByte"; // 變數預設值
                    typeAngularString = "number"; // Angular 型態
                    typeAngularInputString = "number"; // Angular InputType
                    break;
                case ("System.Int16"):
                    typeClassString = "SqlDbType.SmallInt";
                    typeDetailsString = "short";
                    typeDefaultString = "Default.MyShort";
                    typeAngularString = "number";
                    typeAngularInputString = "number";
                    break;
                case ("System.Int32"):
                    typeClassString = "SqlDbType.Int";
                    typeDetailsString = "int";
                    typeDefaultString = "Default.MyInt";
                    typeAngularString = "number";
                    typeAngularInputString = "number";
                    break;
                case ("System.Int64"):
                    typeClassString = "SqlDbType.BigInt";
                    typeDetailsString = "long";
                    typeDefaultString = "Default.MyLong";
                    typeAngularString = "number";
                    typeAngularInputString = "number";
                    break;
                case ("System.String"):
                    typeClassString = "SqlDbType.NVarChar";
                    typeDetailsString = "string";
                    typeDefaultString = "Default.MyString";
                    typeAngularString = "string";
                    typeAngularInputString = "text";
                    break;
                case ("System.Guid"):
                    typeClassString = "SqlDbType.UniqueIdentifier";
                    typeDetailsString = "Guid";
                    typeDefaultString = "Default.MyGuid";
                    typeAngularString = "string";
                    typeAngularInputString = "text";
                    break;
                case ("System.Boolean"):
                    typeClassString = "SqlDbType.Bit";
                    typeDetailsString = "bool";
                    typeDefaultString = "Default.MyBoolean";
                    typeAngularString = "boolean";
                    typeAngularInputString = "number";
                    break;
                case ("System.DateTime"):
                    typeClassString = "SqlDbType.DateTime";
                    typeDetailsString = "DateTime";
                    typeDefaultString = "Default.MyDateTime";
                    typeAngularString = "Date";
                    typeAngularInputString = "Date";
                    break;
                case ("System.Double"):
                    typeClassString = "SqlDbType.Float";
                    typeDetailsString = "double";
                    typeDefaultString = "Default.MyDouble";
                    typeAngularString = "number";
                    typeAngularInputString = "number";
                    break;
                case ("System.Decimal"):
                    typeClassString = "SqlDbType.Decimal";
                    typeDetailsString = "decimal";
                    typeDefaultString = "Default.MyDecimal";
                    typeAngularString = "number";
                    typeAngularInputString = "number";
                    break;
                case ("System.Byte[]"):
                    typeClassString = "SqlDbType.Image";
                    typeDetailsString = "byte[]";
                    typeDefaultString = "Default.MyBytes";
                    typeAngularString = "string";
                    typeAngularInputString = "text";
                    break;

                default:
                    typeClassString = "無法解析";
                    typeDetailsString = "無法解析";
                    typeDefaultString = "無法解析";
                    typeAngularString = "無法解析";
                    typeAngularInputString = "text";
                    break;
            }

            if (returnType == 1)
                returnString = typeClassString;
            else if (returnType == 2)
                returnString = typeDetailsString;
            else if (returnType == 7)
                returnString = typeAngularString;
            else if (returnType == 8)
                returnString = typeAngularInputString;
            else
                returnString = typeDefaultString;

            return returnString;
        }
    }
}
