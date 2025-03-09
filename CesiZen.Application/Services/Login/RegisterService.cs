using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interface;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using Serilog;

namespace CesiZen.Application.Services;

public class RegisterService : ALoginService, IRegisterService
{
    private readonly ILoginCommand loginCommand;

    public RegisterService(
        ILogger logger,
        IUserCommand command,
        IPasswordService passwordService,
        ILoginQuery query,
        ILoginCommand loginCommand,
        IEmailService emailService,
        ITokenProvider tokenProvider
        ) : base(logger, command, passwordService, query, emailService, tokenProvider)
    {
        this.loginCommand = loginCommand;
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

        string verificationToken = tokenProvider.GenerateVerificationToken();

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
