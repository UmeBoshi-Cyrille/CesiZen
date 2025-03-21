using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
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
        User user;
        IResult result;

        if (IsEmailUnique(dto.Email).IsFailure)
        {
            return Result.Failure(UserErrors.ClientNotUnique(dto.Email));
        }

        string verificationToken = tokenProvider.GenerateVerificationToken();

        var authentifier = passwordService.HashPassword(dto.Password);

        user = dto.Map(authentifier, verificationToken);

        result = await userCommand.Insert(user);

        await emailService.SendVerificationEmailAsync(dto.Email, verificationToken);

        logger.Information(result.Info.Message);
        return Result.Success(UserInfos.ClientInsertionSucceeded);
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
