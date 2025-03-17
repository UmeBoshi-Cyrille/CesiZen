using CesiZen.Application.Services;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interface;
using CesiZen.Domain.Mapper;
using CesiZen.Infrastructure.DatabaseContext;
using CesiZen.Test.Fakers;
using Moq;
using Serilog;

namespace CesiZen.Test.CommandServices;

public class UserCommandServiceTests
{
    private readonly Mock<ILogger> mockLogger;
    private readonly Mock<IUserCommand> mockUserCommand;
    private Mock<MongoDbContext> mockContext;
    private readonly UserCommandService service;

    public UserCommandServiceTests()
    {
        mockLogger = new Mock<ILogger>();
        mockUserCommand = new Mock<IUserCommand>();
        mockContext = new Mock<MongoDbContext>();
        service = new UserCommandService(mockUserCommand.Object, mockLogger.Object);
    }

    [Fact]
    public async Task UpdateTest_Success_WhenDataUpdated()
    {
        // Arrange
        var dto = UserFaker.FakeDtoGenerator().Generate();
        var user = dto.Map();
        mockContext.Setup(c => c.Users.Add(user));
        mockUserCommand.Setup(c => c.Update(It.IsAny<User>()))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await service.Update(dto);

        // Assert
        Assert.True(result.IsSuccess);
        mockUserCommand.Verify(c => c.Update(It.IsAny<User>()), Times.Once);
        mockContext.Verify(c => c.Users.FirstOrDefault().Firstname == dto.Firstname &&
                                c.Users.FirstOrDefault().Lastname == dto.Lastname);
    }

    [Fact]
    public async Task UpdateTest_Failure_WhenOperationFails()
    {
        // Arrange
        var dto = UserFaker.FakeDtoGenerator().Generate();
        mockUserCommand.Setup(c => c.Update(It.IsAny<User>()))
            .ReturnsAsync(Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.Update(dto);

        // Assert
        Assert.True(result.IsFailure);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTest_Success_WhenDeletion()
    {
        // Arrange
        string userId = "1";
        var user = UserFaker.FakeUserGenerator().Generate();
        mockContext.Setup(c => c.Users.Add(user));
        mockUserCommand.Setup(c => c.Delete(userId)).ReturnsAsync(Result.Success());

        // Act
        var result = await service.Delete(userId);

        // Assert
        Assert.True(result.IsSuccess);
        mockUserCommand.Verify(c => c.Delete(userId), Times.Once);
        mockContext.Verify(c => !c.Articles.Any());
    }

    [Fact]
    public async Task Delete_Failure_WhenOperationFails()
    {
        // Arrange
        string userId = "1";
        var user = UserFaker.FakeUserGenerator().Generate();
        user.Id = "10";
        mockContext.Setup(c => c.Users.Add(user));
        mockUserCommand.Setup(c => c.Delete(userId))
            .ReturnsAsync(Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.Delete(userId);

        // Assert
        Assert.True(result.IsFailure);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }
}
