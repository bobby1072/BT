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
    public abstract class BaseRepository<TEnt, TEntId, TModel, TDbContext> : IRepository<TEnt, TEntId, TModel>
        where TEnt : BaseEntity<TEntId, TModel>
        where TModel : class
        where TDbContext : DbContext
    {
        protected static readonly Type EntityType = typeof(TEnt);
        protected static readonly IReadOnlyCollection<PropertyInfo> EntityProperties =
            EntityType.GetProperties();
        protected readonly IDbContextFactory<TDbContext> ContextFactory;
        private readonly ILogger<BaseRepository<TEnt, TEntId, TModel, TDbContext>> _logger;

        protected BaseRepository(
            IDbContextFactory<TDbContext> dbContextFactory,
            ILogger<BaseRepository<TEnt, TEntId, TModel, TDbContext>> logger
        )
        {
            _logger = logger;
            ContextFactory =
                dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected abstract TEnt RuntimeToEntity(TModel runtimeObj);

        public virtual async Task<DbGetManyResult<TModel>> GetAll(params string[] relations)
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>(), relations);

            var allEnts = await TimeAndLogDbOperation(() => foundOneQuerySet.ToArrayAsync(),
                nameof(GetAll));

            return new DbGetManyResult<TModel>(allEnts?.FastArraySelect(x => x.ToModel()).ToArray());
        }
        public virtual async Task<DbResult<int>> GetCount()
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            var foundOneQuerySet = dbContext.Set<TEnt>();
            var count = await TimeAndLogDbOperation(
                () => foundOneQuerySet.CountAsync(),
                nameof(GetCount)
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
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>(), relations);
            var foundOne = await TimeAndLogDbOperation(
                () =>
                    foundOneQuerySet
                        .Where(x => EF.Property<T>(x, propertyName)!.Equals(value))
                        .ToArrayAsync(),
                nameof(GetMany)
            );

            return new DbGetManyResult<TModel>(
                foundOne?.FastArraySelect(x => x.ToModel()).ToArray()
            );
        }

        public virtual async Task<DbGetManyResult<TModel>> GetMany(IReadOnlyCollection<TEntId> entityIds, params string[] relations)
        {
            if (entityIds.Count < 1)
            {
                return new DbGetManyResult<TModel>();
            }

            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>(), relations);

            var foundOne = await TimeAndLogDbOperation(
                () => foundOneQuerySet.Where(x => entityIds.Contains(x.Id!)).ToArrayAsync(),
                nameof(GetMany)
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
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>(), relations);
            var foundOne = await TimeAndLogDbOperation(
                () => foundOneQuerySet.Where(x => x.Id!.Equals(entityId)).ToArrayAsync(),
                nameof(GetMany));

            return new DbGetManyResult<TModel>(
                foundOne?.FastArraySelect(x => x.ToModel()).ToArray()
            );
        }

        public virtual async Task<DbGetOneResult<TModel>> GetOne(
            TEntId entityId,
            params string[] relations
        )
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>(), relations);
            var foundOne = await TimeAndLogDbOperation(
                () => foundOneQuerySet.FirstOrDefaultAsync(x => x.Id!.Equals(entityId)),
                nameof(GetOne)
            );

            return new DbGetOneResult<TModel>(foundOne?.ToModel());
        }

        public virtual async Task<DbResult<bool>> Exists(TEntId entityId)
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            var foundOneQuerySet = dbContext.Set<TEnt>();
            var foundOne = await TimeAndLogDbOperation(
                () => foundOneQuerySet.AnyAsync(x => x.Id!.Equals(entityId)),
                nameof(Exists));

            return new DbResult<bool>(true, foundOne);
        }

        public virtual async Task<DbResult<bool>> Exists<T>(
            T value,
            string propertyName
        )
        {
            ThrowIfPropertyDoesNotExist<T>(propertyName);
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>());
            var foundOne = await TimeAndLogDbOperation(
                () => foundOneQuerySet.AnyAsync(x => EF.Property<T>(x, propertyName)!.Equals(value)),
                nameof(Exists)
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
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>(), relations);
            var foundOne = await TimeAndLogDbOperation(
                () =>
                    foundOneQuerySet.FirstOrDefaultAsync(x =>
                        EF.Property<T>(x, propertyName)!.Equals(value)
                    ),
                nameof(GetOne)
            );

            return new DbGetOneResult<TModel>(foundOne?.ToModel());
        }

        public virtual async Task<DbSaveResult<TModel>> Create(IReadOnlyCollection<TModel> entObj)
        {
            if (entObj.Count < 1)
            {
                return new DbSaveResult<TModel>();
            }
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            var set = dbContext.Set<TEnt>();
            async Task<TModel?> Operation()
            {
                await set.AddRangeAsync(entObj.FastArraySelect(x => RuntimeToEntity(x)));
                await dbContext.SaveChangesAsync();
                return null;
            }
            await TimeAndLogDbOperation(Operation, nameof(Create));
            var runtimeObjs = set.Local.FastArraySelect(x => x.ToModel());
            return new DbSaveResult<TModel>(runtimeObjs.ToArray());
        }

        public virtual Task<DbSaveResult<TModel>> Create(TModel entObj) => Create([entObj]);
        public virtual async Task<DbDeleteResult<TModel>> Delete(IReadOnlyCollection<TModel> entObj)
        {
            if (entObj.Count < 1)
            {
                return new DbDeleteResult<TModel>();
            }
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            var set = dbContext.Set<TEnt>();
            async Task<TModel?> Operation()
            {
                set.RemoveRange(entObj.FastArraySelect(x => RuntimeToEntity(x)));
                await dbContext.SaveChangesAsync();
                return null;
            }
            await TimeAndLogDbOperation(Operation, nameof(Delete));
            return new DbDeleteResult<TModel>(entObj);
        }

        public virtual async Task<DbDeleteResult<TEntId>> Delete(IReadOnlyCollection<TEntId> entIds)
        {
            if (entIds.Count < 1)
            {
                return new DbDeleteResult<TEntId>();
            }
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            var set = dbContext.Set<TEnt>();
            async Task<TEntId?> Operation()
            {
                await set.Where(x => entIds.Contains(x.Id!)).ExecuteDeleteAsync();
                await dbContext.SaveChangesAsync();
                return default;
            }
            await TimeAndLogDbOperation(Operation, nameof(Delete));

            return new DbDeleteResult<TEntId>(entIds);
        }

        public virtual Task<DbDeleteResult<TModel>> Delete(TModel entObj) => Delete([entObj]);

        public virtual async Task<DbSaveResult<TModel>> Update(IReadOnlyCollection<TModel> entObj)
        {
            if (entObj.Count < 1)
            {
                return new DbSaveResult<TModel>();
            }
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            var set = dbContext.Set<TEnt>();
            async Task<TModel?> Operation()
            {
                set.UpdateRange(entObj.FastArraySelect(x => RuntimeToEntity(x)));
                await dbContext.SaveChangesAsync();
                return null;
            }
            await TimeAndLogDbOperation(Operation, nameof(Update));

            var runtimeObjs = set.Local.FastArraySelect(x => x.ToModel());
            return new DbSaveResult<TModel>(runtimeObjs.ToArray());
        }

        public virtual Task<DbSaveResult<TModel>> Update(TModel entObj) => Update([entObj]);

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

        protected async Task TimeAndLogDbTransaction(params Func<IQueryable<TEnt>, Task>[] actions)
        {
            if (actions.Length < 1)
            {
                return;
            }
            
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var dbSet = dbContext.Set<TEnt>();
                foreach (var action in actions)
                {
                    await TimeAndLogDbOperation<bool>(async () =>
                    {
                        await action.Invoke(dbSet);
                        await dbContext.SaveChangesAsync();
                        return true;
                    }, nameof(TimeAndLogDbTransaction));
                    
                    await dbContext.SaveChangesAsync();
                }
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        protected async Task<T> TimeAndLogDbOperation<T>(
            Func<Task<T>> func,
            string operationName,
            string? entityId = null
        )
        {
            var logMessageBuilder = new StringBuilder("Performing {OperationName} on {EntityName}");
            if (entityId != null)
            {
                logMessageBuilder.Append(" with id {EntityId}");
            }
            _logger.LogDebug(logMessageBuilder.ToString(), operationName, entityId);

            var (timeTaken, result) = await OperationTimerUtils.TimeWithResultsAsync(func);

            _logger.LogDebug(
                "finished {OperationName} on {EntityName} in {TimeTaken}ms",
                operationName,
                entityId is not null ? $"{EntityType.Name} with id {entityId}" : EntityType.Name,
                timeTaken.TotalMilliseconds
            );

            return result;
        }

        private static bool DoesPropertyExist<T>(string propertyName)
        {
            return EntityProperties.Any(x =>
                x.Name == propertyName && x.PropertyType == typeof(T)
            );
        }

        private static void ThrowIfPropertyDoesNotExist<T>(string propertyName)
        {
            if (!DoesPropertyExist<T>(propertyName))
            {
                throw new ArgumentException(
                    $"Property {propertyName} does not exist on entity {EntityType.Name}"
                );
            }
        }
    }
}
