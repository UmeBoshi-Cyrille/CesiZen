using CesiZen.Application.Services;
using CesiZen.Domain.BusinessResult;
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
        var articleId = 1;
        var article = ArticleFaker.FakeArticleDtoGenerator().Generate();
        var expectedDto = article.Map();

        queryMock.Setup(q => q.GetByIdAsync(articleId))
            .ReturnsAsync(Result<ArticleDto>.Success(article));

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
        var articleId = 1;
        queryMock.Setup(q => q.GetByIdAsync(articleId))
            .ReturnsAsync(Result<ArticleDto>.Failure(Error.NullValue("Error message")));

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
        var articles = ArticleFaker.FakeArticleMinimumDtoGenerator().Generate(50);
        var pagedResult = new PagedResultDto<ArticleMinimumDto>()
        {
            Data = articles,
            TotalCount = articles.Count,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
        };

        queryMock.Setup(q => q.SearchArticles(parameters, ""))
            .ReturnsAsync(Result<PagedResultDto<ArticleMinimumDto>>.Success(pagedResult));

        // Act
        var result = await service.SearchArticles(parameters);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Data.Any());
        Assert.Equal(pagedResult.Data.FirstOrDefault()!.Id, result.Value.Data.FirstOrDefault()!.Id);
        Assert.Equal(pagedResult.Data.FirstOrDefault()!.Title, result.Value.Data.FirstOrDefault()!.Title);
    }

    [Fact]
    public async Task GetArticlesAsyncTest_Failure_ReturnsFailureResult()
    {
        // Arrange
        var parameters = CommonFaker.FakePageParametersGenerator().Generate();
        queryMock.Setup(q => q.SearchArticles(parameters, ""))
            .ReturnsAsync(Result<PagedResultDto<ArticleMinimumDto>>.Failure(Error.NullValue("Error message")));

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
        var articles = ArticleFaker.FakeArticleMinimumDtoGenerator().Generate(50);
        var pagedResult = new PagedResultDto<ArticleMinimumDto>()
        {
            Data = articles,
            TotalCount = articles.Count,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
        };

        queryMock.Setup(q => q.GetAllAsync(parameters.PageNumber, parameters.PageSize))
            .ReturnsAsync(Result<PagedResultDto<ArticleMinimumDto>>.Success(pagedResult));

        // Act
        var result = await service.GetAllAsync(parameters.PageNumber, parameters.PageSize);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Data.Any());
        Assert.Equal(pagedResult.Data.FirstOrDefault()!.Id, result.Value.Data.FirstOrDefault()!.Id);
        Assert.Equal(pagedResult.Data.FirstOrDefault()!.Title, result.Value.Data.FirstOrDefault()!.Title);
    }

    [Fact]
    public async Task GetAllAsync_ReturnFailure_DataNotFound()
    {
        // Arrange
        var parameters = CommonFaker.FakePageParametersGenerator().Generate();
        queryMock.Setup(q => q.GetAllAsync(parameters.PageNumber, parameters.PageSize))
            .ReturnsAsync(Result<PagedResultDto<ArticleMinimumDto>>.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.GetAllAsync(parameters.PageNumber, parameters.PageSize);

        // Assert
        Assert.True(result.IsFailure);
        loggerMock.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetLastTest_Success_ReturnsListOfLastArticles()
    {
        // Arrange
        var articles = ArticleFaker.FakeArticleMinimumDtoGenerator().Generate(50);

        queryMock.Setup(q => q.GetLast(10))
            .ReturnsAsync(Result<List<ArticleMinimumDto>>.Success(articles));

        // Act
        var result = await service.GetLast(10);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Any());
        Assert.Equal(articles.FirstOrDefault()!.Id, result.Value.FirstOrDefault()!.Id);
        Assert.Equal(articles.FirstOrDefault()!.Title, result.Value.FirstOrDefault()!.Title);
    }

    [Fact]
    public async Task GetLastTest_Failure_DataNotFound()
    {
        // Arrange
        queryMock.Setup(q => q.GetLast(5))
            .ReturnsAsync(Result<List<ArticleMinimumDto>>.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.GetLast(5);

        // Assert
        Assert.True(result.IsFailure);
        loggerMock.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByCategoryTest_Success_ReturnsPagedResultOfArticleDto()
    {
        // Arrange
        var parameters = CommonFaker.FakePageParametersGenerator().Generate();
        var articles = ArticleFaker.FakeArticleMinimumDtoGenerator().Generate(50);
        var pagedResult = new PagedResultDto<ArticleMinimumDto>()
        {
            Data = articles,
            TotalCount = articles.Count,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
        };

        queryMock.Setup(q => q.GetByCategory(1, parameters.PageNumber, parameters.PageSize))
            .ReturnsAsync(Result<PagedResultDto<ArticleMinimumDto>>.Success(pagedResult));

        // Act
        var result = await service.GetByCategory(1, parameters.PageNumber, parameters.PageSize);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Data.Any());
        Assert.Equal(pagedResult.Data.FirstOrDefault()!.Id, result.Value.Data.FirstOrDefault()!.Id);
        Assert.Equal(pagedResult.Data.FirstOrDefault()!.Title, result.Value.Data.FirstOrDefault()!.Title);
    }

    [Fact]
    public async Task GetByCategoryTest_ReturnFailure_DataNotFound()
    {
        // Arrange
        var parameters = CommonFaker.FakePageParametersGenerator().Generate();
        queryMock.Setup(q => q.GetByCategory(1, parameters.PageNumber, parameters.PageSize))
            .ReturnsAsync(Result<PagedResultDto<ArticleMinimumDto>>.Failure(Error.NullValue("Error message")));

        // Act
        var result = await service.GetByCategory(1, parameters.PageNumber, parameters.PageSize);

        // Assert
        Assert.True(result.IsFailure);
        loggerMock.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }
}
