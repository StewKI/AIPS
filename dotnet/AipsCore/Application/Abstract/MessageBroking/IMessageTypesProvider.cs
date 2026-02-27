namespace AipsCore.Application.Abstract.MessageBroking;

public interface IMessageTypesProvider
{
    ICollection<Type> GetAllMessageTypes();
}