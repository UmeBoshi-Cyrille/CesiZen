using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Mapper;

public static class TokenMapper
{

    public static RefreshTokenDto MapDto(this RefreshToken model)
    {
        return new RefreshTokenDto
        {
            Token = model.Token,
            ExpirationTime = model.ExpirationTime,
        };
    }
}

