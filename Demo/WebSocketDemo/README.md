# 參考

## WebSocket

1. [ASP.NET Core 6 Websocket 香草 JS](https://blog.darkthread.net/blog/aspnet-core-websocket-chatroom/)
2. 照著參考弄了一版跨域都不會擋感覺有點危險的版本
3. [ASP.NET Core 中的 WebSockets 支援 - Microsoft Docs](https://docs.microsoft.com/zh-tw/aspnet/core/fundamentals/websockets?view=aspnetcore-6.0)
4. 原來大大是參考微軟範本，裡面就有講到 net core controller websocket

## SignalR

1. [我與 ASP.NET Core 的 30天系列 第 25 篇](https://ithelp.ithome.com.tw/articles/10251470)
   * SignalR
   1. 自動處理連接管理
   2. 同時將訊息傳送給所有已連線的用戶端
   3. 將訊息傳送給特定用戶端或用戶端群組
   4. 調整以處理增加的流量
2. [SignaIR-Cross](https://blog.darkthread.net/blog/signalr-cross-domain/)
3. [SignaIR-Cross 需直接設定 app.UseCors](https://docs.microsoft.com/zh-tw/aspnet/core/signalr/security?view=aspnetcore-6.0)
4.[CorsPolicy 指定來源](https://www.cnblogs.com/myzony/p/10511492.html)

## Angular WeSocket

1. [在 Angular 当中使用 WebSocket](https://heptaluan.github.io/2019/05/20/Angular/15/)

## 功能

1. 多客戶端同時跟伺服器間建立持續連線、雙向傳輸
2. 客戶端可發送訊息給伺服器取得回應
3. 伺服器端也要能主動派發訊息給指定對象或對所有 WebSocket 連線廣播
4. 即時偵測到某條 WebSocket 被關閉做出因應
5. 斷線自動重連 [實測SignalR自動重連特性](https://blog.darkthread.net/blog/test-signalr-reconnect/)
6. 瀏覽器不支援 WebSocket 的備援

上述有些利用 SignalR 做會更省事，此 Demo 兩種都試看看

## 架構閱讀

[Furion 大陸 net core 框架](https://dotnetchina.gitee.io/furion/)

透過研讀開源框架了解有什麼基礎設施可以使用

1. MiniProfiler 性能分析和监听 必須要整合至 swagger
2. 设置 Swagger 自动授权
3. [Docker Simple](https://github.com/twtrubiks/docker-tutorial)
4. [swagger 根據规范化文档產生 api js code](https://dotnetchina.gitee.io/furion/docs/clientapi) 或許可以寫成程式碼產生器，只需置換之類的
