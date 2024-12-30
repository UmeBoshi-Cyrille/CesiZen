namespace CesiZen.Domain.Interface;

public interface ICommandInterface<T> : ICommand<T> where T : class
{
}
