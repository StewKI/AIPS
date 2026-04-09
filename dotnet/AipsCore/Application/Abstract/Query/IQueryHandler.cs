namespace AipsCore.Application.Abstract.Query;

public interface IQueryHandler<in TQuery, TQueryResult>
    where TQuery : IQuery<TQueryResult>
    where TQueryResult : IQueryResult
{
    Task<TQueryResult> Handle(TQuery query, CancellationToken cancellationToken = default);
}