using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlowAPI.Filter
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
