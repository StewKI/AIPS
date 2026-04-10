using AipsCore.Domain.Abstract;
using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Common.ValueObjects;

namespace AipsCore.Domain.Common.Validation.Rules;

public class DomainModelExistsRule<TModel, TId> : AbstractRule
    where TModel : DomainModel<TId>
    where TId : DomainId
{
    private readonly TModel? _model;
    private readonly TId _id;
    private readonly string _modelName;

    public DomainModelExistsRule(TModel? model, TId id)
    {
        _model = model;
        _id = id;
        _modelName = typeof(TModel).Name;
    }

    public DomainModelExistsRule(TModel? model, TId id, string modelName)
    {
        _model = model;
        _id = id;
        _modelName = modelName;
    }

    protected override string ErrorCode => $"{_modelName.ToLowerInvariant()}_not_found";
    protected override string ErrorMessage => $"{_modelName} with id '{_id}' was not found";
    public override bool Validate() => _model is not null;
}