using CesiZen.Application.Services;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using CesiZen.Infrastructure.DatabaseContext;
using CesiZen.Test.Fakers;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Moq;
using Serilog;

namespace CesiZen.Test.CommandServices;

public class ArticleCommandServiceTests
{
    private readonly Mock<ILogger> mockLogger;
    private readonly Mock<IArticleCommand> mockCommand;
    private readonly ArticleCommandService service;
    private Mock<MongoDbContext> mockContext;
    private Mock<DbSet<Article>> mockSet;

    public ArticleCommandServiceTests()
    {
        var options = new DbContextOptionsBuilder<MongoDbContext>()
            .UseMongoDB("mongodb://localhost:27017", "TestDB")
            .Options;

        mockLogger = new Mock<ILogger>();
        mockCommand = new Mock<IArticleCommand>();
        mockContext = new Mock<MongoDbContext>(options);
        service = new ArticleCommandService(mockLogger.Object, mockCommand.Object);
    }

    [Fact]
    public async Task AddAsyncTest_Success_WhenDataSaved()
    {
        // Arrange
        var dtos = ArticleFaker.FakeArticleDtoGenerator().Generate(10);
        var articles = dtos.Map();
        mockSet = CommonFaker.CreateMockDbSet(articles);
        mockContext.Setup(c => c.Articles).Returns(mockSet.Object);
        mockCommand.Setup(c => c.Insert(It.IsAny<Article>())).ReturnsAsync(Result.Success());

        // Act
        var result = await service.Insert(dtos[0]);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.Insert(It.Is<Article>(a => a.Title == dtos[0].Title)), Times.Once);
        Assert.True(mockContext.Object.Articles.Any(c => c.Title == dtos[0].Title));
    }

    [Fact]
    public async Task AddAsyncTest_Failure_WhenOperationFails()
    {
        // Arrange
        var dto = ArticleFaker.FakeArticleDtoGenerator().Generate();
        mockCommand.Setup(c => c.Insert(It.IsAny<Article>()))
            .ReturnsAsync(Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.Insert(dto);

        // Assert
        Assert.True(result.IsFailure);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsyncTest_Success_WhenDataUpdated()
    {
        // Arrange
        var dtos = ArticleFaker.FakeArticleDtoGenerator().Generate(10);
        var articles = dtos.Map();
        mockSet = CommonFaker.CreateMockDbSet(articles);
        mockContext.Setup(c => c.Articles).Returns(mockSet.Object);
        mockCommand.Setup(c => c.Update(It.IsAny<Article>())).ReturnsAsync(Result.Success());
        dtos[0].Title = "new";

        // Act
        var result = await service.Update(dtos[0]);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.Update(
            It.Is<Article>(a => a.Id == dtos[0].Id && a.Title == dtos[0].Title)), Times.Once);
        Assert.True(mockContext.Object.Articles.Any(a => a.Title == dtos[0].Title));
    }

    [Fact]
    public async Task UpdateAsyncTest_Failure_WhenOperationFails()
    {
        // Arrange
        var dto = ArticleFaker.FakeArticleDtoGenerator().Generate();
        var article = dto.Map();
        article.Id = "2";
        mockContext.Setup(c => c.Articles.Add(article));
        mockCommand.Setup(c => c.Update(It.IsAny<Article>())).ReturnsAsync(
            Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.Update(dto);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.Update(
            It.Is<Article>(a => a.Id == dto.Id && a.Title == dto.Title)), Times.Once);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTitleAsyncTest_Success_WhenTitleUpdated()
    {
        // Arrange
        string id = "1";
        string newTitle = "New Title";
        var article = ArticleFaker.FakeArticleGenerator().Generate();
        mockContext.Setup(c => c.Articles.Add(article));
        mockCommand.Setup(c => c.UpdateTitleAsync(article)).ReturnsAsync(Result.Success());

        // Act
        var result = await service.UpdateTitleAsync(id, newTitle);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.UpdateTitleAsync(article), Times.Once);
        mockContext.Verify(c => c.Articles.FirstOrDefault().Title == newTitle);
    }

    [Fact]
    public async Task UpdateTitleAsyncTest_Failure_WhenOperationFails()
    {
        // Arrange
        string id = "1";
        string newTitle = "New Title";
        var article = ArticleFaker.FakeArticleGenerator().Generate();
        article.Id = "2";
        mockContext.Setup(c => c.Articles.Add(article));
        mockCommand.Setup(c => c.UpdateTitleAsync(article)).ReturnsAsync(
            Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.UpdateTitleAsync(id, newTitle);

        // Assert
        Assert.True(result.IsFailure);
        mockCommand.Verify(c => c.UpdateTitleAsync(article), Times.Once);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateDescriptionAsyncTest_Success_WhenTitleUpdated()
    {
        // Arrange
        string id = "1";
        string newDescription = "New Description";
        var article = ArticleFaker.FakeArticleGenerator().Generate();
        mockContext.Setup(c => c.Articles.Add(article));
        mockCommand.Setup(c => c.UpdateTitleAsync(article)).ReturnsAsync(Result.Success());

        // Act
        var result = await service.UpdateTitleAsync(id, newDescription);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.UpdateTitleAsync(article), Times.Once);
        mockContext.Verify(c => c.Articles.FirstOrDefault().Title == newDescription);
    }

    [Fact]
    public async Task UpdateDescriptionAsyncTest_Failure_WhenOperationFails()
    {
        // Arrange
        string id = "1";
        string newDescription = "New Description";
        var article = ArticleFaker.FakeArticleGenerator().Generate();
        article.Id = "2";
        mockContext.Setup(c => c.Articles.Add(article));
        mockCommand.Setup(c => c.UpdateTitleAsync(article)).ReturnsAsync(
            Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.UpdateTitleAsync(id, newDescription);

        // Assert
        Assert.True(result.IsFailure);
        mockCommand.Verify(c => c.UpdateTitleAsync(article), Times.Once);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateContentAsyncTest_Success_WhenTitleUpdated()
    {
        // Arrange
        string id = "1";
        string newContent = "New Content";
        var article = ArticleFaker.FakeArticleGenerator().Generate();
        mockContext.Setup(c => c.Articles.Add(article));
        mockCommand.Setup(c => c.UpdateTitleAsync(article)).ReturnsAsync(Result.Success());

        // Act
        var result = await service.UpdateTitleAsync(id, newContent);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.UpdateTitleAsync(article), Times.Once);
        mockContext.Verify(c => c.Articles.FirstOrDefault().Title == newContent);
    }

    [Fact]
    public async Task UpdateContentAsyncTest_Failure_WhenOperationFails()
    {
        // Arrange
        string id = "1";
        string newContent = "New Content";
        var article = ArticleFaker.FakeArticleGenerator().Generate();
        article.Id = "2";
        mockContext.Setup(c => c.Articles.Add(article));
        mockCommand.Setup(c => c.UpdateTitleAsync(article)).ReturnsAsync(
            Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.UpdateTitleAsync(id, newContent);

        // Assert
        Assert.True(result.IsFailure);
        mockCommand.Verify(c => c.UpdateTitleAsync(article), Times.Once);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTest_Success_WhenDeletion()
    {
        // Arrange
        string id = "1";
        var article = ArticleFaker.FakeArticleGenerator().Generate();
        article.Id = id;
        mockContext.Setup(c => c.Articles.Add(article));
        mockCommand.Setup(c => c.Delete(id)).ReturnsAsync(Result.Success());

        // Act
        var result = await service.Delete(id);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.Delete(id), Times.Once);
        mockContext.Verify(c => !c.Articles.Any());
    }

    [Fact]
    public async Task DeleteTest_Failure_WhenNotFound()
    {
        // Arrange
        string id = "1";
        var article = ArticleFaker.FakeArticleGenerator().Generate();
        article.Id = "10";
        mockContext.Setup(c => c.Articles.Add(article));
        mockCommand.Setup(c => c.Delete(id)).ReturnsAsync(
            Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.Delete(id);

        // Assert
        Assert.True(result.IsFailure);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }
}
