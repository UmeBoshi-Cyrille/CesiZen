using CesiZen.Application.Services;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using CesiZen.Test.Fakers;
using Moq;
using Serilog;

namespace CesiZen.Test.QueryServices;

public class ArticleQueryServiceTests
{
    private readonly Mock<ILogger> loggerMock;
    private readonly Mock<IArticleQuery> queryMock;
    private readonly ArticleQueryService service;

    public ArticleQueryServiceTests()
    {
        loggerMock = new Mock<ILogger>();
        queryMock = new Mock<IArticleQuery>();
        service = new ArticleQueryService(loggerMock.Object, queryMock.Object);
    }

    [Fact]
    public async Task GetByIdAsyncTest_Success_ReturnsArticleDto()
    {
        // Arrange
        int articleId = 1;
        var article = ArticleFaker.FakeArticleGenerator().Generate();
        var expectedDto = article.Map();

        queryMock.Setup(q => q.GetByIdAsync(articleId))
            .ReturnsAsync(Result<Article>.Success(article));

        // Act
        var result = await service.GetByIdAsync(articleId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedDto.Id, result.Value.Id);
        Assert.Equal(expectedDto.Title, result.Value.Title);
    }

    [Fact]
    public async Task GetByIdAsyncTest_Failure_ReturnsFailureResult()
    {
        // Arrange
        int articleId = 1;
        queryMock.Setup(q => q.GetByIdAsync(articleId))
            .ReturnsAsync(Result<Article>.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.GetByIdAsync(articleId);

        // Assert
        Assert.True(result.IsFailure);
        loggerMock.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task SearchArticlesTest_Success_ReturnsPagedResultOfArticleDtos()
    {
        // Arrange
        var parameters = CommonFaker.FakePageParametersGenerator().Generate();
        var articles = ArticleFaker.FakeArticleGenerator().Generate(50);
        var pagedResult = new PagedResult<Article>()
        {
            Data = articles,
            TotalCount = articles.Count,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
        };

        queryMock.Setup(q => q.SearchArticles(parameters, null))
            .ReturnsAsync(Result<PagedResult<Article>>.Success(pagedResult));

        // Act
        var result = await service.SearchArticles(parameters);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Data.Any());
        Assert.Equal(pagedResult.Data.FirstOrDefault().Id, result.Value.Data.First().Id);
        Assert.Equal(pagedResult.Data.FirstOrDefault().Title, result.Value.Data.First().Title);
    }

    [Fact]
    public async Task GetArticlesAsyncTest_Failure_ReturnsFailureResult()
    {
        // Arrange
        var parameters = CommonFaker.FakePageParametersGenerator().Generate();
        queryMock.Setup(q => q.SearchArticles(parameters, null))
            .ReturnsAsync(Result<PagedResult<Article>>.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.SearchArticles(parameters);

        // Assert
        Assert.True(result.IsFailure);
        loggerMock.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsyncTest_Success_ReturnsPagedResultOfArticleDtos()
    {
        // Arrange
        var parameters = CommonFaker.FakePageParametersGenerator().Generate();
        var articles = ArticleFaker.FakeArticleGenerator().Generate(50);
        var pagedResult = new PagedResult<Article>()
        {
            Data = articles,
            TotalCount = articles.Count,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
        };

        queryMock.Setup(q => q.GetAllAsync(parameters.PageNumber, parameters.PageSize))
            .ReturnsAsync(Result<PagedResult<Article>>.Success(pagedResult));

        // Act
        var result = await service.GetAllAsync(parameters.PageNumber, parameters.PageSize);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Data.Any());
        Assert.Equal(pagedResult.Data.FirstOrDefault().Id, result.Value.Data.First().Id);
        Assert.Equal(pagedResult.Data.FirstOrDefault().Title, result.Value.Data.First().Title);
    }

    [Fact]
    public async Task GetAllAsync_Failure_ReturnsFailureResult()
    {
        // Arrange
        var parameters = CommonFaker.FakePageParametersGenerator().Generate();
        queryMock.Setup(q => q.GetAllAsync(parameters.PageNumber, parameters.PageSize))
            .ReturnsAsync(Result<PagedResult<Article>>.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.GetAllAsync(parameters.PageNumber, parameters.PageSize);

        // Assert
        Assert.True(result.IsFailure);
        loggerMock.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }
}
