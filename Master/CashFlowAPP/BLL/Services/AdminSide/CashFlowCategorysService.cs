

        using BLL.IServices;
        using Common.Model;
        using DPL.EF;
        using Common.Enum;
        using Common.Model.AdminSide;
        using Newtonsoft.Json;

        namespace BLL.Services.AdminSide
        {
            public class CashFlowCategorysService : ICashFlowCategorysService<
                    List<CreateCashFlowCategoryArgs>,
                    List<ReadCashFlowCategoryArgs>,
                    List<UpdateCashFlowCategoryArgs>,
                    List<int?>
                >
            {
                    private readonly CashFlowDbContext _CashFlowDbContext;

                    public CashFlowCategorysService(CashFlowDbContext cashFlowDbContext)
                    {
                            _CashFlowDbContext = cashFlowDbContext;
                    }

                    public async Task<ApiResponse> Create(ApiRequest<List<CreateCashFlowCategoryArgs>> Req)
                    {
                            var cashFlowCategorys = new List<CashFlowCategory>();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var cashFlowCategory = new CashFlowCategory();        

                                cashFlowCategory.Id = Arg.Id;
cashFlowCategory.Name = Arg.Name;
cashFlowCategory.ParentId = Arg.ParentId;
cashFlowCategory.Time = Arg.Time;
cashFlowCategory.Type = Arg.Type;
cashFlowCategory.Status = Arg.Status;


                                cashFlowCategorys.Add(cashFlowCategory);
                            }

                            _CashFlowDbContext.AddRange(cashFlowCategorys);
                            _CashFlowDbContext.SaveChanges();
                            // 不做銷毀 Dispose 動作，交給 DI 容器處理

                            // 此處 SaveChanges 後 SQL Server 會 Tracking 回傳新增後的 Id
                            SussList = cashFlowCategorys.Select(x => x.Id).ToList();

                            var Res = new ApiResponse();
                            Res.Data = $@"已新增以下筆數(Id)：[{string.Join(',', SussList)}]";
                            Res.Success = true;
                            Res.Code = (int) ResponseStatusCode.Success;
                            Res.Message = "成功新增";

                            return Res;
                    }

                    public async Task<ApiResponse> Read(ApiRequest<List<ReadCashFlowCategoryArgs>> Req)
                    {
                            var Res = new ApiResponse();
                            var cashFlowCategorys = _CashFlowDbContext.CashFlowCategories.AsQueryable();

                            foreach (var Arg in Req.Args)
                            {
                                if (Arg.Key == "Id") // Id 篩選條件
                                {
                                    var Ids = JsonConvert
                                            .DeserializeObject<List<int>>(Arg.JsonString);

                                    cashFlowCategorys = cashFlowCategorys.Where(x => Ids.Contains(x.Id));
                                }

                                if (Arg.Key == "Status") // 狀態篩選條件
                                {
                                    var Status = JsonConvert
                                               .DeserializeObject<byte>(Arg.JsonString);

                                    cashFlowCategorys = cashFlowCategorys.Where(x => x.Status == Status);
                                }
                            }

                            var Data = cashFlowCategorys
                            // 後端分頁
                            // 省略幾筆 ( 頁數 * 每頁幾筆 )
                            .Skip(((int)Req.PageIndex -1) * (int)Req.PageSize)
                            // 取得幾筆，
                            .Take((int)Req.PageSize)
                            .ToList();

                            Res.Data = Data;
                            Res.Success = true;
                            Res.Code = (int)ResponseStatusCode.Success;
                            Res.Message = "成功讀取";
                            Res.TotalDataCount = cashFlowCategorys.ToList().Count;

                            return Res;
                    }

                    public async Task<ApiResponse> Update(ApiRequest<List<UpdateCashFlowCategoryArgs>> Req)
                    {
                            var Res = new ApiResponse();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var cashFlowCategory = _CashFlowDbContext.CashFlowCategories
                                         .FirstOrDefault(x => x.Id == Arg.Id);

                                if (cashFlowCategory == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message += $@"Id：{Arg.Id} 無此Id\n";
                                }
                                else
                                {
                                    cashFlowCategory.Id = Arg.Id;
cashFlowCategory.Name = Arg.Name;
cashFlowCategory.ParentId = Arg.ParentId;
cashFlowCategory.Time = Arg.Time;
cashFlowCategory.Type = Arg.Type;
cashFlowCategory.Status = Arg.Status;
                                    

                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(cashFlowCategory.Id);
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
                                var cashFlowCategory = _CashFlowDbContext.CashFlowCategories
                                         .FirstOrDefault(x => x.Id == Arg);

                                if (cashFlowCategory == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message = "無此Id";
                                }
                                else
                                {
                                    _CashFlowDbContext.CashFlowCategories.Remove(cashFlowCategory);
                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(cashFlowCategory.Id);
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

