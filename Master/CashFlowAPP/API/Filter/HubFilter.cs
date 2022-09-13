using Microsoft.AspNetCore.SignalR;

namespace API.Filter
{
    /// <summary>
    /// 參考 
    /// https://stackoverflow.com/questions/64133021/handle-all-exceptions-in-signalr-hubs-in-asp-net-core-3-1
    /// https://docs.microsoft.com/en-us/aspnet/core/signalr/hub-filters?view=aspnetcore-5.0
    /// </summary>
    public class HubFilter : IHubFilter
    {
        public async ValueTask<object> InvokeMethodAsync(
        HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
        {
            Console.WriteLine($"Calling hub method '{invocationContext.HubMethodName}'");
            try
            {
                return await next(invocationContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception calling '{invocationContext.HubMethodName}': {ex}");
                throw;
            }
        }

        // Optional method
        public Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
        {
            try
            {
                return next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception calling '{next.Method.Name}': {ex}");
                throw;
            }
        }

        // Optional method
        public Task OnDisconnectedAsync(
            HubLifetimeContext context, Exception exception, Func<HubLifetimeContext, Exception, Task> next)
        {
            try
            {
                return next(context, exception);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception calling '{next.Method.Name}': {ex}");
                throw;
            }
        }
    }
}
