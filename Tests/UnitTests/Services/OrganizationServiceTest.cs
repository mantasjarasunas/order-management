using AutoBogus;
using Business.Services;
using Domain.Product;
using Moq;
using Persistence.Infrastructure;
using Persistence.Repositories;
using Tests.Infrastructure;
using Xunit;

namespace Tests.UnitTests.Services;

public class ProductServiceTest : UnitTestBase
{
    private readonly Mock<IDbContext> _dbContextMock = new();
    private readonly Mock<IProductRepository> _productRepositoryMock = new();

    [Fact]
    public async Task CreateProductAsync_ShouldCallRepository_AndCommitTransaction()
    {
        var product = new AutoFaker<CreateProductRequestModel>().Generate();
        var service = new ProductService(_dbContextMock.Object, _productRepositoryMock.Object);

        await service.CreateProductAsync(product);

        _productRepositoryMock.Verify(r => r.CreateProductAsync(IsDeep(product)), Times.Once);
        _dbContextMock.Verify(db => db.Commit(), Times.Once);
    }
}