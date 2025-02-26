using RE.Domain.Datamodel;
using RE.Domain.DataTransfertObjects;
using RE.Domain.Interfaces;
using RE.Domain.Interfaces.IServices;
using RE.Domain.Mapper;
using RE.Domain.Result;
using RE.Domain.Results;
using Serilog;

namespace RE.Application.Services;

public class RegisterService : ALoginService, IRegisterService
{
    public RegisterService(
        ILogger logger,
        IUserCommand command,
        IPasswordService passwordService,
        ILoginQuery query,
        IEmailService emailService
        ) : base(logger, command, passwordService, query, emailService)
    {
    }

    public async Task<IResult> Register(UserDto dto)
    {
        User user = new();
        IResult result = new Result();

        if (IsEmailUnique(dto.Email).IsFailure)
        {
            return Result.Failure(
                    Error.NotUnique(string.Format(
                        Message.GetResource("ErrorMessages", "CLIENT_UNICITY_CONSTRAINT"), "Email")));
        }

        string verificationToken = emailService.GenerateVerificationToken();

        var authentifier = passwordService.HashPassword(dto.Password);

        user.MapUser(dto, authentifier, verificationToken);

        result = await userCommand.Insert(user);

        await emailService.SendVerificationEmailAsync(dto.Email, verificationToken);

        return result;
    }

    private IResult IsEmailUnique(string email)
    {
        var result = loginQuery.CheckEmail(email).Result;

        if (result.IsFailure)
        {
            logger.Information(result.Error.Message);
        }

        return result;
    }
}
