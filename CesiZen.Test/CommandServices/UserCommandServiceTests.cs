using CesiZen.Application.Services;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interface;
using CesiZen.Domain.Mapper;
using CesiZen.Infrastructure.DatabaseContext;
using CesiZen.Test.Fakers;
using CesiZen.Test.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using Serilog;

namespace CesiZen.Test.CommandServices;

public class UserCommandServiceTests
{
    private readonly Mock<ILogger> mockLogger;
    private readonly Mock<IUserCommand> mockCommand;
    private readonly UserCommandService service;
    private Mock<MongoDbContext> mockContext;
    private Mock<DbSet<User>> mockSet;

    public UserCommandServiceTests()
    {
        var options = new DbContextOptionsBuilder<MongoDbContext>()
            .UseMongoDB("mongodb://localhost:27017", "TestDB")
            .Options;

        mockLogger = new Mock<ILogger>();
        mockCommand = new Mock<IUserCommand>();
        mockContext = new Mock<MongoDbContext>(options);
        service = new UserCommandService(mockCommand.Object, mockLogger.Object);
    }

    [Fact]
    public async Task UpdateTest_Success_WhenDataUpdated()
    {
        // Arrange
        var dtos = UserFaker.FakeDtoGenerator().Generate(10);
        var users = dtos.Map();
        MockSetter(users, CommandSelector.C1);
        dtos[0].Firstname = "New";

        // Act
        var result = await service.Update(dtos[0]);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.Update(It.IsAny<User>()), Times.Once);
        Assert.True(mockContext.Object.Users.Any(c => c.Firstname == dtos[0].Firstname));
    }

    [Fact]
    public async Task UpdateTest_Failure_WhenOperationFails()
    {
        // Arrange
        var dto = UserFaker.FakeDtoGenerator().Generate();
        mockCommand.Setup(c => c.Update(It.IsAny<User>()))
            .ReturnsAsync(Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.Update(dto);

        // Assert
        Assert.True(result.IsFailure);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUsernameTest_Success_WhenTitleUpdated()
    {
        // Arrange
        string newUsername = "New";
        var users = UserFaker.FakeUserGenerator().Generate(10);
        MockSetter(users, CommandSelector.C3);

        // Act
        var result = await service.UpdateUserName(users[0].Id, newUsername);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.UpdateUserName(
            It.Is<string>(a => a == users[0].Id), It.Is<string>(a => a == newUsername)), Times.Once);
        Assert.True(mockContext.Object.Users.Any(c => c.Username == newUsername));
    }

    [Fact]
    public async Task UpdateUsernameTest_Failure_WhenOperationFails()
    {
        // Arrange
        string id = "1";
        string newContent = "New Content";
        var users = UserFaker.FakeUserGenerator().Generate(10);
        users[0].Id = "2";
        mockCommand.Setup(c => c.UpdateUserName(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(
            Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.UpdateUserName(id, newContent);

        // Assert
        Assert.True(result.IsFailure);
        mockCommand.Verify(c => c.UpdateUserName(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTest_Success_WhenDeletion()
    {
        // Arrange
        var users = UserFaker.FakeUserGenerator().Generate(10);
        MockSetter(users, CommandSelector.C2);

        // Act
        var result = await service.Delete(users[0].Id);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.Delete(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Delete_Failure_WhenOperationFails()
    {
        // Arrange
        string userId = "1";
        var user = UserFaker.FakeUserGenerator().Generate();
        user.Id = "10";
        mockContext.Setup(c => c.Users.Add(user));
        mockCommand.Setup(c => c.Delete(userId))
            .ReturnsAsync(Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.Delete(userId);

        // Assert
        Assert.True(result.IsFailure);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    private void MockSetter(List<User> entities, CommandSelector commandSelector)
    {
        mockSet = CommonFaker.CreateMockDbSet(entities);
        mockContext.Setup(c => c.Users).Returns(mockSet.Object);
        MockCommandSelector(entities, commandSelector);
    }

    private void MockCommandSelector(List<User> entities, CommandSelector commandSelector)
    {
        switch (commandSelector)
        {
            case CommandSelector.C1:
                mockCommand.Setup(c => c.Update(It.IsAny<User>())).Callback<User>(
                    update =>
                    {
                        var entity = entities.FirstOrDefault(a => a.Id == update.Id);
                        if (entity != null)
                        {
                            entity.Firstname = update.Firstname;
                        }
                    }
                ).ReturnsAsync(Result.Success());
                break;
            case CommandSelector.C2:
                mockCommand.Setup(c => c.Delete(It.IsAny<string>())).Callback<string>(
                    id =>
                    {
                        var entity = entities.FirstOrDefault(a => a.Id == id);
                        if (entity != null)
                        {
                            entities.Remove(entity);
                        }
                    }
                ).ReturnsAsync(Result.Success());
                break;
            case CommandSelector.C3:
                mockCommand.Setup(c => c.UpdateUserName(It.IsAny<string>(), It.IsAny<string>()))
                    .Callback<string, string>((id, name) =>
                    {
                        var entity = entities.FirstOrDefault(a => a.Id == id);
                        if (entity != null)
                        {
                            entity.Username = name;
                        }
                    }
                ).ReturnsAsync(Result.Success());
                break;
        }
    }
}
