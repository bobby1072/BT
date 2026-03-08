using System.Linq.Expressions;
using BT.Common.Persistence.Shared.Entities;
using BT.Common.Persistence.Shared.Models;

namespace BT.Common.Persistence.Shared.Repositories.Abstract
{
    public interface IRepository<TEnt, TEntId, TModel>
        where TEnt : BaseEntity<TEntId, TModel>
        where TModel : class
    {
        Task<DbGetManyResult<TModel>> GetAllAsync(
            CancellationToken cancellationToken = default,
            params string[] relations
        );
        Task<DbResult<int>> GetCountAsync(
            Expression<Func<TEnt, bool>>? predicate = null,
            CancellationToken cancellationToken = default
        );
        Task<DbGetManyResult<TModel>> GetManyAsync<T>(
            T value,
            string propertyName,
            CancellationToken cancellationToken = default,
            params string[] relations
        );
        Task<DbGetManyResult<TModel>> GetManyAsync(
            Dictionary<string, object?> propertiesToMatch,
            CancellationToken cancellationToken = default,
            params string[] relations
        );
        Task<DbGetManyResult<TModel>> GetManyAsync(
            TEntId entityId,
            CancellationToken cancellationToken = default,
            params string[] relations
        );
        Task<DbGetManyResult<TModel>> GetManyAsync(
            IReadOnlyCollection<TEntId> entityIds,
            CancellationToken cancellationToken = default,
            params string[] relations
        );
        Task<DbGetOneResult<TModel>> GetOneAsync(
            TEntId entityId,
            CancellationToken cancellationToken = default,
            params string[] relations
        );
        Task<DbGetOneResult<TModel>> GetOneAsync(
            Dictionary<string, object?> propertiesToMatch,
            CancellationToken cancellationToken = default,
            params string[] relations
        );
        Task<DbGetOneResult<TModel>> GetOneAsync<T>(
            T value,
            string propertyName,
            CancellationToken cancellationToken = default,
            params string[] relations
        );
        Task<DbSaveResult<TModel>> CreateAsync(
            IReadOnlyCollection<TModel> entObj,
            CancellationToken cancellationToken = default
        );
        Task<DbSaveResult<TModel>> CreateAsync(
            TModel entObj,
            CancellationToken cancellationToken = default
        );
        Task<DbSaveResult<TModel>> UpdateAsync(
            IReadOnlyCollection<TModel> entObj,
            CancellationToken cancellationToken = default
        );
        Task<DbSaveResult<TModel>> UpdateAsync(
            TModel entObj,
            CancellationToken cancellationToken = default
        );
        Task<DbDeleteResult<TModel>> DeleteAsync(
            IReadOnlyCollection<TModel> entObj,
            CancellationToken cancellationToken = default
        );
        Task<DbDeleteResult<TModel>> DeleteAsync(
            TModel entObj,
            CancellationToken cancellationToken = default
        );
        Task<DbDeleteResult<TEntId>> DeleteAsync(
            IReadOnlyCollection<TEntId> entIds,
            CancellationToken cancellationToken = default
        );
        Task<DbResult<bool>> ExistsAsync<T>(
            T value,
            string propertyName,
            CancellationToken cancellationToken = default
        );
        Task<DbResult<bool>> ExistsAsync(TEntId entityId, CancellationToken cancellationToken = default);
        Task<DbResult<bool>> AnyExistsAsync(
            IReadOnlyCollection<TEntId> entityIds,
            CancellationToken cancellationToken = default
        );
    }
}
