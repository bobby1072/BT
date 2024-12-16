﻿namespace BT.Common.Persistence.Shared.Models
{
    public record DbGetOneResult<TModel> : DbResult<TModel?> where TModel : class
    {
        public DbGetOneResult(TModel? model = null) : base(model is null, model) { }
    }
}
