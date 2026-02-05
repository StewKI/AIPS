using AipsCore.Application.Abstract.Command;
using AipsCore.Application.Abstract.Query;

namespace AipsCore.Application.Common.Dispatcher;

public class DispatchException : Exception
{
    public DispatchException(object commandQuery, Exception innerException)
        : base($"Error dispatching '{commandQuery.GetType().Name}' because of: {innerException.Message}", innerException)
    { }
}