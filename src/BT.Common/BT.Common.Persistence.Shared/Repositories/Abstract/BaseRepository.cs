using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using BT.Common.Persistence.Shared.Entities;
using BT.Common.Persistence.Shared.Models;
using BT.Common.Polly.Extensions;
using BT.Common.Polly.Models.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BT.Common.Persistence.Shared.Repositories.Abstract
{
    public abstract class BaseRepository<TEnt, TEntId, TModel, TDbContext>
        : IRepository<TEnt, TEntId, TModel>
        where TEnt : BaseEntity<TEntId, TModel>
        where TModel : class
        where TDbContext : DbContext
    {
        protected static readonly Type EntityType = typeof(TEnt);
        protected static readonly IReadOnlyCollection<PropertyInfo> EntityProperties =
            EntityType.GetProperties();
        protected readonly IDbContextFactory<TDbContext> ContextFactory;
        private readonly ILogger<BaseRepository<TEnt, TEntId, TModel, TDbContext>> _logger;
        private readonly IPollyRetrySettings? _pollyRetrySettings;

        protected BaseRepository(
            IDbContextFactory<TDbContext> dbContextFactory,
            ILogger<BaseRepository<TEnt, TEntId, TModel, TDbContext>> logger,
            IPollyRetrySettings? pollyRetrySettings = null
        )
        {
            _logger = logger;
            ContextFactory =
                dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _pollyRetrySettings = pollyRetrySettings;
        }

        protected abstract TEnt RuntimeToEntity(TModel runtimeObj);

        public virtual async Task<DbGetManyResult<TModel>> GetAll(
            CancellationToken cancellationToken = default,
            params string[] relations
        )
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>(), relations);

            var allEnts = await TimeAndLogDbOperation(
                ct => foundOneQuerySet.ToArrayAsync(ct),
                nameof(GetAll),
                cancellationToken
            );

            return new DbGetManyResult<TModel>(allEnts?.Select(x => x.ToModel()).ToArray());
        }

        public virtual async Task<DbResult<int>> GetCount(
            Expression<Func<TEnt, bool>>? predicate = null,
            CancellationToken cancellationToken = default
        )
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            var foundOneQuerySet = dbContext.Set<TEnt>();
            var count = await TimeAndLogDbOperation(
                ct =>
                    predicate is null
                        ? foundOneQuerySet.CountAsync(ct)
                        : foundOneQuerySet.CountAsync(predicate, ct),
                nameof(GetCount),
                cancellationToken
            );

            return new DbResult<int>(true, count);
        }

        public virtual async Task<DbGetManyResult<TModel>> GetMany(
            Dictionary<string, object?> propertiesToMatch,
            CancellationToken cancellationToken = default,
            params string[] relations
        )
        {
            ThrowIfPropertyDoesNotExist(propertiesToMatch.Keys);
            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>(), relations);

            foreach (var kvp in propertiesToMatch)
            {
                var propertyName = kvp.Key;
                var value = kvp.Value;
                foundOneQuerySet = foundOneQuerySet.Where(x =>
                    EF.Property<object>(x, propertyName).Equals(value)
                );
            }

            var foundMany = await TimeAndLogDbOperation(
                ct => foundOneQuerySet.ToArrayAsync(ct),
                nameof(GetMany),
                cancellationToken
            );

            return new DbGetManyResult<TModel>(foundMany?.Select(x => x.ToModel()).ToArray());
        }

        public virtual async Task<DbGetOneResult<TModel>> GetOne(
            Dictionary<string, object?> propertiesToMatch,
            CancellationToken cancellationToken = default,
            params string[] relations
        )
        {
            ThrowIfPropertyDoesNotExist(propertiesToMatch.Keys);
            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>(), relations);

            foreach (var kvp in propertiesToMatch)
            {
                var propertyName = kvp.Key;
                var value = kvp.Value;
                foundOneQuerySet = foundOneQuerySet.Where(x =>
                    EF.Property<object>(x, propertyName).Equals(value)
                );
            }

            var foundOne = await TimeAndLogDbOperation(
                ct => foundOneQuerySet.FirstOrDefaultAsync(ct),
                nameof(GetOne),
                cancellationToken
            );

            return new DbGetOneResult<TModel>(foundOne?.ToModel());
        }

        public virtual async Task<DbGetManyResult<TModel>> GetMany<T>(
            T value,
            string propertyName,
            CancellationToken cancellationToken = default,
            params string[] relations
        )
        {
            ThrowIfPropertyDoesNotExist<T>(propertyName);
            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>(), relations);
            var foundOne = await TimeAndLogDbOperation(
                ct =>
                    foundOneQuerySet
                        .Where(x => EF.Property<T>(x, propertyName)!.Equals(value))
                        .ToArrayAsync(ct),
                nameof(GetMany),
                cancellationToken
            );

            return new DbGetManyResult<TModel>(foundOne?.Select(x => x.ToModel()).ToArray());
        }

        public virtual async Task<DbGetManyResult<TModel>> GetMany(
            IReadOnlyCollection<TEntId> entityIds,
            CancellationToken cancellationToken = default,
            params string[] relations
        )
        {
            if (entityIds.Count < 1)
            {
                return new DbGetManyResult<TModel>();
            }

            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>(), relations);

            var foundOne = await TimeAndLogDbOperation(
                ct => foundOneQuerySet.Where(x => entityIds.Contains(x.Id!)).ToArrayAsync(ct),
                nameof(GetMany),
                cancellationToken
            );

            return new DbGetManyResult<TModel>(foundOne?.Select(x => x.ToModel()).ToArray());
        }

        public virtual async Task<DbGetManyResult<TModel>> GetMany(
            TEntId entityId,
            CancellationToken cancellationToken = default,
            params string[] relations
        )
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>(), relations);
            var foundOne = await TimeAndLogDbOperation(
                ct => foundOneQuerySet.Where(x => x.Id!.Equals(entityId)).ToArrayAsync(ct),
                nameof(GetMany),
                cancellationToken
            );

            return new DbGetManyResult<TModel>(foundOne?.Select(x => x.ToModel()).ToArray());
        }

        public virtual async Task<DbGetOneResult<TModel>> GetOne(
            TEntId entityId,
            CancellationToken cancellationToken = default,
            params string[] relations
        )
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>(), relations);
            var foundOne = await TimeAndLogDbOperation(
                ct => foundOneQuerySet.FirstOrDefaultAsync(x => x.Id!.Equals(entityId), ct),
                nameof(GetOne),
                cancellationToken
            );

            return new DbGetOneResult<TModel>(foundOne?.ToModel());
        }

        public virtual async Task<DbResult<bool>> AnyExists(
            IReadOnlyCollection<TEntId> entityIds,
            CancellationToken cancellationToken = default
        )
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            var foundOneQuerySet = dbContext.Set<TEnt>();

            var anyExists = await TimeAndLogDbOperation(
                ct => foundOneQuerySet.AnyAsync(x => entityIds.Contains(x.Id!), ct),
                nameof(AnyExists),
                cancellationToken
            );

            return new DbResult<bool>(true, anyExists);
        }

        public virtual async Task<DbResult<bool>> Exists(
            TEntId entityId,
            CancellationToken cancellationToken = default
        )
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            var foundOneQuerySet = dbContext.Set<TEnt>();
            var foundOne = await TimeAndLogDbOperation(
                ct => foundOneQuerySet.AnyAsync(x => x.Id!.Equals(entityId), ct),
                nameof(Exists),
                cancellationToken
            );

            return new DbResult<bool>(true, foundOne);
        }

        public virtual async Task<DbResult<bool>> Exists<T>(
            T value,
            string propertyName,
            CancellationToken cancellationToken = default
        )
        {
            ThrowIfPropertyDoesNotExist<T>(propertyName);
            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>());
            var foundOne = await TimeAndLogDbOperation(
                ct =>
                    foundOneQuerySet.AnyAsync(
                        x => EF.Property<T>(x, propertyName)!.Equals(value),
                        ct
                    ),
                nameof(Exists),
                cancellationToken
            );

            return new DbResult<bool>(true, foundOne);
        }

        public virtual async Task<DbGetOneResult<TModel>> GetOne<T>(
            T value,
            string propertyName,
            CancellationToken cancellationToken = default,
            params string[] relations
        )
        {
            ThrowIfPropertyDoesNotExist<T>(propertyName);
            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            var foundOneQuerySet = AddRelationsToSet(dbContext.Set<TEnt>(), relations);
            var foundOne = await TimeAndLogDbOperation(
                ct =>
                    foundOneQuerySet.FirstOrDefaultAsync(
                        x => EF.Property<T>(x, propertyName)!.Equals(value),
                        ct
                    ),
                nameof(GetOne),
                cancellationToken
            );

            return new DbGetOneResult<TModel>(foundOne?.ToModel());
        }

        public virtual async Task<DbSaveResult<TModel>> Create(
            IReadOnlyCollection<TModel> entObj,
            CancellationToken cancellationToken = default
        )
        {
            if (entObj.Count < 1)
            {
                return new DbSaveResult<TModel>();
            }
            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            var set = dbContext.Set<TEnt>();
            async Task<TModel?> Operation(CancellationToken ct)
            {
                await set.AddRangeAsync(entObj.Select(x => RuntimeToEntity(x)), ct);
                await dbContext.SaveChangesAsync(ct);
                return null;
            }
            await TimeAndLogDbOperation(Operation, nameof(Create), cancellationToken);
            var runtimeObjs = set.Local.Select(x => x.ToModel());
            return new DbSaveResult<TModel>(runtimeObjs.ToArray());
        }

        public virtual Task<DbSaveResult<TModel>> Create(
            TModel entObj,
            CancellationToken cancellationToken = default
        ) => Create([entObj], cancellationToken);

        public virtual async Task<DbDeleteResult<TModel>> Delete(
            IReadOnlyCollection<TModel> entObj,
            CancellationToken cancellationToken = default
        )
        {
            if (entObj.Count < 1)
            {
                return new DbDeleteResult<TModel>();
            }
            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            var set = dbContext.Set<TEnt>();
            async Task<TModel?> Operation(CancellationToken ct)
            {
                set.RemoveRange(entObj.Select(x => RuntimeToEntity(x)));
                await dbContext.SaveChangesAsync(ct);
                return null;
            }
            await TimeAndLogDbOperation(Operation, nameof(Delete), cancellationToken);
            return new DbDeleteResult<TModel>(entObj);
        }

        public virtual async Task<DbDeleteResult<TEntId>> Delete(
            IReadOnlyCollection<TEntId> entIds,
            CancellationToken cancellationToken = default
        )
        {
            if (entIds.Count < 1)
            {
                return new DbDeleteResult<TEntId>();
            }
            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            var set = dbContext.Set<TEnt>();
            async Task<TEntId?> Operation(CancellationToken ct)
            {
                await set.Where(x => entIds.Contains(x.Id!)).ExecuteDeleteAsync(ct);
                await dbContext.SaveChangesAsync(ct);
                return default;
            }
            await TimeAndLogDbOperation(Operation, nameof(Delete), cancellationToken);

            return new DbDeleteResult<TEntId>(entIds);
        }

        public virtual Task<DbDeleteResult<TModel>> Delete(
            TModel entObj,
            CancellationToken cancellationToken = default
        ) => Delete([entObj], cancellationToken);

        public virtual async Task<DbSaveResult<TModel>> Update(
            IReadOnlyCollection<TModel> entObj,
            CancellationToken cancellationToken = default
        )
        {
            if (entObj.Count < 1)
            {
                return new DbSaveResult<TModel>();
            }
            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            var set = dbContext.Set<TEnt>();
            async Task<TModel?> Operation(CancellationToken ct)
            {
                set.UpdateRange(entObj.Select(x => RuntimeToEntity(x)));
                await dbContext.SaveChangesAsync(ct);
                return null;
            }
            await TimeAndLogDbOperation(Operation, nameof(Update), cancellationToken);

            var runtimeObjs = set.Local.Select(x => x.ToModel());
            return new DbSaveResult<TModel>(runtimeObjs.ToArray());
        }

        public virtual Task<DbSaveResult<TModel>> Update(
            TModel entObj,
            CancellationToken cancellationToken = default
        ) => Update([entObj], cancellationToken);

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

        protected async Task TimeAndLogDbTransaction(
            CancellationToken cancellationToken = default,
            params Func<IQueryable<TEnt>, CancellationToken, Task>[] actions
        )
        {
            if (actions.Length < 1)
            {
                return;
            }

            await using var dbContext = await ContextFactory.CreateDbContextAsync(
                cancellationToken
            );
            await using var transaction = await dbContext.Database.BeginTransactionAsync(
                cancellationToken
            );
            try
            {
                var dbSet = dbContext.Set<TEnt>();
                foreach (var action in actions)
                {
                    await TimeAndLogDbOperation<bool>(
                        async ct =>
                        {
                            await action.Invoke(dbSet, ct);
                            await dbContext.SaveChangesAsync(ct);
                            return true;
                        },
                        nameof(TimeAndLogDbTransaction),
                        cancellationToken
                    );

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        protected async Task<T> TimeAndLogDbOperation<T>(
            Func<CancellationToken, Task<T>> func,
            string operationName,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogDebug(
                "Performing {OperationName} on {EntityName}",
                operationName,
                EntityType.Name
            );

            var retryPipeline = _pollyRetrySettings?.ToPipeline();

            Func<Task<T>> opToInvoke = retryPipeline is not null
                ? () =>
                    retryPipeline
                        .ExecuteAsync(async ct => await func(ct), cancellationToken)
                        .AsTask()
                : () => func(cancellationToken);

            var stopWatch = Stopwatch.StartNew();
            var result = await opToInvoke.Invoke();
            stopWatch.Stop();

            _logger.LogDebug(
                "finished {OperationName} on {EntityName} in {TimeTaken}ms",
                operationName,
                EntityType.Name,
                stopWatch.ElapsedMilliseconds
            );

            return result;
        }

        private static bool DoesPropertyExist<T>(string propertyName)
        {
            return EntityProperties.Any(x => x.Name == propertyName && x.PropertyType == typeof(T));
        }

        private static bool DoesPropertyExist(string propertyName)
        {
            return EntityProperties.Any(x => x.Name == propertyName);
        }

        private static void ThrowIfPropertyDoesNotExist(IReadOnlyCollection<string> propertyNames)
        {
            if (propertyNames.Any(x => !DoesPropertyExist(x)))
            {
                throw new ArgumentException(
                    $"Properties does not exist on entity {EntityType.Name}"
                );
            }
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
