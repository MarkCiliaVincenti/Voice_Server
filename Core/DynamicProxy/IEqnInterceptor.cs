namespace Core.DynamicProxy;

public interface IEqnInterceptor
{
    Task InterceptAsync(IEqnMethodInvocation invocation);
}
