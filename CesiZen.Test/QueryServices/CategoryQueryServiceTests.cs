using CesiZen.Application.Services;
using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using CesiZen.Infrastructure.Repositories;
using CesiZen.Test.Fakers;
using CesiZen.Test.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using Serilog;

namespace CesiZen.Test.QueryServices;

public class CategoryQueryServiceTests
{
    private readonly Mock<ILogger> mockLogger;
    private readonly Mock<ICategoryQuery> mockQuery;
    private Mock<DbSet<Category>> mockSet;
    private Mock<CesizenDbContext> mockContext;
    private readonly CategoryQueryService service;
    private readonly CategoryQuery repository;

    public CategoryQueryServiceTests()
    {
        mockLogger = new Mock<ILogger>();
        mockQuery = new Mock<ICategoryQuery>();
        mockSet = new Mock<DbSet<Category>>();
        mockContext = new Mock<CesizenDbContext>(Tools.SetContext());
        mockSet = new Mock<DbSet<Category>>();
        service = new CategoryQueryService(mockLogger.Object, mockQuery.Object);
        repository = new CategoryQuery(mockContext.Object);
    }

    [Fact]
    public async Task GetAllAsyncTest_Success_WhenQuerySucceeds()
    {
        // Arrange
        int pageNumber = 1, pageSize = 10;
        var categories = CategoryFaker.FakeCategoryResponseDtoGenerator().Generate(34);
        var pagedResult = new PagedResultDto<CategoryResponseDto>
        {
            Data = categories,
            TotalCount = categories.Count,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        mockQuery.Setup(q => q.GetAllAsync(pageNumber, pageSize))
                  .ReturnsAsync(Result<PagedResultDto<CategoryResponseDto>>.Success(pagedResult));

        // Act
        var result = await service.GetAllAsync(pageNumber, pageSize);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.True(result.Value.Data.Any());
        Assert.Equal(categories[0].Name, result.Value.Data[0].Name);
    }

    [Fact]
    public async Task GetAllAsyncTest_Failure_WhenQueryFails()
    {
        // Arrange
        int pageNumber = 1, pageSize = 10;

        mockQuery.Setup(q => q.GetAllAsync(pageNumber, pageSize))
                  .ReturnsAsync(Result<PagedResultDto<CategoryResponseDto>>.Failure(
                        Error.NullValue("Error occurred")));

        // Act
        var result = await service.GetAllAsync(pageNumber, pageSize);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Null(result.Value);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetManyByIdTest_Success_WhenQuerySucceeds()
    {
        // Arrange
        var categoriesId = new List<int>() { 1, 2, 3 };
        var categories = CategoryFaker.FakeCategoryGenerator().Generate(5);
        categories[0].Id = 1;
        categories[1].Id = 2;
        categories[2].Id = 3;

        mockSet = CommonFaker.CreateMockDbSet(categories);
        mockContext.Setup(c => c.Categories).Returns(mockSet.Object);

        mockQuery.Setup(q => q.GetManyById(It.IsAny<List<int>>()))
                  .ReturnsAsync(Result<List<Category>>.Success(categories));

        // Act
        var result = await repository.GetManyById(categoriesId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.True(result.Value.Any());
        Assert.Equal(categories[0].Name, result.Value[0].Name);
    }

    [Fact]
    public async Task GetManyByIdTest_Failure_WhenQueryFails()
    {
        // Arrange
        var categoriesId = new List<int>() { 1, 2, 3 };
        var categories = CategoryFaker.FakeCategoryGenerator().Generate(5);

        mockSet = CommonFaker.CreateMockDbSet(categories);
        mockContext.Setup(c => c.Categories).Returns(mockSet.Object);

        mockQuery.Setup(q => q.GetManyById(It.IsAny<List<int>>()))
                  .ReturnsAsync(Result<List<Category>>.Failure(
                        Error.NullValue("Error occurred")));

        // Act
        var result = await repository.GetManyById(categoriesId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Null(result.Value);
        Assert.Equal(CategoryErrors.LogMultipleNotFound.Message, result.Error.Message);
    }

    [Fact]
    public async Task GetByIdAsyncTest_Success_WhenQuerySucceeds()
    {
        // Arrange
        int id = 1;
        var category = CategoryFaker.FakeCategoryResponseDtoGenerator().Generate();

        mockQuery.Setup(q => q.GetByIdAsync(id))
                  .ReturnsAsync(Result<CategoryResponseDto>.Success(category));

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(category.Name, result.Value.Name);
    }

    [Fact]
    public async Task GetByIdAsyncTest_Failure_WhenQueryFails()
    {
        // Arrange
        int id = 1;
        var queryResult = Result<CategoryResponseDto>.Failure(
            Error.NullValue("Error occurred"));

        mockQuery.Setup(q => q.GetByIdAsync(id))
                  .ReturnsAsync(queryResult);

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Null(result.Value);
        mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
    }
}
