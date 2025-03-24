using CesiZen.Application.Services;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using CesiZen.Infrastructure.DatabaseContext;
using CesiZen.Test.Fakers;
using CesiZen.Test.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using Serilog;

namespace CesiZen.Test.CommandServices;

public class BreathExerciseCommandServiceTests
{
    private readonly Mock<ILogger> loggerMock;
    private readonly Mock<IBreathExerciseCommand> mockCommand;
    private readonly BreathExerciseCommandService service;
    private Mock<CesizenDbContext> mockContext;
    private Mock<DbSet<BreathExercise>> mockSet;

    public BreathExerciseCommandServiceTests()
    {
        loggerMock = new Mock<ILogger>();
        mockCommand = new Mock<IBreathExerciseCommand>();
        mockContext = new Mock<CesizenDbContext>(Tools.SetContext());
        service = new BreathExerciseCommandService(loggerMock.Object, mockCommand.Object);
        mockSet = null!;
    }

    [Fact]
    public async Task Insert_Success_ReturnsSuccessResult()
    {
        // Arrange
        var dtos = BreathExerciseFaker.FakeNewBreathExerciseDtoGenerator().Generate(10);
        var entities = dtos.Map();
        mockSet = CommonFaker.CreateMockDbSet(entities);
        mockContext.Setup(c => c.BreathExercises).Returns(mockSet.Object);
        mockCommand.Setup(c => c.Insert(It.IsAny<BreathExercise>()))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await service.Insert(dtos[0]);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.Insert(
            It.Is<BreathExercise>(e => e.Title == entities[0].Title)), Times.Once);
        Assert.True(mockContext.Object.BreathExercises.Any(a => a.Title == entities[0].Title));
    }

    [Fact]
    public async Task Insert_Failure_ReturnsFailureResult()
    {
        // Arrange
        var dto = BreathExerciseFaker.FakeNewBreathExerciseDtoGenerator().Generate();
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
        var dtos = BreathExerciseFaker.FakeBreathExerciseDtoGenerator().Generate(10);
        var entities = dtos.Map();
        MockSetter(entities, CommandSelector.C1);
        dtos[0].Title = "New";

        // Act
        var result = await service.Update(dtos[0]);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.Update(
            It.Is<BreathExercise>(e => e.Id == dtos[0].Id && e.Title == dtos[0].Title)), Times.Once);
        Assert.True(mockContext.Object.BreathExercises
            .Any(e => e.Title == dtos[0].Title && e.Id == dtos[0].Id));
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
        var entities = BreathExerciseFaker.FakeBreathExerciseGenerator().Generate(10);
        MockSetter(entities, CommandSelector.C2);

        // Act
        var result = await service.Delete(entities[0].Id);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Delete_Failure_ReturnsFailureResult()
    {
        // Arrange
        int id = 1;
        var entity = BreathExerciseFaker.FakeBreathExerciseGenerator().Generate();
        entity.Id = 2;
        mockContext.Setup(c => c.BreathExercises.Add(entity));
        mockCommand.Setup(c => c.Delete(id))
            .ReturnsAsync(Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.Delete(id);

        // Assert
        Assert.True(result.IsFailure);
        loggerMock.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    private void MockSetter(List<BreathExercise> entities, CommandSelector commandSelector)
    {
        mockSet = CommonFaker.CreateMockDbSet(entities);
        mockContext.Setup(c => c.BreathExercises).Returns(mockSet.Object);
        MockCommandSelector(entities, commandSelector);
    }

    private void MockCommandSelector(List<BreathExercise> entities, CommandSelector commandSelector)
    {
        switch (commandSelector)
        {
            case CommandSelector.C1:
                mockCommand.Setup(c => c.Update(It.IsAny<BreathExercise>())).Callback<BreathExercise>(
                    updated =>
                    {
                        var entity = entities.FirstOrDefault(a => a.Id == updated.Id);
                        if (entity != null)
                        {
                            entity.Title = updated.Title;
                        }
                    }
                ).ReturnsAsync(Result.Success());
                break;
            case CommandSelector.C2:
                mockCommand.Setup(c => c.Delete(It.IsAny<int>())).Callback<int>(
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
        }
    }
}
