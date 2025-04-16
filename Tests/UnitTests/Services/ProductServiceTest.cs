using AutoBogus;
using Business.Services;
using Domain.Product;
using FluentAssertions;
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
    private readonly ProductService _service;

    public ProductServiceTest()
    {
        _service = new ProductService(_dbContextMock.Object, _productRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateProductAsync_WithValidModel_CallsRepositoryAndCommits()
    {
        var product = new AutoFaker<CreateProductRequestModel>().Generate();

        await _service.CreateProductAsync(product);

        _productRepositoryMock.Verify(r => r.CreateProductAsync(IsDeep(product)), Times.Once);
        _dbContextMock.Verify(db => db.Commit(), Times.Once);
    }

    [Fact]
    public async Task CreateProductAsync_WhenRepositoryThrows_DoesNotCommit()
    {
        var product = new AutoFaker<CreateProductRequestModel>().Generate();
        _productRepositoryMock.Setup(x => x.CreateProductAsync(It.IsAny<CreateProductRequestModel>())).ThrowsAsync(new Exception("DB error"));

        await Assert.ThrowsAsync<Exception>(() => _service.CreateProductAsync(product));
        _dbContextMock.Verify(db => db.Commit(), Times.Never);
    }

    [Fact]
    public async Task GetFilteredProductsAsync_WithQuery_ReturnsExpectedList()
    {
        const string searchQuery = "test";
        var expected = new AutoFaker<ProductListItemModel>().Generate(3);
        _productRepositoryMock.Setup(x => x.GetFilteredProductsAsync(searchQuery)).ReturnsAsync(expected);

        var result = await _service.GetFilteredProductsAsync(searchQuery);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task UpdateProductDiscountAsync_WithValidModel_CallsRepositoryAndCommits()
    {
        const int productId = 123;
        var request = new AutoFaker<UpdateProductDiscountRequestModel>().Generate();

        await _service.UpdateProductDiscountAsync(productId, request);

        _productRepositoryMock.Verify(r => r.UpdateProductDiscountAsync(productId, IsDeep(request)), Times.Once);
        _dbContextMock.Verify(db => db.Commit(), Times.Once);
    }

    [Fact]
    public async Task UpdateProductDiscountAsync_WhenRepositoryThrows_DoesNotCommit()
    {
        const int productId = 123;
        var request = new AutoFaker<UpdateProductDiscountRequestModel>().Generate();
        _productRepositoryMock.Setup(x => x.UpdateProductDiscountAsync(productId, It.IsAny<UpdateProductDiscountRequestModel>())).ThrowsAsync(new Exception("DB error"));

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateProductDiscountAsync(productId, request));
        _dbContextMock.Verify(db => db.Commit(), Times.Never);
    }
}