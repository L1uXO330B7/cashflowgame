using DPL.EF;
using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Profiling;

namespace API.Filter
{
    /// <summary>
    /// 全域分析器
    /// </summary>
    public class MiniProfilerActionFilter : IActionFilter
    {
        private readonly CashFlowDbContext _CashFlowDbContext;
        public MiniProfilerActionFilter(CashFlowDbContext cashFlowDbContext)
        {
            _CashFlowDbContext = cashFlowDbContext;
        }

        public void OnActionExecuting(ActionExecutingContext _ActionExecutingContext)
        {
            var Context = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 當前執行:{_ActionExecutingContext.ActionDescriptor.DisplayName}";
            var step = MiniProfiler.Current.CustomTimingIf("MiniProfiler", Context, 5);
            _ActionExecutingContext.HttpContext.Items["step"] = step;

            var Ip = _ActionExecutingContext.HttpContext.Connection.RemoteIpAddress.ToString();
            var SafetyList = new List<string>() { "150.117.83.67", "122.254.0.131", "::1" };
            if (!SafetyList.Contains(Ip))
            {
                var log = new Log();
                log.UserId = 0;
                log.UserName = "Request";
                log.TableId = 0;
                log.TableName = $"IP：{Ip}";
                log.Action = 0;
                log.ActionDate = DateTime.Now;

                _CashFlowDbContext.Logs.Add(log);
                _CashFlowDbContext.SaveChanges();
            }
        }

        public void OnActionExecuted(ActionExecutedContext _ActionExecutingContext)
        {
            var step = _ActionExecutingContext.HttpContext.Items["step"] as IDisposable;
            if (step != null)
            {
                step.Dispose();
            }
        }
    }
}
