namespace SharedKernel;

/// <summary>
/// Base repository interface for aggregate roots.
/// </summary>
/// <typeparam name="T">The type of aggregate root.</typeparam>
public interface IRepository<T> where T : class
{
}
