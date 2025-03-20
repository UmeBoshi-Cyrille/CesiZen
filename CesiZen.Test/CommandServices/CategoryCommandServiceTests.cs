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

public class CategoryCommandServiceTests
{
    private readonly Mock<ILogger> mockLogger;
    private readonly Mock<ICategoryCommand> mockCommand;
    private readonly CategoryCommandService service;
    private Mock<MongoDbContext> mockContext;
    private Mock<DbSet<Category>> mockSet;

    public CategoryCommandServiceTests()
    {
        mockLogger = new Mock<ILogger>();
        mockCommand = new Mock<ICategoryCommand>();
        mockContext = new Mock<MongoDbContext>(Tools.SetContext());
        service = new CategoryCommandService(mockLogger.Object, mockCommand.Object);
        mockSet = null!;
    }

    [Fact]
    public async Task InsertTest_Success_WhenAddData()
    {
        // Arrange
        var dtos = CategoryFaker.FakeCategoryDtoGenerator().Generate(10);
        var entities = dtos.Map();
        mockSet = CommonFaker.CreateMockDbSet(entities);
        mockContext.Setup(c => c.Categories).Returns(mockSet.Object);
        mockCommand.Setup(c => c.Insert(It.IsAny<Category>()))
                    .ReturnsAsync(Result.Success(CategoryInfos.LogInsertionSucceeded(entities[0].Name)));

        // Act
        var result = await service.Insert(dtos[0]);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(l => l.Insert(
            It.Is<Category>(e => e.Name == dtos[0].Name)), Times.Once);
        Assert.True(mockContext.Object.Categories.Any(a => a.Name == dtos[0].Name));
    }

    [Fact]
    public async Task InsertTest_Failure_WhenOperationFails()
    {
        // Arrange
        var dto = CategoryFaker.FakeCategoryDtoGenerator().Generate();
        mockCommand.Setup(c => c.Insert(It.IsAny<Category>()))
                    .ReturnsAsync(Result.Failure(Error.NullValue("Error message")));

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
        var dtos = CategoryFaker.FakeCategoryDtoGenerator().Generate(10);
        var entities = dtos.Map();
        MockSetter(entities, CommandSelector.C1);
        dtos[0].Name = "New";

        // Act
        var result = await service.Update(dtos[0]);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.Update(
            It.Is<Category>(e => e.Id == dtos[0].Id && e.Name == dtos[0].Name)), Times.Once);
        Assert.True(mockContext.Object.Categories
            .Any(e => e.Name == dtos[0].Name && e.Id == dtos[0].Id));
    }

    [Fact]
    public async Task UpdateTest_Failure_WhenOperationFails()
    {
        // Arrange
        var dto = CategoryFaker.FakeCategoryDtoGenerator().Generate();
        mockCommand.Setup(c => c.Update(It.IsAny<Category>()))
                    .ReturnsAsync(Result.Failure(Error.NullValue("Error")));

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
        var entities = CategoryFaker.FakeCategoryGenerator().Generate(10);
        MockSetter(entities, CommandSelector.C2);

        // Act
        var result = await service.Delete(entities[0].Id);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(l => l.Delete(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTest_Failure_WhenOperationFails()
    {
        // Arrange
        string id = "1";
        var category = CategoryFaker.FakeCategoryGenerator(id).Generate();
        category.Id = "10";

        mockContext.Setup(c => c.Categories.Add(category));
        mockCommand.Setup(c => c.Delete(It.IsAny<string>()))
                    .ReturnsAsync(Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.Delete(id);

        // Assert
        Assert.True(result.IsFailure);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    private void MockSetter(List<Category> entities, CommandSelector commandSelector)
    {
        mockSet = CommonFaker.CreateMockDbSet(entities);
        mockContext.Setup(c => c.Categories).Returns(mockSet.Object);
        MockCommandSelector(entities, commandSelector);
    }

    private void MockCommandSelector(List<Category> entities, CommandSelector commandSelector)
    {
        switch (commandSelector)
        {
            case CommandSelector.C1:
                mockCommand.Setup(c => c.Update(It.IsAny<Category>())).Callback<Category>(
                    updatedArticle =>
                    {
                        var entity = entities.FirstOrDefault(a => a.Id == updatedArticle.Id);
                        if (entity != null)
                        {
                            entity.Name = updatedArticle.Name;
                        }
                    }
                ).ReturnsAsync(Result.Success(CategoryInfos.LogUpdateSucceeded(It.IsAny<string>())));
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
                ).ReturnsAsync(Result.Success(CategoryInfos.LogDeleteCompleted(It.IsAny<string>())));
                break;
        }
    }
}
