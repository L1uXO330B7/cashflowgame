# MiniProfiler 轻量级性能分析工具

## 參考 

### MiniProfiler

https://iter01.com/585025.html

https://miniprofiler.com/dotnet/AspDotNetCore

https://blog.csdn.net/hykkingking/article/details/117746154

### Swagger

https://dotblogs.com.tw/shadowkk/2019/09/03/092620

https://ithelp.ithome.com.tw/articles/10195190

## AOP

https://www.cnblogs.com/laozhang-is-phi/p/10287023.html#autoid-3-4-0

https://www.cnblogs.com/laozhang-is-phi/p/9547574.html#autoid-5-2-0

https://stackoverflow.com/questions/51783365/logging-using-aop-in-net-core-2-1

https://ithelp.ithome.com.tw/articles/10277706

https://blog.hungwin.com.tw/aspnet-mvc-aop-authorize/#ASPNET_MVC_AOP

https://stackoverflow.com/questions/6507568/using-mvc-miniprofiler-for-every-action-call

## Filter

https://blog.johnwu.cc/article/ironman-day17-asp-net-core-exception-handler.html

## Nuget

``` 
MiniProfiler.AspNetCore.Mvc
MiniProfiler.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.SqlServer
Castle.Core
```

## EF

dotnet tool install --global dotnet-ef

https://www.796t.com/post/NW82eGM=.html

dotnet ef dbcontext scaffold "Data Source=.\SQLEXPRESS;Database=MiniProfilerDb;AttachDbFilename=C:\Users\rognp\Desktop\cashflowgame\BackEnd\Demo\MiniProfilerSwagger\MiniProfilerDb.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir EntityFramework -f -v

## 設定 Sample

```
    // （可選）訪問地址路由根目錄；預設為：/mini-profiler-resources
    o.RouteBasePath = "/profiler";

    // （可選）資料快取時間 （在 MemoryCacheStorage 中默認為 30 分鐘）
    // 注意：如果在 MemoryCache 上設置了 SizeLimit，MiniProfiler 將不起作用！
    // 詳見：https://github.com/MiniProfiler/dotnet/issues/501
    (o.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);

    // （可選）控制使用哪個 SQL 格式化程序，InlineFormatter 是默認的
    o.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();

    // （可選）您可以禁用“Connection Open()”、“Connection Close()”（和異步變體）跟踪。
    // 跟蹤連線開啟關閉 （默認為true，並跟踪連接打開/關閉）
    o.TrackConnectionOpenClose = true;

    // 介面主題顏色方案;預設淺色
    // （可選）使用“淺色”配色方案以外的其他顏色方案。（默認為“輕”）
    //o.ColorScheme = StackExchange.Profiling.ColorScheme.Auto；
    o.ColorScheme = StackExchange.Profiling.ColorScheme.Dark;

    // 啟用在響應中發送 Server-Timing 標頭
    o.EnableServerTimingHeader = true;

    // （可選）控制訪問頁面授權，您可以使用 Func<HttpRequest, bool> 選項，預設所有人都能訪問
    //o.ResultsAuthorize;
    //o.ResultsListAuthorize = request => MyGetUserFunction(request).CanSeeMiniProfiler;
    // 或者，有可用的異步版本：
    //o.ResultsAuthorizeAsync = 異步請求 => (await MyGetUserFunctionAsync(request)).CanSeeMiniProfiler;
    //o.ResultsAuthorizeListAsync = 異步請求 => (await MyGetUserFunctionAsync(request)).CanSeeMiniProfilerLists;

    // （可選）要控制分析哪些請求，請使用 Func<HttpRequest, bool> 選項：
    // 要控制分析哪些請求，預設說有請求都分析
    //o.ShouldProfile = request => MyShouldThisBeProfiledFunction(request);

    // （可選）配置文件存儲在用戶 ID 下，獲取它的函數：
    // （默認為null，因為上述方法默認不使用）
    //o.UserIdProvider = request => MyGetUserIdFunction(request);

    // （可選）如果需要，交換整個分析器提供程序
    // （默認處理異步並且幾乎適用於所有應用程序）
    // o.ProfilerProvider = new MyProfilerProvider();

    // 可選擇更改毫秒計時顯示的小數位數。
    //（默認為 2）
    //o.PopupDecimalPlaces = 1;

    // 下面是較新的選項，在 .NET Core 3.0 及更高版本中可用：
    // （可選）您可以禁用 MVC 過濾器分析
    //（默認為true，過濾器被配置）
    // o.EnableMvcFilterProfiling = true;

    // ...或僅保存超過特定毫秒持續時間的過濾器（包括它們的子級）
    //（默認為null，所有過濾器都被分析）
    // options.MvcFilterMinimumSaveMs = 1.0m;

    // （可選）您可以禁用 MVC 視圖分析
    //（默認為true，並且視圖被分析）
    // o.EnableMvcViewProfiling = true;

    // ...或僅保存超過特定毫秒持續時間的視圖（包括它們的子視圖）
    //（默認為null，所有視圖都被分析）
    // options.MvcViewMinimumSaveMs = 1.0m;

    // （可選）監聽 MiniProfiler 本身發生的任何錯誤
    // options.OnInternalError = e => MyExceptionLogger(e);

    // （可選 - 不推薦）您可以在使用內存存儲時啟用帶有堆棧和工具提示的繁重調試模式
    // 與普通分析相比，它有很多開銷，並且應該只在考慮到這一點的情況下使用
    //（默認為false，debug/heavy模式關閉）
    //options.EnableDebugMode = true;
```