using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using Serilog;

namespace CesiZen.Application.Services;

public class RegisterService : ALoginService, IRegisterService
{
    public RegisterService(
        ILogger logger,
        IUserCommand command,
        IPasswordService passwordService,
        ILoginQuery loginQuery,
        ITokenProvider tokenProvider
        ) : base(logger, command, passwordService, loginQuery, tokenProvider)
    {
    }

    public async Task<IResult<EmailSenderDto>> Register(UserDto dto)
    {
        User user;
        IResult result;

        if (IsEmailUnique(dto.Email).IsFailure)
        {
            return Result<EmailSenderDto>.Failure(UserErrors.ClientNotUnique(dto.Email));
        }

        string verificationToken = tokenProvider.GenerateVerificationToken();

        var authentifier = passwordService.HashPassword(dto.Password);

        user = dto.Map(authentifier, verificationToken);

        user.CreatedAt = DateTime.UtcNow;
        result = await userCommand.Insert(user);

        if (result.IsFailure)
        {
            return Result<EmailSenderDto>.Failure(UserErrors.ClientRegisterFailed);
        }

        var emailSender = UserMapper.MapEmailSender(dto.Email, "", verificationToken, "Email de vérification");

        logger.Information(result.Info.Message);
        return Result<EmailSenderDto>.Success(emailSender, UserInfos.ClientInsertionSucceeded);
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
