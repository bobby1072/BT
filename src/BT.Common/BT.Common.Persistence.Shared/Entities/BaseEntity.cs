using System.ComponentModel.DataAnnotations;

namespace BT.Common.Persistence.Shared.Entities
{
    public abstract record BaseEntity<TId, TRuntime>
        where TRuntime : class
    {
        [Key]
        public virtual TId Id { get; set; }
        public abstract TRuntime ToModel();
    }
}
