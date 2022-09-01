

using BLL.IServices;
using Common.Model;
using DPL.EF;
using Common.Enum;
using Common.Model.AdminSide;
using Newtonsoft.Json;

namespace BLL.Services.AdminSide
{
    public class CardsService : ICardsService<
            List<CreateCardArgs>,
            List<ReadCardArgs>,
            List<UpdateCardArgs>,
            List<int?>
        >
    {
        private readonly CashFlowDbContext _CashFlowDbContext;

        public CardsService(CashFlowDbContext cashFlowDbContext)
        {
            _CashFlowDbContext = cashFlowDbContext;
        }

        public async Task<ApiResponse> Create(ApiRequest<List<CreateCardArgs>> Req)
        {
            var cards = new List<Card>();

            var SussList = new List<int>();

            foreach (var Arg in Req.Args)
            {
                var card = new Card();

                card.Id = Arg.Id;
                card.Name = Arg.Name;
                card.Status = Arg.Status;
                card.Weight = Arg.Weight;

                cards.Add(card);
            }

            _CashFlowDbContext.AddRange(cards);
            _CashFlowDbContext.SaveChanges();
            // 不做銷毀 Dispose 動作，交給 DI 容器處理

            // 此處 SaveChanges 後 SQL Server 會 Tracking 回傳新增後的 Id
            SussList = cards.Select(x => x.Id).ToList();

            var Res = new ApiResponse();
            Res.Data = $@"已新增以下筆數(Id)：[{string.Join(',', SussList)}]";
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "成功新增";

            return Res;
        }

        public async Task<ApiResponse> Read(ApiRequest<List<ReadCardArgs>> Req)
        {
            var Res = new ApiResponse();
            var cards = _CashFlowDbContext.Cards.AsQueryable();

            foreach (var Arg in Req.Args)
            {
                if (Arg.Key == "Id") // Id 篩選條件
                {
                    var Ids = JsonConvert
                            .DeserializeObject<List<int>>(Arg.JsonString);

                    cards = cards.Where(x => Ids.Contains(x.Id));
                }

                if (Arg.Key == "Status") // 狀態篩選條件
                {
                    var Status = JsonConvert
                               .DeserializeObject<byte>(Arg.JsonString);

                    cards = cards.Where(x => x.Status == Status);
                }
            }

            var Data = cards
            // 後端分頁
            // 省略幾筆 ( 頁數 * 每頁幾筆 )
            .Skip(((int)Req.PageIndex - 1) * (int)Req.PageSize)
            // 取得幾筆，
            .Take((int)Req.PageSize)
            .ToList();

            Res.Data = Data;
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "成功讀取";
            Res.TotalDataCount = cards.ToList().Count;

            return Res;
        }

        public async Task<ApiResponse> Update(ApiRequest<List<UpdateCardArgs>> Req)
        {
            var Res = new ApiResponse();

            var SussList = new List<int>();

            foreach (var Arg in Req.Args)
            {
                var card = _CashFlowDbContext.Cards
                         .FirstOrDefault(x => x.Id == Arg.Id);

                if (card == null)
                {
                    Res.Success = false;
                    Res.Code = (int)ResponseStatusCode.CannotFind;
                    Res.Message += $@"Id：{Arg.Id} 無此Id\n";
                }
                else
                {
                    card.Id = Arg.Id;
                    card.Name = Arg.Name;
                    card.Status = Arg.Status;
                    card.Weight = Arg.Weight;

                    _CashFlowDbContext.SaveChanges();
                    SussList.Add(card.Id);
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
                var card = _CashFlowDbContext.Cards
                         .FirstOrDefault(x => x.Id == Arg);

                if (card == null)
                {
                    Res.Success = false;
                    Res.Code = (int)ResponseStatusCode.CannotFind;
                    Res.Message = "無此Id";
                }
                else
                {
                    _CashFlowDbContext.Cards.Remove(card);
                    _CashFlowDbContext.SaveChanges();
                    SussList.Add(card.Id);
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

