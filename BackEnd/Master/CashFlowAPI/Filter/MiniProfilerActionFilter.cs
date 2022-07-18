using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlowAPI.Filter
{
    public class MiniProfilerActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
