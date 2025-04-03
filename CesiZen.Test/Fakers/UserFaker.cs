using Bogus;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Test.Fakers;

internal class UserFaker
{
    public static Faker<User> FakeUserGenerator()
    {
        return new Faker<User>()
            .RuleFor(a => a.Id, f => f.Random.Int(1, 200))
            .RuleFor(a => a.Firstname, f => f.Name.FirstName())
            .RuleFor(a => a.Lastname, f => f.Name.LastName())
            .RuleFor(a => a.CreatedAt, f => f.Date.Past())
            .RuleFor(a => a.UpdatedAt, f => f.Date.Recent())
            .RuleFor(a => a.IsActive, f => f.Random.Bool());
    }

    public static Faker<UserAccountDto> FakeDtoGenerator()
    {
        return new Faker<UserAccountDto>()
            .RuleFor(a => a.Firstname, f => f.Name.FirstName())
            .RuleFor(a => a.Lastname, f => f.Name.LastName())
            .RuleFor(a => a.Username, f => f.Name.LastName())
            .RuleFor(a => a.Email, f => f.Internet.Email())
            .RuleFor(a => a.Password, "Password1234=***$");
    }

    public static Faker<UserMinimumDto> FakeRequestDtoGenerator()
    {
        return new Faker<UserMinimumDto>()
            .RuleFor(a => a.Id, f => f.Random.Int(1, 200))
            .RuleFor(a => a.Firstname, f => f.Name.FirstName())
            .RuleFor(a => a.Lastname, f => f.Name.LastName())
            .RuleFor(a => a.IsActive, f => f.Random.Bool());
    }
}
