using Microsoft.AspNetCore.Mvc;

namespace BaseCommon
{
    public class ControllerHandler : ControllerBase
    {
        public static int ConnectionsCount { get; set; }
    }
}