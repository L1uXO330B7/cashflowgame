using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace BaseCommon
{
    public class ControllerHandler : ControllerBase
    {
        public static ConcurrentDictionary<int, WebSocket> WebSockets; // ��m�s�u��T���e���A�i�H��"�h�Ӱ�����ֵo�I�s"
    }
}