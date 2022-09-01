

        using BLL.IServices;
        using Common.Model;
        using DPL.EF;
        using Common.Enum;
        using Common.Model.AdminSide;
        using Newtonsoft.Json;

        namespace BLL.Services.AdminSide
        {
            public class UserBoardsService : IUserBoardsService<
                    List<CreateUserBoardArgs>,
                    List<ReadUserBoardArgs>,
                    List<UpdateUserBoardArgs>,
                    List<int?>
                >
            {
                    private readonly CashFlowDbContext _CashFlowDbContext;

                    public UserBoardsService(CashFlowDbContext cashFlowDbContext)
                    {
                            _CashFlowDbContext = cashFlowDbContext;
                    }

                    public async Task<ApiResponse> Create(ApiRequest<List<CreateUserBoardArgs>> Req)
                    {
                            var userBoards = new List<UserBoard>();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var userBoard = new UserBoard();        

                                userBoard.Id = Arg.Id;
userBoard.TotoalNetProfit = Arg.TotoalNetProfit;
userBoard.Debt = Arg.Debt;
userBoard.Revenue = Arg.Revenue;
userBoard.NetProfit = Arg.NetProfit;
userBoard.UserId = Arg.UserId;


                                userBoards.Add(userBoard);
                            }

                            _CashFlowDbContext.AddRange(userBoards);
                            _CashFlowDbContext.SaveChanges();
                            // 不做銷毀 Dispose 動作，交給 DI 容器處理

                            // 此處 SaveChanges 後 SQL Server 會 Tracking 回傳新增後的 Id
                            SussList = userBoards.Select(x => x.Id).ToList();

                            var Res = new ApiResponse();
                            Res.Data = $@"已新增以下筆數(Id)：[{string.Join(',', SussList)}]";
                            Res.Success = true;
                            Res.Code = (int) ResponseStatusCode.Success;
                            Res.Message = "成功新增";

                            return Res;
                    }

                    public async Task<ApiResponse> Read(ApiRequest<List<ReadUserBoardArgs>> Req)
                    {
                            var Res = new ApiResponse();
                            var userBoards = _CashFlowDbContext.UserBoards.AsQueryable();

                            foreach (var Arg in Req.Args)
                            {
                                if (Arg.Key == "Id") // Id 篩選條件
                                {
                                    var Ids = JsonConvert
                                            .DeserializeObject<List<int>>(Arg.JsonString);

                                    userBoards = userBoards.Where(x => Ids.Contains(x.Id));
                                }

                                if (Arg.Key == "Status") // 狀態篩選條件
                                {
                                    var Status = JsonConvert
                                               .DeserializeObject<byte>(Arg.JsonString);

                                    userBoards = userBoards.Where(x => x.Status == Status);
                                }
                            }

                            var Data = userBoards
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
                            Res.TotalDataCount = userBoards.ToList().Count;

                            return Res;
                    }

                    public async Task<ApiResponse> Update(ApiRequest<List<UpdateUserBoardArgs>> Req)
                    {
                            var Res = new ApiResponse();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var userBoard = _CashFlowDbContext.UserBoards
                                         .FirstOrDefault(x => x.Id == Arg.Id);

                                if (userBoard == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message += $@"Id：{Arg.Id} 無此Id\n";
                                }
                                else
                                {
                                    userBoard.Id = Arg.Id;
userBoard.TotoalNetProfit = Arg.TotoalNetProfit;
userBoard.Debt = Arg.Debt;
userBoard.Revenue = Arg.Revenue;
userBoard.NetProfit = Arg.NetProfit;
userBoard.UserId = Arg.UserId;
                                    

                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(userBoard.Id);
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
                                var userBoard = _CashFlowDbContext.UserBoards
                                         .FirstOrDefault(x => x.Id == Arg);

                                if (userBoard == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message = "無此Id";
                                }
                                else
                                {
                                    _CashFlowDbContext.UserBoards.Remove(userBoard);
                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(userBoard.Id);
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

