namespace BuildingBlock.Application.Repositories;

public interface ISecureCommand
{
    /// <summary>
    /// Verifies the entity with HasAccess before inserting.
    /// </summary>
    /// <typeparam name="T">The type of the entity to insert.</typeparam>
    /// <param name="obj">The entity to insert.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if access is denied.</exception>
    Task InsertAsync<T>(T obj, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Verifies the entity with HasAccess before inserting and optionally persists immediately.
    /// </summary>
    /// <typeparam name="T">The type of the entity to insert.</typeparam>
    /// <param name="obj">The entity to insert.</param>
    /// <param name="persistImmediately">Whether to persist the changes immediately.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if access is denied.</exception>
    Task InsertAsync<T>(T obj, bool persistImmediately, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Verifies the entity with HasAccess before updating.
    /// </summary>
    /// <typeparam name="T">The type of the entity to update.</typeparam>
    /// <param name="obj">The entity to update.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if access is denied.</exception>
    Task UpdateAsync<T>(T obj, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Verifies the entity with HasAccess before updating and optionally persists immediately.
    /// </summary>
    /// <typeparam name="T">The type of the entity to update.</typeparam>
    /// <param name="obj">The entity to update.</param>
    /// <param name="persistImmediately">Whether to persist the changes immediately.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if access is denied.</exception>
    Task UpdateAsync<T>(T obj, bool persistImmediately, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Verifies the entity with HasAccess before deleting.
    /// </summary>
    /// <typeparam name="T">The type of the entity to delete.</typeparam>
    /// <param name="obj">The entity to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if access is denied.</exception>
    Task DeleteAsync<T>(T obj, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Verifies the entity with HasAccess before deleting and optionally persists immediately.
    /// </summary>
    /// <typeparam name="T">The type of the entity to delete.</typeparam>
    /// <param name="obj">The entity to delete.</param>
    /// <param name="persistImmediately">Whether to persist the changes immediately.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if access is denied.</exception>
    Task DeleteAsync<T>(T obj, bool persistImmediately, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Persists all changes made in this context to the database.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SaveAsync(CancellationToken cancellationToken);
}
