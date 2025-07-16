using System.ComponentModel.DataAnnotations;

namespace BT.Common.Persistence.Shared.Entities
{
    public abstract class BaseEntity<TId, TRuntime>
        where TRuntime : class
    {
        #nullable disable
        [Key]
        public virtual TId Id { get; set; }
        #nullable enable
        public abstract TRuntime ToModel();
    }
}
