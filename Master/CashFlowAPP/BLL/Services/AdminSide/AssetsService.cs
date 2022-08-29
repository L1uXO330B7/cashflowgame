

using BLL.IServices;
using Common.Model;
using DPL.EF;
using Common.Enum;
using Common.Model.AdminSide;
using Newtonsoft.Json;

namespace BLL.Services.AdminSide
{
    public class AssetsService : IAssetsService<
            List<CreateAssetArgs>,
            List<ReadAssetArgs>,
            List<UpdateAssetArgs>,
            List<int?>
        >
    {
        private readonly CashFlowDbContext _CashFlowDbContext;

        public AssetsService(CashFlowDbContext cashFlowDbContext)
        {
            _CashFlowDbContext = cashFlowDbContext;
        }

        public async Task<ApiResponse> Create(ApiRequest<List<CreateAssetArgs>> Req)
        {
            var assets = new List<Asset>();

            var SussList = new List<int>();

            foreach (var Arg in Req.Args)
            {
                var asset = new Asset();

                asset.Id = Arg.Id;
                asset.Name = Arg.Name;
                asset.Value = Arg.Value;
                asset.Status = Arg.Status;
                asset.AssetCategoryId = Arg.AssetCategoryId;
                asset.Description = Arg.Description;

                assets.Add(asset);
            }

            _CashFlowDbContext.AddRange(assets);
            _CashFlowDbContext.SaveChanges();
            // 不做銷毀 Dispose 動作，交給 DI 容器處理

            // 此處 SaveChanges 後 SQL Server 會 Tracking 回傳新增後的 Id
            SussList = assets.Select(x => x.Id).ToList();

            var Res = new ApiResponse();
            Res.Data = $@"已新增以下筆數(Id)：[{string.Join(',', SussList)}]";
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "成功新增";

            return Res;
        }

        public async Task<ApiResponse> Read(ApiRequest<List<ReadAssetArgs>> Req)
        {
            var Res = new ApiResponse();
            var assets = _CashFlowDbContext.Assets.AsQueryable();

            foreach (var Arg in Req.Args)
            {
                if (Arg.Key == "Id") // Id 篩選條件
                {
                    var Ids = JsonConvert
                            .DeserializeObject<List<int>>(Arg.JsonString);

                    assets = assets.Where(x => Ids.Contains(x.Id));
                }

                if (Arg.Key == "Status") // 狀態篩選條件
                {
                    var Status = JsonConvert
                               .DeserializeObject<byte>(Arg.JsonString);

                    assets = assets.Where(x => x.Status == Status);
                }
            }

            var Assets = assets
            // 後端分頁
            // 省略幾筆 ( 頁數 * 每頁幾筆 )
            .Skip(((int)Req.PageIndex - 1) * (int)Req.PageSize)
            // 取得幾筆，
            .Take((int)Req.PageSize)
            .ToList();

            var AssetCategorys = _CashFlowDbContext.AssetCategories.ToList();

            Res.Data = new { Assets, AssetCategorys };
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "成功讀取";
            Res.TotalDataCount = assets.ToList().Count;

            return Res;
        }

        public async Task<ApiResponse> Update(ApiRequest<List<UpdateAssetArgs>> Req)
        {
            var Res = new ApiResponse();

            var SussList = new List<int>();

            foreach (var Arg in Req.Args)
            {
                var asset = _CashFlowDbContext.Assets
                         .FirstOrDefault(x => x.Id == Arg.Id);

                if (asset == null)
                {
                    Res.Success = false;
                    Res.Code = (int)ResponseStatusCode.CannotFind;
                    Res.Message += $@"Id：{Arg.Id} 無此Id\n";
                }
                else
                {
                    asset.Id = Arg.Id;
                    asset.Name = Arg.Name;
                    asset.Value = Arg.Value;
                    asset.Status = Arg.Status;
                    asset.AssetCategoryId = Arg.AssetCategoryId;
                    asset.Description = Arg.Description;

                    _CashFlowDbContext.SaveChanges();
                    SussList.Add(asset.Id);
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
                var asset = _CashFlowDbContext.Assets
                         .FirstOrDefault(x => x.Id == Arg);

                if (asset == null)
                {
                    Res.Success = false;
                    Res.Code = (int)ResponseStatusCode.CannotFind;
                    Res.Message = "無此Id";
                }
                else
                {
                    _CashFlowDbContext.Assets.Remove(asset);
                    _CashFlowDbContext.SaveChanges();
                    SussList.Add(asset.Id);
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

