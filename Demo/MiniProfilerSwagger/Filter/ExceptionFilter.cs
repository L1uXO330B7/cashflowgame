using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MiniProfilerSwagger.Model;
using Newtonsoft.Json;
using StackExchange.Profiling;

namespace MiniProfilerSwagger.Filter
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext _ExceptionContext)
        {
            // https://www.cnblogs.com/dotnet261010/p/13193124.html

            // 如果异常没有被处理则进行处理
            if (_ExceptionContext.ExceptionHandled == false)
            {
                var ErrorMsg = "";
                var Exception = _ExceptionContext.Exception;
                while (Exception != null)
                {
                    ErrorMsg += $@"{Exception.Message}";
                    Exception = Exception.InnerException;
                }

                using (var step = _ExceptionContext.HttpContext.Items["step"] as CustomTiming)
                {
                    step.CommandString = ErrorMsg;
                    step.StackTraceSnippet = JsonConvert.SerializeObject(_ExceptionContext.Exception.StackTrace, Formatting.Indented);
                    step.Errored = true;
                    step.Stop();
                }

                var _Result = new Result<string>();
                _Result.Success = false;
                _Result.Message = _ExceptionContext.Exception.StackTrace;

                _ExceptionContext.Result = new ContentResult
                {
                    // 返回状态码设置为200，表示成功
                    StatusCode = StatusCodes.Status200OK,
                    // 设置返回格式
                    ContentType = "application/json;charset=utf-8",
                    Content = JsonConvert.SerializeObject(_Result)
                };

                // 设置为true，表示异常已经被处理了
                _ExceptionContext.ExceptionHandled = true;
            }

            return Task.CompletedTask;
        }
    }
}
