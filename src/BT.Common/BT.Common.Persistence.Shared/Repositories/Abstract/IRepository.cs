using BT.Common.Persistence.Shared.Entities;
using BT.Common.Persistence.Shared.Models;

namespace BT.Common.Persistence.Shared.Repositories.Abstract
{
    public interface IRepository<TEnt, TEntId, TModel>
        where TEnt : BaseEntity<TEntId, TModel>
        where TModel : class
    {
        Task<DbResult<int>> GetCount();
        Task<DbGetManyResult<TModel>> GetMany<T>(
            T value,
            string propertyName,
            params string[] relations
        );
        Task<DbGetManyResult<TModel>> GetMany(TEntId entityId, params string[] relations);
        Task<DbGetManyResult<TModel>> GetMany(IReadOnlyCollection<TEntId> entityIds, params string[] relations);
        Task<DbGetOneResult<TModel>> GetOne(TEntId entityId, params string[] relations);
        Task<DbGetOneResult<TModel>> GetOne<T>(
            T value,
            string propertyName,
            params string[] relations
        );
        Task<DbSaveResult<TModel>> Create(IReadOnlyCollection<TModel> entObj);
        Task<DbSaveResult<TModel>> Create(TModel entObj);
        Task<DbSaveResult<TModel>> Update(IReadOnlyCollection<TModel> entObj);
        Task<DbSaveResult<TModel>> Update(TModel entObj);
        Task<DbDeleteResult<TModel>> Delete(IReadOnlyCollection<TModel> entObj);
        Task<DbDeleteResult<TModel>> Delete(TModel entObj);
        Task<DbDeleteResult<TEntId>> Delete(IReadOnlyCollection<TEntId> entIds);
        Task<DbResult<bool>> Exists<T>(T value, string propertyName, params string[] relations);
        Task<DbResult<bool>> Exists(TEntId entityId);
    }
}
