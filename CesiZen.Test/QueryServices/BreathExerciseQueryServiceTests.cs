using CesiZen.Application.Services;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Test.Fakers;
using Moq;
using Serilog;

namespace CesiZen.Test.QueryServices;

public class BreathExerciseQueryServiceTests
{
    private readonly Mock<ILogger> mockLogger;
    private readonly Mock<IBreathExerciseQuery> mockQuery;
    private readonly BreathExerciseQueryService service;

    public BreathExerciseQueryServiceTests()
    {
        mockLogger = new Mock<ILogger>();
        mockQuery = new Mock<IBreathExerciseQuery>();
        service = new BreathExerciseQueryService(mockLogger.Object, mockQuery.Object);
    }

    [Fact]
    public async Task GetAllByIdAsync_Success_ReturnsListOfBreathExerciseDtos()
    {
        // Arrange
        int userId = 1;
        var breathExercises = BreathExerciseFaker.FakeBreathExerciseMinimumDtoGenerator().Generate(5);
        mockQuery.Setup(q => q.GetAllByIdAsync(userId))
            .ReturnsAsync(Result<List<BreathExerciseMinimumDto>>.Success(breathExercises));

        // Act
        var result = await service.GetAllByIdAsync(userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.IsType<List<BreathExerciseMinimumDto>>(result.Value);
        Assert.Equal(breathExercises.Count, result.Value.Count);
    }

    [Fact]
    public async Task GetAllByIdAsync_Failure_ReturnsFailureResult()
    {
        // Arrange
        int userId = 1;
        mockQuery.Setup(q => q.GetAllByIdAsync(userId))
            .ReturnsAsync(Result<List<BreathExerciseMinimumDto>>.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.GetAllByIdAsync(userId);

        // Assert
        Assert.True(result.IsFailure);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Success_ReturnsBreathExerciseDto()
    {
        // Arrange
        int id = 1;
        var breathExercise = BreathExerciseFaker.FakeBreathExerciseDtoGenerator().Generate();
        mockQuery.Setup(q => q.GetByIdAsync(id))
            .ReturnsAsync(Result<BreathExerciseDto>.Success(breathExercise));

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.IsType<BreathExerciseDto>(result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_Failure_ReturnsFailureResult()
    {
        // Arrange
        int id = 1;
        mockQuery.Setup(q => q.GetByIdAsync(id))
            .ReturnsAsync(Result<BreathExerciseDto>.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.True(result.IsFailure);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }
}
