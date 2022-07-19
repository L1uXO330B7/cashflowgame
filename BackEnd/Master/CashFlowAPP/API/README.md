# CashFlowAPP

## 分層式架構

1. Application Programming Interface ( API ) 應用程式介面層：
利用路由 Controller 與 Action 作為一個程式路口串起來其他層，Global 類別也可以寫在這如 Filter。

2. Business Logic Layer ( BLL ) 業務邏輯層：
主要提供 Service 類別，供 API 層操作功能、回傳資料。

3. Data Access Layer ( DAL ) 資料訪問層：
主要不是針對資料庫是針對資料去做處理，但專案小的狀況下可以與 BLL 合併血再一起利用 Linq 處理就好。

4. Data Persistence Layer ( DPL )：
負責向一個或者多個數據存儲器中操作數據，其實就是 EF 相關。

### 還有很多種其餘定義，其實就是看架構師怎去定義什麼東西寫在哪，達到

1. 易於理解與開發：什麼東西寫在哪結構清楚。
2. Separation Of Concerns ( SoC ）關注點分離：下層無法相依使用上層類別，上下層盡量相依於介面，減少相依性地獄 Dependency Hell。
3. 可測試性：分層明確對於單元測試來說不用額外抽單元類別出來。

### Task

1. 新增其餘類別庫專案並新增相依 ( 不增加專案相依 VS 無法幫你提示 Using ) 與資料夾如底下圖示

![分層架構與相依性](https://github.com/L1uXO330B7/CashFlowProject/blob/master/BackEnd/Master/Images/%E5%88%86%E5%B1%A4%E6%9E%B6%E6%A7%8B%E8%88%87%E7%9B%B8%E4%BE%9D%E6%80%A7.png
![新增專案相依](https://github.com/L1uXO330B7/CashFlowProject/blob/master/BackEnd/Master/Images/%E6%96%B0%E5%A2%9E%E5%B0%88%E6%A1%88%E7%9B%B8%E4%BE%9D.png)

2. 新增一單元測試專案
