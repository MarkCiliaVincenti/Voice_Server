namespace Core.DynamicProxy;

public abstract class EqnInterceptor : IEqnInterceptor
{
    public abstract Task InterceptAsync(IEqnMethodInvocation invocation);
}
