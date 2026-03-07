using System.Linq.Expressions;
using BT.Common.Persistence.Shared.Entities;
using BT.Common.Persistence.Shared.Models;

namespace BT.Common.Persistence.Shared.Repositories.Abstract
{
    public interface IRepository<TEnt, TEntId, TModel>
        where TEnt : BaseEntity<TEntId, TModel>
        where TModel : class
    {
        Task<DbGetManyResult<TModel>> GetAll(
            CancellationToken cancellationToken = default,
            params string[] relations
        );
        Task<DbResult<int>> GetCount(
            Expression<Func<TEnt, bool>>? predicate = null,
            CancellationToken cancellationToken = default
        );
        Task<DbGetManyResult<TModel>> GetMany<T>(
            T value,
            string propertyName,
            CancellationToken cancellationToken = default,
            params string[] relations
        );
        Task<DbGetManyResult<TModel>> GetMany(
            Dictionary<string, object?> propertiesToMatch,
            CancellationToken cancellationToken = default,
            params string[] relations
        );
        Task<DbGetManyResult<TModel>> GetMany(
            TEntId entityId,
            CancellationToken cancellationToken = default,
            params string[] relations
        );
        Task<DbGetManyResult<TModel>> GetMany(
            IReadOnlyCollection<TEntId> entityIds,
            CancellationToken cancellationToken = default,
            params string[] relations
        );
        Task<DbGetOneResult<TModel>> GetOne(
            TEntId entityId,
            CancellationToken cancellationToken = default,
            params string[] relations
        );
        Task<DbGetOneResult<TModel>> GetOne(
            Dictionary<string, object?> propertiesToMatch,
            CancellationToken cancellationToken = default,
            params string[] relations
        );
        Task<DbGetOneResult<TModel>> GetOne<T>(
            T value,
            string propertyName,
            CancellationToken cancellationToken = default,
            params string[] relations
        );
        Task<DbSaveResult<TModel>> Create(
            IReadOnlyCollection<TModel> entObj,
            CancellationToken cancellationToken = default
        );
        Task<DbSaveResult<TModel>> Create(
            TModel entObj,
            CancellationToken cancellationToken = default
        );
        Task<DbSaveResult<TModel>> Update(
            IReadOnlyCollection<TModel> entObj,
            CancellationToken cancellationToken = default
        );
        Task<DbSaveResult<TModel>> Update(
            TModel entObj,
            CancellationToken cancellationToken = default
        );
        Task<DbDeleteResult<TModel>> Delete(
            IReadOnlyCollection<TModel> entObj,
            CancellationToken cancellationToken = default
        );
        Task<DbDeleteResult<TModel>> Delete(
            TModel entObj,
            CancellationToken cancellationToken = default
        );
        Task<DbDeleteResult<TEntId>> Delete(
            IReadOnlyCollection<TEntId> entIds,
            CancellationToken cancellationToken = default
        );
        Task<DbResult<bool>> Exists<T>(
            T value,
            string propertyName,
            CancellationToken cancellationToken = default
        );
        Task<DbResult<bool>> Exists(TEntId entityId, CancellationToken cancellationToken = default);
        Task<DbResult<bool>> AnyExists(
            IReadOnlyCollection<TEntId> entityIds,
            CancellationToken cancellationToken = default
        );
    }
}
