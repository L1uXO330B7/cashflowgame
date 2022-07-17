using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Profiling;

namespace MiniProfilerSwagger.Filter
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext _ExceptionContext)
        {
            using (var step = MiniProfiler.Current.CustomTimingIf("Exception", _ExceptionContext.Exception.Message, 5))
            {
                step.Errored = true;
            }

            return Task.CompletedTask;
        }
    }
}
