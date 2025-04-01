using CesiZen.Application.Services;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using CesiZen.Infrastructure.DatabaseContext;
using CesiZen.Test.Fakers;
using CesiZen.Test.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using Serilog;

namespace CesiZen.Test.CommandServices;

public class ArticleCommandServiceTests
{
    private readonly Mock<ILogger> mockLogger;
    private readonly Mock<IArticleCommand> mockCommand;
    private readonly ArticleCommandService service;
    private readonly Mock<CesizenDbContext> mockContext;
    private Mock<DbSet<Article>> mockSet;

    public ArticleCommandServiceTests()
    {
        mockLogger = new Mock<ILogger>();
        mockCommand = new Mock<IArticleCommand>();
        mockContext = new Mock<CesizenDbContext>(Tools.SetContext());
        service = new ArticleCommandService(mockLogger.Object, mockCommand.Object);
        mockSet = null!;
    }

    [Fact]
    public async Task InsertTest_Success_WhenDataSaved()
    {
        // Arrange
        var dtos = ArticleFaker.FakeNewArticleDtoGenerator().Generate(10);
        var articles = dtos.Map();
        mockSet = CommonFaker.CreateMockDbSet(articles);
        mockContext.Setup(c => c.Articles).Returns(mockSet.Object);
        mockCommand.Setup(c => c.Insert(It.IsAny<Article>()))
            .ReturnsAsync(Result<ArticleMinimumDto>
            .Success(It.IsAny<ArticleMinimumDto>(), ArticleInfos.ClientInsertionSucceeded));

        // Act
        var result = await service.Insert(dtos[0]);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.Insert(It.Is<Article>(a => a.Title == dtos[0].Title)), Times.Once);
        Assert.True(mockContext.Object.Articles.Any(c => c.Title == dtos[0].Title));
    }

    [Fact]
    public async Task InsertTest_Failure_WhenOperationFails()
    {
        // Arrange
        var dto = ArticleFaker.FakeNewArticleDtoGenerator().Generate();
        mockCommand.Setup(c => c.Insert(It.IsAny<Article>()))
            .ReturnsAsync(Result<ArticleMinimumDto>
            .Failure(Error.NullValue("Error message")));

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
        MockSetter(articles, CommandSelector.C1);
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
        article.Id = 2;
        mockContext.Setup(c => c.Articles.Add(article));
        mockCommand.Setup(c => c.Update(It.IsAny<Article>())).ReturnsAsync(
            Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.Update(dto);

        // Assert
        Assert.True(result.IsFailure);
        mockCommand.Verify(c => c.Update(
            It.Is<Article>(a => a.Id == dto.Id && a.Title == dto.Title)), Times.Once);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTitleAsyncTest_Success_WhenTitleUpdated()
    {
        // Arrange
        string newTitle = "New Title";
        var articles = ArticleFaker.FakeArticleGenerator().Generate(10);
        MockSetter(articles, CommandSelector.C2);

        // Act
        var result = await service.UpdateTitleAsync(articles[0].Id, newTitle);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.UpdateTitleAsync(It.Is<Article>(a => a.Id == articles[0].Id && a.Title == newTitle)), Times.Once);
        Assert.True(mockContext.Object.Articles.Any(a => a.Title == newTitle));
    }

    [Fact]
    public async Task UpdateTitleAsyncTest_Failure_WhenOperationFails()
    {
        // Arrange
        int id = 0;
        string newTitle = "New Title";
        var article = ArticleFaker.FakeArticleGenerator().Generate();
        mockContext.Setup(c => c.Articles.Add(article));
        mockCommand.Setup(c => c.UpdateTitleAsync(It.IsAny<Article>())).ReturnsAsync(
            Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.UpdateTitleAsync(id, newTitle);

        // Assert
        Assert.True(result.IsFailure);
        mockCommand.Verify(c => c.UpdateTitleAsync(It.IsAny<Article>()), Times.Once);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateDescriptionAsyncTest_Success_WhenTitleUpdated()
    {
        // Arrange
        string newDescription = "New Description";
        var articles = ArticleFaker.FakeArticleGenerator().Generate(10);
        MockSetter(articles, CommandSelector.C3);

        // Act
        var result = await service.UpdateDescriptionAsync(articles[0].Id, newDescription);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.UpdateDescriptionAsync(
            It.Is<Article>(a => a.Id == articles[0].Id && a.Description == newDescription)), Times.Once);
        Assert.True(mockContext.Object.Articles.Any(c => c.Description == newDescription));
    }

    [Fact]
    public async Task UpdateDescriptionAsyncTest_Failure_WhenOperationFails()
    {
        // Arrange
        int id = 1;
        string newDescription = "New Description";
        var article = ArticleFaker.FakeArticleGenerator().Generate();
        article.Id = 2;
        mockContext.Setup(c => c.Articles.Add(article));
        mockCommand.Setup(c => c.UpdateDescriptionAsync(It.IsAny<Article>())).ReturnsAsync(
            Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.UpdateDescriptionAsync(id, newDescription);

        // Assert
        Assert.True(result.IsFailure);
        mockCommand.Verify(c => c.UpdateDescriptionAsync(It.IsAny<Article>()), Times.Once);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateContentAsyncTest_Success_WhenTitleUpdated()
    {
        // Arrange
        string newContent = "New Content";
        var articles = ArticleFaker.FakeArticleGenerator().Generate(10);
        MockSetter(articles, CommandSelector.C4);

        // Act
        var result = await service.UpdateContentAsync(articles[0].Id, newContent);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.UpdateContentAsync(
            It.Is<Article>(a => a.Id == articles[0].Id && a.Content == newContent)), Times.Once);
        Assert.True(mockContext.Object.Articles.Any(c => c.Content == newContent));
    }

    [Fact]
    public async Task UpdateContentAsyncTest_Failure_WhenOperationFails()
    {
        // Arrange
        int id = 1;
        string newContent = "New Content";
        var articles = ArticleFaker.FakeArticleGenerator().Generate(10);
        articles[0].Id = 2;
        mockCommand.Setup(c => c.UpdateContentAsync(It.IsAny<Article>())).ReturnsAsync(
            Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.UpdateContentAsync(id, newContent);

        // Assert
        Assert.True(result.IsFailure);
        mockCommand.Verify(c => c.UpdateContentAsync(It.IsAny<Article>()), Times.Once);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTest_Success_WhenDeletion()
    {
        // Arrange
        var articles = ArticleFaker.FakeArticleGenerator().Generate(10);
        MockSetter(articles, CommandSelector.C5);

        // Act
        var result = await service.Delete(articles[0].Id);

        // Assert
        Assert.True(result.IsSuccess);
        mockCommand.Verify(c => c.Delete(It.IsAny<int>()), Times.Once);
        //Assert.False(mockContext.Object.Articles.Any(c => c.Id == articles[0].Id));
    }

    [Fact]
    public async Task DeleteTest_Failure_WhenNotFound()
    {
        // Arrange
        int id = 1;
        var article = ArticleFaker.FakeArticleGenerator().Generate();
        article.Id = 10;
        mockContext.Setup(c => c.Articles.Add(article));
        mockCommand.Setup(c => c.Delete(id)).ReturnsAsync(
            Result.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.Delete(id);

        // Assert
        Assert.True(result.IsFailure);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    private void MockSetter(List<Article> articles, CommandSelector commandSelector)
    {
        mockSet = CommonFaker.CreateMockDbSet(articles);
        mockContext.Setup(c => c.Articles).Returns(mockSet.Object);
        MockCommandSelector(articles, commandSelector);
    }

    private void MockCommandSelector(List<Article> articles, CommandSelector commandSelector)
    {
        switch (commandSelector)
        {
            case CommandSelector.C1:
                mockCommand.Setup(c => c.Update(It.IsAny<Article>())).Callback<Article>(
                    updatedArticle =>
                    {
                        var article = articles.FirstOrDefault(a => a.Id == updatedArticle.Id);
                        if (article != null)
                        {
                            article.Title = updatedArticle.Title;
                        }
                    }
                ).ReturnsAsync(Result.Success());
                break;
            case CommandSelector.C2:
                mockCommand.Setup(c => c.UpdateTitleAsync(It.IsAny<Article>())).Callback<Article>(
                    updatedArticle =>
                    {
                        var article = articles.FirstOrDefault(a => a.Id == updatedArticle.Id);
                        if (article != null)
                        {
                            article.Title = updatedArticle.Title;
                        }
                    }
                ).ReturnsAsync(Result.Success());
                break;
            case CommandSelector.C3:
                mockCommand.Setup(c => c.UpdateDescriptionAsync(It.IsAny<Article>())).Callback<Article>(
                    updatedArticle =>
                    {
                        var article = articles.FirstOrDefault(a => a.Id == updatedArticle.Id);
                        if (article != null)
                        {
                            article.Description = updatedArticle.Description;
                        }
                    }
                ).ReturnsAsync(Result.Success());
                break;
            case CommandSelector.C4:
                mockCommand.Setup(c => c.UpdateContentAsync(It.IsAny<Article>())).Callback<Article>(
                    updatedArticle =>
                    {
                        var article = articles.FirstOrDefault(a => a.Id == updatedArticle.Id);
                        if (article != null)
                        {
                            article.Content = updatedArticle.Content;
                        }
                    }
                ).ReturnsAsync(Result.Success());
                break;
            case CommandSelector.C5:
                mockCommand.Setup(c => c.Delete(It.IsAny<int>())).Callback<int>(
                    id =>
                    {
                        var article = articles.FirstOrDefault(a => a.Id == id);
                        if (article != null)
                        {
                            articles.Remove(article);
                        }
                    }
                ).ReturnsAsync(Result.Success());
                break;
        }
    }
}
