

        using BLL.IServices;
        using Common.Model;
        using DPL.EF;
        using Common.Enum;
        using Common.Model.AdminSide;
        using Newtonsoft.Json;

        namespace BLL.Services.AdminSide
        {
            public class CardEffectsService : ICardEffectsService<
                    List<CreateCardEffectArgs>,
                    List<ReadCardEffectArgs>,
                    List<UpdateCardEffectArgs>,
                    List<int?>
                >
            {
                    private readonly CashFlowDbContext _CashFlowDbContext;

                    public CardEffectsService(CashFlowDbContext cashFlowDbContext)
                    {
                            _CashFlowDbContext = cashFlowDbContext;
                    }

                    public async Task<ApiResponse> Create(ApiRequest<List<CreateCardEffectArgs>> Req)
                    {
                            var cardEffects = new List<CardEffect>();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var cardEffect = new CardEffect();        

                                cardEffect.Id = Arg.Id;
cardEffect.TableId = Arg.TableId;
cardEffect.Description = Arg.Description;
cardEffect.CardId = Arg.CardId;
cardEffect.EffectTableId = Arg.EffectTableId;


                                cardEffects.Add(cardEffect);
                            }

                            _CashFlowDbContext.AddRange(cardEffects);
                            _CashFlowDbContext.SaveChanges();
                            // 不做銷毀 Dispose 動作，交給 DI 容器處理

                            // 此處 SaveChanges 後 SQL Server 會 Tracking 回傳新增後的 Id
                            SussList = cardEffects.Select(x => x.Id).ToList();

                            var Res = new ApiResponse();
                            Res.Data = $@"已新增以下筆數(Id)：[{string.Join(',', SussList)}]";
                            Res.Success = true;
                            Res.Code = (int) ResponseStatusCode.Success;
                            Res.Message = "成功新增";

                            return Res;
                    }

                    public async Task<ApiResponse> Read(ApiRequest<List<ReadCardEffectArgs>> Req)
                    {
                            var Res = new ApiResponse();
                            var cardEffects = _CashFlowDbContext.CardEffects.AsQueryable();

                            foreach (var Arg in Req.Args)
                            {
                                if (Arg.Key == "Id") // Id 篩選條件
                                {
                                    var Ids = JsonConvert
                                            .DeserializeObject<List<int>>(Arg.JsonString);

                                    cardEffects = cardEffects.Where(x => Ids.Contains(x.Id));
                                }
                            }

                            var Data = cardEffects
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
                            Res.TotalDataCount = cardEffects.ToList().Count;

                            return Res;
                    }

                    public async Task<ApiResponse> Update(ApiRequest<List<UpdateCardEffectArgs>> Req)
                    {
                            var Res = new ApiResponse();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var cardEffect = _CashFlowDbContext.CardEffects
                                         .FirstOrDefault(x => x.Id == Arg.Id);

                                if (cardEffect == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message += $@"Id：{Arg.Id} 無此Id\n";
                                }
                                else
                                {
                                    cardEffect.Id = Arg.Id;
cardEffect.TableId = Arg.TableId;
cardEffect.Description = Arg.Description;
cardEffect.CardId = Arg.CardId;
cardEffect.EffectTableId = Arg.EffectTableId;
                                    

                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(cardEffect.Id);
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
                                var cardEffect = _CashFlowDbContext.CardEffects
                                         .FirstOrDefault(x => x.Id == Arg);

                                if (cardEffect == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message = "無此Id";
                                }
                                else
                                {
                                    _CashFlowDbContext.CardEffects.Remove(cardEffect);
                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(cardEffect.Id);
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

