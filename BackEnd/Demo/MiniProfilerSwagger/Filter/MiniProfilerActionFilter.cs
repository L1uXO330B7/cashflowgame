using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Profiling;

namespace MiniProfilerSwagger.Filter
{
    public class MiniProfilerActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext _ActionExecutingContext)
        {
            var Context = $"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")} 當前執行:{_ActionExecutingContext.ActionDescriptor.DisplayName}";
            var step = MiniProfiler.Current.Step(Context);
            _ActionExecutingContext.HttpContext.Items["step"] = step;
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
