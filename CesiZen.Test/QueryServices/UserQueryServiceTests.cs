using CesiZen.Application.Services;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Test.Fakers;
using Moq;
using Serilog;

namespace CesiZen.Test.QueryServices;

public class UserQueryServiceTests
{
    private readonly Mock<ILogger> mockLogger;
    private readonly Mock<IUserQuery> mockUserQuery;
    private readonly UserQueryService service;

    public UserQueryServiceTests()
    {
        mockLogger = new Mock<ILogger>();
        mockUserQuery = new Mock<IUserQuery>();
        service = new UserQueryService(mockUserQuery.Object, mockLogger.Object);
    }

    [Fact]
    public async Task GetByIdAsyncTest_Success_ReturnsUserDto()
    {
        // Arrange
        int userId = 1;
        var user = UserFaker.FakeUserDtoGenerator().Generate();

        mockUserQuery.Setup(q => q.GetByIdAsync(userId))
            .ReturnsAsync(Result<UserDto>.Success(user));

        // Act
        var result = await service.GetByIdAsync(userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(user.Id, result.Value.Id);
        Assert.Equal(user.Firstname, result.Value.Firstname);
        Assert.Equal(user.Lastname, result.Value.Lastname);
    }

    [Fact]
    public async Task GetByIdAsyncTest_Failure_ReturnsFailureResult()
    {
        // Arrange
        int userId = 1;
        mockUserQuery.Setup(q => q.GetByIdAsync(userId))
            .ReturnsAsync(Result<UserDto>.Failure(Error.NullValue("Error Message")));

        // Act
        var result = await service.GetByIdAsync(userId);

        // Assert
        Assert.True(result.IsFailure);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task SearchUsersTest_Success_ReturnsPagedResultOfUserDto()
    {
        // Arrange
        var parameters = new PageParametersDto { PageNumber = 1, PageSize = 10 };
        var searchTerm = "John";
        var users = UserFaker.FakeUserMinimumDtoGenerator().Generate(10);
        users[0].Firstname = searchTerm;
        var pagedResult = new PagedResultDto<UserMinimumDto>()
        {
            Data = users,
            TotalCount = 1,
            PageNumber = 1,
            PageSize = 10
        };

        mockUserQuery.Setup(q => q.SearchUsers(parameters, searchTerm))
            .ReturnsAsync(Result<PagedResultDto<UserMinimumDto>>.Success(pagedResult));

        // Act
        var result = await service.SearchUsers(parameters, searchTerm);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Data.Any());
        Assert.Equal(1, result.Value.TotalCount);
        Assert.Equal(10, result.Value.PageSize);
        Assert.Equal(1, result.Value.TotalCount);
        Assert.Equal(searchTerm, result.Value.Data[0].Firstname);
    }

    [Fact]
    public async Task SearchUsersTest_Failure_ReturnsFailureResult()
    {
        // Arrange
        var parameters = new PageParametersDto { PageNumber = 1, PageSize = 10 };
        var searchTerm = "John";

        mockUserQuery.Setup(q => q.SearchUsers(parameters, searchTerm))
            .ReturnsAsync(Result<PagedResultDto<UserMinimumDto>>.Failure(Error.NullValue("Error Message")));

        // Act
        var result = await service.SearchUsers(parameters, searchTerm);

        // Assert
        Assert.True(result.IsFailure);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsyncTest_Success_ReturnsPagedResultOfUserDto()
    {
        // Arrange
        var parameters = new PageParametersDto { PageNumber = 1, PageSize = 10 };
        var searchTerm = "John";
        var users = UserFaker.FakeUserMinimumDtoGenerator().Generate(10);
        users[0].Firstname = searchTerm;
        var pagedResult = new PagedResultDto<UserMinimumDto>()
        {
            Data = users,
            TotalCount = 1,
            PageNumber = 1,
            PageSize = 10
        };

        mockUserQuery.Setup(q => q.GetAllAsync(parameters.PageNumber, parameters.PageSize))
            .ReturnsAsync(Result<PagedResultDto<UserMinimumDto>>.Success(pagedResult));

        // Act
        var result = await service.GetAllAsync(parameters.PageNumber, parameters.PageSize);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Data.Any());
        Assert.Equal(1, result.Value.TotalCount);
        Assert.Equal(10, result.Value.PageSize);
        Assert.Equal(1, result.Value.TotalCount);
        Assert.Equal(searchTerm, result.Value.Data[0].Firstname);
    }

    [Fact]
    public async Task GetAllAsyncTest_Failure_ReturnsFailureResult()
    {
        // Arrange
        var parameters = new PageParametersDto { PageNumber = 1, PageSize = 10 };

        mockUserQuery.Setup(q => q.GetAllAsync(parameters.PageNumber, parameters.PageSize))
            .ReturnsAsync(Result<PagedResultDto<UserMinimumDto>>.Failure(Error.NullValue("Error Message")));

        // Act
        var result = await service.GetAllAsync(parameters.PageNumber, parameters.PageSize);

        // Assert
        Assert.True(result.IsFailure);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }
}
