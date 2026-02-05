using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Common.Dispatcher;

public class DispatchException : Exception
{
    public DispatchException(object commandQuery)
        : base($"Error while dispatching '{commandQuery.GetType().Name}'")
    { }
}