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

public class CategoryCommandServiceTests
{
    private readonly Mock<ILogger> mockLogger;
    private readonly Mock<ICategoryCommand> mockCommand;
    private readonly CategoryCommandService service;
    private Mock<MongoDbContext> mockContext;

    public CategoryCommandServiceTests()
    {
        mockLogger = new Mock<ILogger>();
        mockCommand = new Mock<ICategoryCommand>();
        mockContext = new Mock<MongoDbContext>();
        service = new CategoryCommandService(mockLogger.Object, mockCommand.Object);
    }

    [Fact]
    public async Task InsertTest_Success_WhenAddData()
    {
        // Arrange
        var dto = CategoryFaker.FakeCategoryDtoGenerator().Generate();
        var entity = dto.Map();
        mockCommand.Setup(c => c.Insert(entity))
                    .ReturnsAsync(Result.Success());

        // Act
        var result = await service.Insert(dto);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(l => l.Insert(It.IsAny<Category>()), Times.Never);
        mockContext.Verify(c => c.Categories.Any());
        mockContext.Verify(c => c.Categories.FirstOrDefault().Name == entity.Name);

    }

    [Fact]
    public async Task InsertTest_Failure_WhenOperationFails()
    {
        // Arrange
        var dto = CategoryFaker.FakeCategoryDtoGenerator().Generate();
        var entity = dto.Map();
        var errorMessage = "Add failed";

        mockCommand.Setup(c => c.Insert(entity))
                    .ReturnsAsync(Result.Failure(Error.NullValue(errorMessage)));

        // Act
        var result = await service.Insert(dto);

        // Assert
        Assert.True(result.IsFailure);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTest_Success_WhenCommandSucceed()
    {
        // Arrange
        var dto = CategoryFaker.FakeCategoryDtoGenerator().Generate();
        var category = dto.Map();

        mockContext.Setup(c => c.Categories.Add(category));
        mockCommand.Setup(c => c.Update(category))
                    .ReturnsAsync(Result.Success());

        // Act
        var result = await service.Update(dto);

        // Assert
        Assert.True(result.IsSuccess);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Never);
        mockContext.Verify(c => c.Categories.FirstOrDefault().Name == dto.Name);
    }

    [Fact]
    public async Task UpdateTest_Failure_WhenOperationFails()
    {
        // Arrange
        var dto = CategoryFaker.FakeCategoryDtoGenerator().Generate();
        var category = CategoryFaker.FakeCategoryGenerator().Generate();
        var errorMessage = "Update failed";

        mockContext.Setup(c => c.Categories.Add(category));
        mockCommand.Setup(c => c.Update(category))
                    .ReturnsAsync(Result.Failure(Error.NullValue(errorMessage)));

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
        string id = "1";
        var category = CategoryFaker.FakeCategoryGenerator(id).Generate();

        mockContext.Setup(c => c.Categories.Add(category));
        mockCommand.Setup(c => c.Delete(id))
                    .ReturnsAsync(Result.Success());

        // Act
        var result = await service.Delete(id);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(l => l.Delete(id), Times.Never);
        mockContext.Verify(c => !c.Categories.Any());
    }

    [Fact]
    public async Task DeleteTest_Failure_WhenOperationFails()
    {
        // Arrange
        string id = "1";
        var category = CategoryFaker.FakeCategoryGenerator(id).Generate();
        category.Id = "10";
        var errorMessage = "Delete failed";

        mockContext.Setup(c => c.Categories.Add(category));
        mockCommand.Setup(c => c.Delete(id))
                    .ReturnsAsync(Result.Failure(Error.NullValue(errorMessage)));

        // Act
        var result = await service.Delete(id);

        // Assert
        Assert.True(result.IsFailure);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }
}
