# 參考

## WebSocket

1. [ASP.NET Core 6 Websocket 香草 JS](https://blog.darkthread.net/blog/aspnet-core-websocket-chatroom/)
2. 照著參考弄了一版跨域都不會擋感覺有點危險的版本

## SignalR

1. [我與 ASP.NET Core 的 30天系列 第 25 篇](https://ithelp.ithome.com.tw/articles/10251470)

## 功能

1. 多客戶端同時跟伺服器間建立持續連線、雙向傳輸
2. 客戶端可發送訊息給伺服器取得回應
3. 伺服器端也要能主動派發訊息給指定對象或對所有 WebSocket 連線廣播
4. 即時偵測到某條 WebSocket 被關閉做出因應
5. 斷線自動重連
6. 瀏覽器不支援 WebSocket 的備援

上述有些利用 SignalR 做會更省事，此 Demo 兩種都試看看

