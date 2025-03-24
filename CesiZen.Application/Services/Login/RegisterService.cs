using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CesiZen.Application.Services;

public class RegisterService : ALoginService, IRegisterService
{
    private readonly IConfiguration configuration;
    public RegisterService(
        IConfiguration configuration,
        ILogger logger,
        IUserCommand command,
        IPasswordService passwordService,
        ILoginQuery loginQuery,
        ITokenProvider tokenProvider
        ) : base(logger, command, passwordService, loginQuery, tokenProvider)
    {
        this.configuration = configuration;
    }

    public async Task<IResult<MessageEventArgs>> Register(NewUserDto dto)
    {
        User user;
        IResult result;

        if (IsEmailUnique(dto.Email).IsFailure)
        {
            return Result<MessageEventArgs>.Failure(UserErrors.ClientNotUnique(dto.Email));
        }

        string verificationToken = tokenProvider.GenerateVerificationToken();

        var authentifier = passwordService.HashPassword(dto.Password);

        user = dto.Map(authentifier, verificationToken);

        result = await userCommand.Insert(user);

        if (result.IsFailure)
        {
            return Result<MessageEventArgs>.Failure(UserErrors.ClientRegisterFailed);
        }

        var message = BuildEmailVerificationMessage(dto.Email, verificationToken);

        logger.Information(result.Info.Message);
        return Result<MessageEventArgs>.Success(message, UserInfos.ClientInsertionSucceeded);
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

    private MessageEventArgs BuildEmailVerificationMessage(string email, string token)
    {
        var template = Message.GetResource("Templates", "TEMPLATE_VERIFICATION_EMAIL");
        var verificationLink = $"{configuration["App:Url"]}/verify?token={token}";
        var htmlTemplate = template.Replace("{{url}}", verificationLink);

        return new MessageEventArgs
        {
            Email = email,
            Subject = Message.GetResource("Templates", "SUBJECT_VERIFICATION_EMAIL"),
            Body = htmlTemplate,
        };
    }
}
