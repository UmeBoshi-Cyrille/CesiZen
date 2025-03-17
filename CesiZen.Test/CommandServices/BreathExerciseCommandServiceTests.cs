using CesiZen.Application.Services;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using CesiZen.Infrastructure.DatabaseContext;
using CesiZen.Test.Fakers;
using Moq;
using Serilog;

namespace CesiZen.Test.CommandServices;

public class BreathExerciseCommandServiceTests
{
    private readonly Mock<ILogger> loggerMock;
    private readonly Mock<IBreathExerciseCommand> mockCommand;
    private readonly BreathExerciseCommandService service;
    private Mock<MongoDbContext> mockContext;

    public BreathExerciseCommandServiceTests()
    {
        loggerMock = new Mock<ILogger>();
        mockCommand = new Mock<IBreathExerciseCommand>();
        mockContext = new Mock<MongoDbContext>();
        service = new BreathExerciseCommandService(loggerMock.Object, mockCommand.Object);
    }

    [Fact]
    public async Task Insert_Success_ReturnsSuccessResult()
    {
        // Arrange
        var dto = BreathExerciseFaker.FakeBreathExerciseDtoGenerator().Generate();
        var entity = dto.Map();
        mockCommand.Setup(c => c.Insert(It.IsAny<BreathExercise>()))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await service.Insert(dto);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.Insert(It.Is<BreathExercise>(e => e == entity)), Times.Once);
        mockContext.Verify(c => c.BreathExercises.Any());
        mockContext.Verify(c => c.BreathExercises.FirstOrDefault().Title == entity.Title);
    }

    [Fact]
    public async Task Insert_Failure_ReturnsFailureResult()
    {
        // Arrange
        var dto = BreathExerciseFaker.FakeBreathExerciseDtoGenerator().Generate();
        mockCommand.Setup(c => c.Insert(It.IsAny<BreathExercise>()))
            .ReturnsAsync(Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.Insert(dto);

        // Assert
        Assert.True(result.IsFailure);
        loggerMock.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Update_Success_ReturnsSuccessResult()
    {
        // Arrange
        var dto = BreathExerciseFaker.FakeBreathExerciseDtoGenerator().Generate();
        var entity = dto.Map();
        mockContext.Setup(c => c.BreathExercises.Add(entity));
        mockCommand.Setup(c => c.Update(It.IsAny<BreathExercise>()))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await service.Update(dto);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.Update(It.Is<BreathExercise>(e => e == entity)), Times.Once);
        mockContext.Verify(c => c.BreathExercises.FirstOrDefault().Title == dto.Title);
    }

    [Fact]
    public async Task Update_Failure_ReturnsFailureResult()
    {
        // Arrange
        var dto = BreathExerciseFaker.FakeBreathExerciseDtoGenerator().Generate();
        mockCommand.Setup(c => c.Update(It.IsAny<BreathExercise>()))
            .ReturnsAsync(Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.Update(dto);

        // Assert
        Assert.True(result.IsFailure);
        loggerMock.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Delete_Success_ReturnsSuccessResult()
    {
        // Arrange
        string id = "1";
        var entity = BreathExerciseFaker.FakeBreathExerciseGenerator().Generate();
        mockContext.Setup(c => c.BreathExercises.Add(entity));
        mockCommand.Setup(c => c.Delete(id))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await service.Delete(id);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.Delete(id), Times.Once);
        mockContext.Verify(c => !c.Articles.Any());
    }

    [Fact]
    public async Task Delete_Failure_ReturnsFailureResult()
    {
        // Arrange
        string id = "1";
        var entity = BreathExerciseFaker.FakeBreathExerciseGenerator().Generate();
        entity.Id = "2";
        mockContext.Setup(c => c.BreathExercises.Add(entity));
        mockCommand.Setup(c => c.Delete(id))
            .ReturnsAsync(Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.Delete(id);

        // Assert
        Assert.True(result.IsFailure);
        loggerMock.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }
}
