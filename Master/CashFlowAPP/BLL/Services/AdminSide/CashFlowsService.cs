

using BLL.IServices;
using Common.Model;
using DPL.EF;
using Common.Enum;
using Common.Model.AdminSide;
using Newtonsoft.Json;

namespace BLL.Services.AdminSide
{
    public class CashFlowsService : ICashFlowsService<
            List<CreateCashFlowArgs>,
            List<ReadCashFlowArgs>,
            List<UpdateCashFlowArgs>,
            List<int?>
        >
    {
        private readonly CashFlowDbContext _CashFlowDbContext;

        public CashFlowsService(CashFlowDbContext cashFlowDbContext)
        {
            _CashFlowDbContext = cashFlowDbContext;
        }

        public async Task<ApiResponse> Create(ApiRequest<List<CreateCashFlowArgs>> Req)
        {
            var cashFlows = new List<CashFlow>();

            var SussList = new List<int>();

            foreach (var Arg in Req.Args)
            {
                var cashFlow = new CashFlow();
                cashFlow.Id = Arg.Id;
                cashFlow.Name = Arg.Name;
                cashFlow.Value = Arg.Value;
                cashFlow.CashFlowCategoryId = Arg.CashFlowCategoryId;
                cashFlow.Status = Arg.Status;
                cashFlow.Description = Arg.Description;
                cashFlows.Add(cashFlow);
            }

            _CashFlowDbContext.AddRange(cashFlows);
            _CashFlowDbContext.SaveChanges();
            // 不做銷毀 Dispose 動作，交給 DI 容器處理

            // 此處 SaveChanges 後 SQL Server 會 Tracking 回傳新增後的 Id
            SussList = cashFlows.Select(x => x.Id).ToList();

            var Res = new ApiResponse();
            Res.Data = $@"已新增以下筆數(Id)：[{string.Join(',', SussList)}]";
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "成功新增";

            return Res;
        }

        public async Task<ApiResponse> Read(ApiRequest<List<ReadCashFlowArgs>> Req)
        {
            var Res = new ApiResponse();
            var cashFlows = _CashFlowDbContext.CashFlows.AsQueryable();

            foreach (var Arg in Req.Args)
            {
                if (Arg.Key == "Id") // Id 篩選條件
                {
                    var Ids = JsonConvert
                            .DeserializeObject<List<int>>(Arg.JsonString);

                    cashFlows = cashFlows.Where(x => Ids.Contains(x.Id));
                }

                if (Arg.Key == "Status") // 狀態篩選條件
                {
                    var Status = JsonConvert
                               .DeserializeObject<byte>(Arg.JsonString);

                    cashFlows = cashFlows.Where(x => x.Status == Status);
                }
            }

            var CashFlows = cashFlows
            // 後端分頁
            // 省略幾筆 ( 頁數 * 每頁幾筆 )
            .Skip(((int)Req.PageIndex - 1) * (int)Req.PageSize)
            // 取得幾筆，
            .Take((int)Req.PageSize)
            // 因為外鍵會導致JSON無限階層，只好選沒外鍵的資料行
            .Select(x => new {x.Id,x.Name,x.Value,x.Description,x.Status})
            .ToList();

            var CashFlowCategorys = _CashFlowDbContext.CashFlowCategories.Select(x => new {x.Id,x.Name}).ToList();
            // 因為外鍵會導致JSON無限階層，只好選沒外鍵的資料行
            Res.Data = new { CashFlows, CashFlowCategorys };
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "成功讀取";
            Res.TotalDataCount = cashFlows.ToList().Count;

            return Res;
        }

        public async Task<ApiResponse> Update(ApiRequest<List<UpdateCashFlowArgs>> Req)
        {
            var Res = new ApiResponse();

            var SussList = new List<int>();

            foreach (var Arg in Req.Args)
            {
                var cashFlow = _CashFlowDbContext.CashFlows
                         .FirstOrDefault(x => x.Id == Arg.Id);

                if (cashFlow == null)
                {
                    Res.Success = false;
                    Res.Code = (int)ResponseStatusCode.CannotFind;
                    Res.Message += $@"Id：{Arg.Id} 無此Id\n";
                }
                else
                {
                    cashFlow.Id = Arg.Id;
                    cashFlow.Name = Arg.Name;
                    cashFlow.Value = Arg.Value;
                    cashFlow.CashFlowCategoryId = Arg.CashFlowCategoryId;
                    cashFlow.Status = Arg.Status;
                    cashFlow.Description = Arg.Description; 

                    _CashFlowDbContext.SaveChanges();
                    SussList.Add(cashFlow.Id);
                }
            }

            Res.Data = $@"SussList：[{string.Join(',', SussList)}]";
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "成功更改";

            return Res;
        }

        public async Task<ApiResponse> Delete(ApiRequest<List<int?>> Req)
        {
            var Res = new ApiResponse();

            var SussList = new List<int>();

            foreach (var Arg in Req.Args)
            {
                var cashFlow = _CashFlowDbContext.CashFlows
                         .FirstOrDefault(x => x.Id == Arg);

                if (cashFlow == null)
                {
                    Res.Success = false;
                    Res.Code = (int)ResponseStatusCode.CannotFind;
                    Res.Message = "無此Id";
                }
                else
                {
                    _CashFlowDbContext.CashFlows.Remove(cashFlow);
                    _CashFlowDbContext.SaveChanges();
                    SussList.Add(cashFlow.Id);
                }
            }

            Res.Data = $@"SussList：[{string.Join(',', SussList)}]";
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "成功刪除";

            return Res;
        }

    }
}

