using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using Serilog;

namespace CesiZen.Application.Services;

public class LoginQueryService : AService, ILoginQueryService
{
    private readonly ILoginQuery query;

    public LoginQueryService(ILoginQuery query, ILogger logger) : base(logger)
    {
        this.query = query;
    }

    public Task<IResult<Login>> GetByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<IResult<Login>> GetByUserId(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<IResult<int>> GetUserIdByEmail(string email)
    {
        var result = await query.GetUserIdByEmail(email);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<int>.Failure(LoginErrors.EmailVerificationFailed);
        }

        return Result<int>.Success(result.Value);
    }
}
