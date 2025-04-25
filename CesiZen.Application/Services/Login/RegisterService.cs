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

        user = dto.Map(authentifier, verificationToken, configuration);

        result = await userCommand.Insert(user);

        if (result.IsFailure)
        {
            return Result<MessageEventArgs>.Failure(UserErrors.ClientRegisterFailed);
        }

        var message = BuildEmailVerificationMessage(dto.Email, verificationToken, configuration);

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

    internal static MessageEventArgs BuildEmailVerificationMessage(string email, string token, IConfiguration configuration)
    {
        var template = ResourceMessages.GetResource("Templates", "TEMPLATE_VERIFICATION_EMAIL");
        string encodedToken = Uri.EscapeDataString(token!);
        string encodedEmail = Uri.EscapeDataString(email);
        var verificationLink = $"{configuration["App:Url"]}/verify?token={encodedToken}&email={encodedEmail}";
        var htmlTemplate = template.Replace("{{url}}", verificationLink);

        return new MessageEventArgs
        {
            Email = email,
            Subject = ResourceMessages.GetResource("Templates", "SUBJECT_VERIFICATION_EMAIL"),
            Body = htmlTemplate,
        };
    }
}
