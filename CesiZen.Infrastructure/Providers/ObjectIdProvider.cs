using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using MongoDB.Bson;

namespace CesiZen.Infrastructure.Providers;

internal class ObjectIdProvider : ValueGenerator<string>
{
    public override string Next(EntityEntry entry)
    {
        return ObjectId.GenerateNewId().ToString();
    }

    public override bool GeneratesTemporaryValues => false;
}
