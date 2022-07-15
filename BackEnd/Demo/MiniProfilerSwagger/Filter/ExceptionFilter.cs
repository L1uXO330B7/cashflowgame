using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Profiling;

namespace MiniProfilerSwagger.Filter
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext _ExceptionContext)
        {
            //var step = _ExceptionContext.HttpContext.Items["step"] as IDisposable;
            //if (step != null)
            //{
            //    step.Dispose();
            //}
        }
    }
}
