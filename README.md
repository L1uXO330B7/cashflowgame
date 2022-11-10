# CashFlowAPP

### 目錄

[1. 分層式架構](#user-content-1-分層式架構)

## 1. 分層式架構

[[ 控制反轉 ] 以 Mock 進行 Unit Test 為例](https://dotblogs.com.tw/JesperLai/2018/03/23/154708)

1. Application Programming Interface ( API ) 應用程式介面層：

   利用路由 Controller 與 Action 作為一個程式路口串起來其他層，Global 類別也可以寫在這如 Filter。

2. Business Logic Layer ( BLL ) 業務邏輯層：

   主要提供 Service 類別，供 API 層操作 EF 功能 ( 業務邏輯 ) 、回傳資料。

3. Data Access Layer ( DAL ) 資料訪問層：

   主要不是針對資料庫是針對資料去做處理，但專案小的狀況下可以與 BLL 合併寫再一起利用 Linq 處理就好。

4. Data Persistence Layer ( DPL ) 數據持久層：

   負責向一個或者多個數據存儲器中操作數據，其實就是 EF 相關。

5. Common Layer：

   負責一些容易用到的例如 Model / DTO / ViewModel / Enum，基本上需要相依於所有專案。

### 還有很多種其他定義，其實就是看架構師怎去定義什麼東西寫在哪，達到

1. 易於理解與開發：什麼東西寫在哪結構清楚。
2. Separation Of Concerns ( SoC ）關注點分離：下層無法相依使用上層類別，上下層盡量相依於介面，減少相依性地獄 Dependency Hell。
3. 可測試性：分層明確對於單元測試來說不用額外抽單元類別出來。

### Task

1. 新增其餘類別庫專案並新增相依 ( 不增加專案相依 VS 無法幫你提示 Using ) 與資料夾如底下圖示

![分層架構與相依性](https://github.com/L1uXO330B7/CashFlowProject/blob/master/BackEnd/Master/Images/%E5%88%86%E5%B1%A4%E6%9E%B6%E6%A7%8B%E8%88%87%E7%9B%B8%E4%BE%9D%E6%80%A72.png)
![新增專案相依](https://github.com/L1uXO330B7/CashFlowProject/blob/master/BackEnd/Master/Images/%E6%96%B0%E5%A2%9E%E5%B0%88%E6%A1%88%E7%9B%B8%E4%BE%9D.png)

2. 新增單元測試專案

![新增單元測試專案](https://github.com/L1uXO330B7/CashFlowProject/blob/master/BackEnd/Master/Images/%E6%96%B0%E5%A2%9E%E5%96%AE%E5%85%83%E6%B8%AC%E8%A9%A6%E5%B0%88%E6%A1%88.png)

## 2. 混和式 API 架構 x CRUD 介面 ( 設計圖 ) 繼承與實作

[閒聊 - Web API 是否一定要 RESTful?](https://blog.darkthread.net/blog/is-restful-required/)

[[不是工程師] 休息(REST)式架構? 寧靜式(RESTful)的Web API是現在的潮流？](https://progressbar.tw/posts/53)

目前公司使用的是 POST GET 搭配 Controller Action 的混和式，而不是正規的 Restful Web API，

主要是像黑大一樣覺得 KISS 即可，一旦 Get Post Put Patch Delete X Controller Action X Safe & Idempotent，

複雜度會成指數上升，而現今 Net Core API 規格，應該靠 Swagger 即可最大可能性的了解，

甚至利用 Swagger ORG 內提供的 Template 產生器，直接依 Swagger 規格文檔產出即可開箱即用 ...

https://editor.swagger.io

### Task

1. 確定需要 CRUD 的 Table 列清單給我 ( 我先確定一下不然白寫 )

   其實不用全部去繼承 CRUD 設計圖的介面 ( 包含 Controller / Service )，需要的才去繼承這個設計圖
 
2. Step 如下以下其實都可以在同個專案做[可以參考這篇](https://dotblogs.com.tw/JesperLai/2018/03/23/154708)，但我們有分專案或分資料夾，方便尋找與單元測試

   1. 新增一個空白 XxxxxController ( 實例 / 實作 / Class ) 如果需要 CRUD 請繼承 ICrudController
   2. 這個新增的 XxxxxController ( 實例 / 實作 / Class ) 基本要完成 ICRUD 介面所定義的四種接口，其餘的可以自己新增
   ![介面泛型](https://github.com/L1uXO330B7/CashFlowProject/blob/master/BackEnd/Master/Images/%E6%B3%9B%E5%9E%8B.png?raw=true)
   3. 新增一個給這四個接口使用的空類別 Class 定義它為 Service ( 處理業務邏輯的類別的命名規則 XXX Service )
   4. 如果需要 CRUD 就新增一個子介面 IXxxxService ( 介面 / Interface ) 並繼承父介面 ICrudService 這裡為何要父子介面 ?
   ![介面實作與繼承](https://github.com/L1uXO330B7/CashFlowProject/blob/master/BackEnd/Master/Images/%E4%BB%8B%E9%9D%A2%E5%AF%A6%E4%BD%9C%E8%88%87%E7%B9%BC%E6%89%BF.png?raw=true)
   `你注入是利用介面去 New Class 給你，你介面沒定義的在 Controller 使用注入的 Service 時就使用不到`
   `所以當你繼承的是 ICrudService 因為指定義四種 Function 你在 Controller 就只能使用這四種`
   `而如果用 IUserService 去繼承 ICrudService 則你有意新增的如 Function DeleteAll 就可以定義在 IUserService 無需去改 ICrudService`
   5. 第二種思路則是像你講的變成用參數去解決，因為參數我們是用泛型，可以定義一些程式規則如某參數是 Null 或空則撈全部或刪除全部之類
   6. 接著新增一個 XxxxService 的 Class 並繼承上述 4. IXxxxService 實作此介面所定義的 Function 即可
   7. 接著回到 XxxxxController 為了使用 XxxxxService 我們有兩種方式 
      1. var XXX = new XxxxxService(); 直接使用 [為什麼不使用可以參考這篇](https://dotblogs.com.tw/JesperLai/2018/03/23/154708)
      2. 在 XxxxxController 的建構子注入，這裡有個知識點 1. 必須要到 Program.cs 定義 Interface 與 Class 的 Mapping 有機會遇到以下問題，與 4. 取不到方法的問題
      ![注入多型錯誤500注入Mapping不到](https://github.com/L1uXO330B7/CashFlowProject/blob/master/BackEnd/Master/Images/%E6%B3%A8%E5%85%A5%E5%A4%9A%E5%9E%8B%E9%8C%AF%E8%AA%A4500%E6%B3%A8%E5%85%A5Mapping%E4%B8%8D%E5%88%B0.png?raw=true)
      ![避免ControllerReq型別與ServiceReq型別不一樣導致脫褲子放屁](https://github.com/L1uXO330B7/CashFlowProject/blob/master/BackEnd/Master/Images/%E9%81%BF%E5%85%8DControllerReq%E5%9E%8B%E5%88%A5%E8%88%87ServiceReq%E5%9E%8B%E5%88%A5%E4%B8%8D%E4%B8%80%E6%A8%A3%E5%B0%8E%E8%87%B4%E8%84%AB%E8%A4%B2%E5%AD%90%E6%94%BE%E5%B1%81.png?raw=true)

## 接續的部分因為較繁瑣改為紀錄實作了什麼方便以後參考

1. 全局異常處理、全局 MiniProfile、Swagger 設定
2. 拆分業務邏輯為 ClientSide 與 AdminSide 由於 ClientSide 可能不會像後台每張表幾乎都要 CRUD 所以獨立一個 Controller、Service
3. 並實作以 JWT 方式 Encode Decode 的 Login 驗證模式、與 Email 驗證碼 SignUp 的三支 API
4. 接著就進入 Angular 熟悉 Typescript 與 Module、Component、Services 如何開發前端
5. 利用 HttpClient 實作有別於 Promise 行為模式的觀察者模式 Observer、Subscribe
6. 4 著重於在一支 Typescript 完成所有事，接著要熟悉 Service
7. Module
8. Pipe

## 前端引入 Angular ngb (Angular Bootstrap)

1. 這個套件的指引是用 Angular Cli [ng add @ng-bootstrap/ng-bootstrap](https://ng-bootstrap.github.io/#/home)，但使用 NGX Cli 之後，似乎無法使用 NG 指令，所以用 NPM 分別安裝這個套件同捆包(Bundle)所需要的套件，分別是 
   1. [NGB 本身](https://www.npmjs.com/package/@ng-bootstrap/ng-bootstrap)
   2. [Popover](https://popper.js.org/)
   3. [angular/localize](https://www.npmjs.com/package/@angular/localize)，要在 `polyfill.js` 使用 ```import '@angular/localize/init'```

### Angular 接收 File Javascript .js 錯誤 ( 要求 Request 時需再 Header 宣告 responseType )

https://stackoverflow.com/questions/62046090/angular-unexpected-token-c-in-json-at-position-0-at-json-parse-when-expecting-a

https://www.tpisoftware.com/tpu/articleDetails/1084

## 錯誤處理方式與 Log 資料解決方案 ( Serilog & Seq ) 搭配 CRUD 寫入 DB ( 複寫 dbContext ) & MiniProfile 

### 錯誤處理方式

參考 https://ithelp.ithome.com.tw/m/articles/10246428

1. 使用try /catch/ finaly來攔截可能發生的錯誤，讓例外狀況發生時，應用程式可以適當的處理錯誤或結束，保障應用程式的品質。
2. 攔截錯誤後，進行有效資訊的紀錄，以提供開發人員有效率的偵測與檢查問題。
3. 針對不同類型的例外事件進行攔截，不使用類似 System.Exception、System.SystemException 等非特定的例外狀況來處理錯誤。
4. 攔截例外狀況時，不要排除任何可能的特殊例外狀況
5. 請勿過度使用攔截， 應該經常允許例外狀況散佈到呼叫堆疊上。
6. 請使用 try-finally 並避免使用 try-catch 來清除程式碼。 在編寫完善的例外狀況程式碼中，try-finally 遠比 try-catch 更為常用。
7. 當攔截並重新擲回例外狀況時，最好使用空白擲回方式， 因為這是保留例外狀況呼叫堆疊的最好方式。
8. 請不要使用無參數的 catch 區塊來處理不符合 CLS 標準的例外狀況(不是衍生自 System.Exception 的例外狀況)。 可支援不是衍生自 Exception 的例外狀況之語言可以自由處理那些不符合 CLS 標準的例外狀況。

### 日誌解決方案 

1. Serilog 實體 Log 日誌 
2. 搭配 CRUD 時寫入 DB LogTable ( 複寫 dbContext ) 操作日誌

#### Serilog

// 參考 https://ithelp.ithome.com.tw/m/articles/10247300

1. [How to configure and use Serilog in ASP.NET Core 6?](https://stackoverflow.com/questions/71599246/how-to-configure-and-use-serilog-in-asp-net-core-6)

2. [.NET 6.0 如何使用 Serilog 對應用程式事件進行結構化紀錄](https://blog.miniasp.com/post/2021/11/29/How-to-use-Serilog-with-NET-6)

3. [第三十八节：SeriLog日志的各种用法以及与Core MVC封装集成](https://www.cnblogs.com/yaopengfei/p/14261414.html)

4. 後來發現 Seq 是另外一款 ViewLog 的程式，所以此處不使用

#### 覆寫 Override DdbContext

### Db First & Code First & Db Migration 資料庫移轉 ( 可版控程式碼的 Db 結構 )

// 參考 https://ithelp.ithome.com.tw/m/articles/10240606

## SignalR

1. 連線時嘗試使用 GetHttpContext()，官方 API 取得 Header Token，但只能取到 null 的 Header

2. 問題可能來自此 [ISSUE](https://github.com/dotnet/aspnetcore/issues/39894)

3. 第一種解決方式使用前端 Singal 的 API 用 WebSocket 方式，送整包含 Token 的資料回後端解析，再回傳前端

4. 後來發現第一種解決方式不太方便，且無法在連線建立時，就接受到使用者 Token，[第二種解決方式是使用連線 URL 傳參數的方式來將資料帶到後端](https://stackoverflow.com/questions/69519736/how-can-i-pass-retreive-querystring-values-in-net-core-version-of-signalr)，且第二種方式的安全性與第一種雷同，Post 是塞在標頭裡，兩種方式只要進到 Network 觀察，Token 都是無法被藏起來的

#### SignalR Hub 中使用 Timer 所出現的問題

1. 前端需在建立連線之後，才能去 Invoke SetTimer()，來讓計時器開始啟動，不然會出現無法在未建立連線錯誤，[解決方式](https://stackoverflow.com/questions/53412852/signalr-core-call-function-when-connection-is-established)

2. 後端 SetTimer() 被 Invoke 到之後，觸發計時器中的事件，事件中的 SendAsync 是無法啟用的，會有 "Cannot access a disposed object" 的錯誤，原因為[Hub LifeTime 的問題](https://stackoverflow.com/questions/55795669/cannot-access-a-disposed-object-crash-in-signalr)



## .Net Core 6 要使用 FromSqlInterpolated 執行 SQL 指令需安裝 Microsoft.EntityFrameworkCore.Relational

https://stackoverflow.com/questions/51017703/error-cs1061-dbsett-does-not-contain-a-defin   ition-for-fromsql-and-no-exte

## 引入 Dapper 做動態查詢

https://blog.darkthread.net/blog/dapper-with-ef-core/



## DataBase 比對

1. 受限於條件，開發上無法使用相同的 DB，因此測試資料不一樣。

2. 解決上述狀況，可使用 Visual Studio 上方工具列之工具中的 SQL Server，可以比對資料，也可以比對資料結構 


## IIS 部屬 NetCore & Angular

1. 先至控制台中的程式集增加 Windows IIS 服務功能

2. 正式機得先安裝：ASP.NET Core Runtime & Hosting Bundle

3. 新增 IIS 站台

4. 新增 IIS_IUSRS 權限

5. 將 Angular 與 NetCore build 出來的 package 放置 IIS 路徑之中

6. 一個站台同時有前台和後台跟 API

### 此時各種問題都會跑出來

1. 需安裝 URL Rewrite，才能在 IIS 上成功使用 SPA，安裝好 URL Rewrite 模組後，需要將相對應的 web.config 嵌入至 Package 中，並寫好規則，在本專案中，ClientSide 使用 / 來當根路徑，也就是當 URL 是 /index.html 時，就可以由 ClientSide 的 Angular Router 來接管，而 AdminSide 則需要是 /AdminSide/index.html 才能對應到 AdminSide 的 Angular Router

2. 正式機路徑對應的問題 Ex. API 路徑、SQL 位置等等，需要透過 Angular 的 env.prod 中來進行設定，NetCore 則是在 appsetting 來調整

3. 會遇到各種問題，也只能上網查


## JSON String 問題

1. 後台存 JSON String 進後端，回來前端 Parse，狀況百出 => [解決方法](https://stackoverflow.com/questions/14432165/error-uncaught-syntaxerror-unexpected-token-with-json-parse)

2. 改用逗號來分割字串，前台存 TEST,TEST2,TEST3 格式回後端之後，回前端使用 split(",") 方式處理成陣列