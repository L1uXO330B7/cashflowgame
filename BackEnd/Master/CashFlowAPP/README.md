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