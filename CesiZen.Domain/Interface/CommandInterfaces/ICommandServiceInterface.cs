namespace CesiZen.Domain.Interface;

public interface ICommandServiceInterface<T> : ICommand<T> where T : class
{
}
