namespace CesiZen.Domain.Interfaces;

public interface ICommandInterface<T> : ICommand<T> where T : class
{
}
