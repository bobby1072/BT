using System.Reflection;
using System.Text;
using BT.Common.FastArray.Proto;
using BT.Common.OperationTimer.Proto;
using BT.Common.Persistence.Shared.Entities;
using BT.Common.Persistence.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BT.Common.Persistence.Shared.Repositories.Abstract
{
    internal abstract class BaseRepository<TEnt, TEntId, TModel, TDbContext> : IRepository<TEnt, TEntId, TModel>
        where TEnt : BaseEntity<TEntId, TModel>
        where TModel : class
        where TDbContext : DbContext
    {
        protected readonly IDbContextFactory<TDbContext> _contextFactory;
        private readonly ILogger<BaseRepository<TEnt, TEntId, TModel, TDbContext>> _logger;
        protected static readonly Type _entityType = typeof(TEnt);
        protected static readonly IReadOnlyCollection<PropertyInfo> _entityProperties =
            _entityType.GetProperties();

        protected BaseRepository(
            IDbContextFactory<TDbContext> dbContextFactory,
            ILogger<BaseRepository<TEnt, TEntId, TModel, TDbContext>> logger
        )
        {
            _logger = logger;
            _contextFactory =
                dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected abstract TEnt RuntimeToEntity(TModel runtimeObj);

        public virtual async Task<DbResult<int>> GetCount()
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var foundOneQuerySet = dbContext.Set<TEnt>();
            var count = await TimeAndLogDbOperation(
                () => foundOneQuerySet.CountAsync(),
                nameof(GetCount),
                _entityType.Name
            );

            return new DbResult<int>(true, count);
        }

        public virtual async Task<DbGetManyResult<TModel>> GetMany<T>(
            T value,
            string propertyName,
            params string[] relations
        )
        {
            ThrowIfPropertyDoesNotExist<T>(propertyName);
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>());
            var foundOne = await TimeAndLogDbOperation(
                () =>
                    foundOneQuerySet
                        .Where(x => EF.Property<T>(x, propertyName).Equals(value))
                        .ToArrayAsync(),
                nameof(GetMany),
                _entityType.Name
            );

            return new DbGetManyResult<TModel>(
                foundOne?.FastArraySelect(x => x.ToModel()).ToArray()
            );
        }

        public virtual async Task<DbGetManyResult<TModel>> GetMany(params TEntId[] entityIds)
        {
            if (entityIds.Length > 1)
            {
                return new DbGetManyResult<TModel>();
            }

            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>());

            var foundOne = await TimeAndLogDbOperation(
                () => foundOneQuerySet.Where(x => entityIds.Contains(x.Id!)).ToArrayAsync(),
                nameof(GetMany),
                _entityType.Name
            );

            return new DbGetManyResult<TModel>(
                foundOne?.FastArraySelect(x => x.ToModel()).ToArray()
            );
        }

        public virtual async Task<DbGetManyResult<TModel>> GetMany(
            TEntId entityId,
            params string[] relations
        )
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>());
            var foundOne = await TimeAndLogDbOperation(
                () => foundOneQuerySet.Where(x => x.Id!.Equals(entityId)).ToArrayAsync(),
                nameof(GetMany),
                _entityType.Name
            );

            return new DbGetManyResult<TModel>(
                foundOne?.FastArraySelect(x => x.ToModel()).ToArray()
            );
        }

        public virtual async Task<DbGetOneResult<TModel>> GetOne(
            TEntId entityId,
            params string[] relations
        )
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>());
            var foundOne = await TimeAndLogDbOperation(
                () => foundOneQuerySet.FirstOrDefaultAsync(x => x.Id!.Equals(entityId)),
                nameof(GetOne),
                _entityType.Name
            );

            return new DbGetOneResult<TModel>(foundOne?.ToModel());
        }

        public virtual async Task<DbResult<bool>> Exists(TEntId entityId)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var foundOneQuerySet = dbContext.Set<TEnt>();
            var foundOne = await TimeAndLogDbOperation(
                () => foundOneQuerySet.AnyAsync(x => x.Id!.Equals(entityId)),
                nameof(Exists),
                _entityType.Name
            );

            return new DbResult<bool>(true, foundOne);
        }

        public virtual async Task<DbResult<bool>> Exists<T>(
            T value,
            string propertyName,
            params string[] relations
        )
        {
            ThrowIfPropertyDoesNotExist<T>(propertyName);
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>());
            var foundOne = await TimeAndLogDbOperation(
                () => foundOneQuerySet.AnyAsync(x => EF.Property<T>(x, propertyName).Equals(value)),
                nameof(Exists),
                _entityType.Name
            );

            return new DbResult<bool>(true, foundOne);
        }

        public virtual async Task<DbGetOneResult<TModel>> GetOne<T>(
            T value,
            string propertyName,
            params string[] relations
        )
        {
            ThrowIfPropertyDoesNotExist<T>(propertyName);
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>());
            var foundOne = await TimeAndLogDbOperation(
                () =>
                    foundOneQuerySet.FirstOrDefaultAsync(x =>
                        EF.Property<T>(x, propertyName).Equals(value)
                    ),
                nameof(GetOne),
                _entityType.Name
            );

            return new DbGetOneResult<TModel>(foundOne?.ToModel());
        }

        public virtual async Task<DbSaveResult<TModel>> Create(IReadOnlyCollection<TModel> entObj)
        {
            if (entObj.Count < 1)
            {
                return new DbSaveResult<TModel>();
            }
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var set = dbContext.Set<TEnt>();
            async Task<TModel?> operation()
            {
                await set.AddRangeAsync(entObj.FastArraySelect(x => RuntimeToEntity(x)));
                await dbContext.SaveChangesAsync();
                return null;
            }
            await TimeAndLogDbOperation(operation, nameof(Create), _entityType.Name);
            var runtimeObjs = set.Local.FastArraySelect(x => x.ToModel());
            return new DbSaveResult<TModel>(runtimeObjs.ToArray());
        }

        public virtual async Task<DbDeleteResult<TModel>> Delete(IReadOnlyCollection<TModel> entObj)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var set = dbContext.Set<TEnt>();
            async Task<TModel?> operation()
            {
                set.RemoveRange(entObj.FastArraySelect(x => RuntimeToEntity(x)));
                await dbContext.SaveChangesAsync();
                return null;
            }
            await TimeAndLogDbOperation(operation, nameof(Delete), _entityType.Name);
            return new DbDeleteResult<TModel>(entObj);
        }

        public virtual async Task<DbDeleteResult<TEntId>> Delete(IReadOnlyCollection<TEntId> entIds)
        {
            if (entIds.Count < 1)
            {
                return new DbDeleteResult<TEntId>();
            }
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var set = dbContext.Set<TEnt>();
            async Task<TEntId?> operation()
            {
                await set.Where(x => entIds.Contains(x.Id!)).ExecuteDeleteAsync();
                await dbContext.SaveChangesAsync();
                return default;
            }
            await TimeAndLogDbOperation(operation, nameof(Delete), _entityType.Name);

            return new DbDeleteResult<TEntId>(entIds);
        }

        public virtual async Task<DbSaveResult<TModel>> Update(IReadOnlyCollection<TModel> entObj)
        {
            if (entObj.Count < 1)
            {
                return new DbSaveResult<TModel>();
            }
            await using var dbContext = await _contextFactory.CreateDbContextAsync();
            var set = dbContext.Set<TEnt>();
            async Task<TModel?> operation()
            {
                set.UpdateRange(entObj.FastArraySelect(x => RuntimeToEntity(x)));
                await dbContext.SaveChangesAsync();
                return null;
            }
            await TimeAndLogDbOperation(operation, nameof(Update), _entityType.Name);

            var runtimeObjs = set.Local.FastArraySelect(x => x.ToModel());
            return new DbSaveResult<TModel>(runtimeObjs.ToArray());
        }

        protected IQueryable<TEnt> AddRelationsToSet(
            IQueryable<TEnt> set,
            params string[] relations
        )
        {
            foreach (var relation in relations)
            {
                set = set.Include(relation);
            }

            return set;
        }

        protected async Task<T> TimeAndLogDbOperation<T>(
            Func<Task<T>> func,
            string operationName,
            string entityName,
            string? entityId = null
        )
        {
            var logMessageBuilder = new StringBuilder("Performing {OperationName} on {EntityName}");
            if (entityId != null)
            {
                logMessageBuilder.Append(" with id {EntityId}");
            }
            _logger.LogDebug(logMessageBuilder.ToString(), operationName, entityName, entityId);

            var (timeTaken, result) = await OperationTimerUtils.TimeWithResultsAsync(func);

            _logger.LogDebug(
                "finished {OperationName} on {EntityName} in {TimeTaken}ms",
                operationName,
                entityId is not null ? $"{entityName} with id {entityId}" : entityName,
                timeTaken.TotalMilliseconds
            );

            return result;
        }

        protected async Task TimeAndLogDbOperation(
            Func<Task> func,
            string operationName,
            string entityName,
            string? entityId = null
        )
        {
            var logMessageBuilder = new StringBuilder("Performing {OperationName} on {EntityName}");
            if (entityId != null)
            {
                logMessageBuilder.Append(" with id {EntityId}");
            }
            _logger.LogDebug(logMessageBuilder.ToString(), operationName, entityName, entityId);

            var (timeTaken, result) = await OperationTimerUtils.TimeWithResultsAsync(func);

            _logger.LogDebug(
                "finished {OperationName} on {EntityName} in {TimeTaken}ms",
                operationName,
                entityId is not null ? $"{entityName} with id {entityId}" : entityName,
                timeTaken.TotalMilliseconds
            );
        }

        private static bool DoesPropertyExist<T>(string propertyName)
        {
            return _entityProperties.Any(x =>
                x.Name == propertyName && x.PropertyType == typeof(T)
            );
        }

        private static void ThrowIfPropertyDoesNotExist<T>(string propertyName)
        {
            if (!DoesPropertyExist<T>(propertyName))
            {
                throw new ArgumentException(
                    $"Property {propertyName} does not exist on entity {_entityType.Name}"
                );
            }
        }
    }
}
