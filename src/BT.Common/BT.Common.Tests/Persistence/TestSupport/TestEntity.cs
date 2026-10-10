using BT.Common.Persistence.Shared.Attributes;
using BT.Common.Persistence.Shared.Entities;

namespace BT.Common.Tests.Persistence.TestSupport;

[Cacheable]
public sealed class TestEntity : BaseEntity<Guid, TestModel>
{
    public string Name { get; set; } = string.Empty;

    public static TestEntity FromModel(TestModel model) =>
        new() { Id = model.Id, Name = model.Name };

    public override TestModel ToModel() => new(Id, Name);
}

public sealed record TestModel(Guid Id, string Name);
