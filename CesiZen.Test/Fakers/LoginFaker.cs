using Bogus;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Test.Fakers;

public static class LoginFaker
{
    public static Faker<Login> FakeLoginGenerator(bool emailVerified = false, int failedCount = 0)
    {
        return new Faker<Login>()
            .RuleFor(a => a.Id, f => f.Random.Guid().ToString())
            .RuleFor(a => a.Email, f => f.Internet.Email())
            .RuleFor(a => a.Salt, "Salt")
            .RuleFor(a => a.Password, "Password1234=***$")
            .RuleFor(a => a.EmailVerified, emailVerified)
            .RuleFor(a => a.EmailVerificationToken, "token")
            .RuleFor(a => a.PasswordResetToken, "resetToken")
            .RuleFor(a => a.AccessFailedCount, failedCount);
    }

    public static Faker<AuthenticateRequestDto> FakeRequestDtoGenerator(string password = "Password1234=***$")
    {
        return new Faker<AuthenticateRequestDto>()
            .RuleFor(a => a.Identifier, f => f.Internet.Email())
            .RuleFor(a => a.Password, password);
    }

    public static Faker<AuthenticateResponseDto> FakeResponseDtoGenerator(bool loggedIn = false)
    {
        return new Faker<AuthenticateResponseDto>()
            .RuleFor(a => a.IsLoggedIn, loggedIn)
            .RuleFor(a => a.Token, "token");
    }
}
