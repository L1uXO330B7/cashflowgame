using Castle.DynamicProxy;
using StackExchange.Profiling;

namespace MiniProfilerSwagger.Interceptor
{
    /// <summary>
    /// 這種方式要引入太多套件，棄用改為使用原生篩選器，但篩選器好像抓不到真的執行方法，這部分要在確定一下
    /// </summary>
    public class MiniFilterInterceptor : IInterceptor
    {
        public void Intercept(IInvocation Invocation)
        {
            MiniProfiler.Current.Step($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")} 當前執行方法：{Invocation.Method.Name}");
            Invocation.Proceed();
        }
    }
}
