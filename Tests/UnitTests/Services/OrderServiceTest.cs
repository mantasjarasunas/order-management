using AutoBogus;
using Business.Services;
using Domain.Order;
using FluentAssertions;
using Moq;
using Persistence.Infrastructure;
using Persistence.Repositories;
using Tests.Infrastructure;
using Xunit;

namespace Tests.UnitTests.Services;

public class OrderServiceTest : UnitTestBase
{
    private readonly Mock<IDbContext> _dbContextMock = new();
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly OrderService _service;

    public OrderServiceTest()
    {
        _service = new OrderService(_orderRepositoryMock.Object, _dbContextMock.Object);
    }

    [Fact]
    public async Task CreateOrderAsync_WithValidModel_CallsRepositoryAndCommits()
    {
        var order = new AutoFaker<CreateOrderRequestModel>().Generate();

        await _service.CreateOrderAsync(order);

        _orderRepositoryMock.Verify(r => r.CreateOrderAsync(IsDeep(order)), Times.Once);
        _dbContextMock.Verify(db => db.Commit(), Times.Once);
    }

    [Fact]
    public async Task CreateOrderAsync_WhenRepositoryThrows_DoesNotCommit()
    {
        var order = new AutoFaker<CreateOrderRequestModel>().Generate();
        _orderRepositoryMock.Setup(x => x.CreateOrderAsync(It.IsAny<CreateOrderRequestModel>())).ThrowsAsync(new Exception("DB error"));

        await Assert.ThrowsAsync<Exception>(() => _service.CreateOrderAsync(order));
        _dbContextMock.Verify(db => db.Commit(), Times.Never);
    }

    [Fact]
    public async Task GetOrdersAsync_ShouldReturnExpectedList()
    {
        var expected = new AutoFaker<OrderListItem>().Generate(2);
        _orderRepositoryMock.Setup(x => x.GetOrdersAsync()).ReturnsAsync(expected);

        var result = await _service.GetOrdersAsync();

        result.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public async Task GetOrderInvoiceAsync_WithValidOrderId_ReturnsInvoiceWithCorrectTotal()
    {
        const long orderId = 1;
        var items = new List<OrderInvoiceItemModel>
        {
            new() { Name = "Order A", Quantity = 2, DiscountPercentage = 10, Amount = 18 },
            new() { Name = "Order B", Quantity = 1, DiscountPercentage = null, Amount = 15 }
        };
        _orderRepositoryMock.Setup(x => x.GetOrderInvoiceItemsAsync(orderId)).ReturnsAsync(items);

        var result = await _service.GetOrderInvoiceAsync(orderId);

        result.Items.Should().BeEquivalentTo(items);
        result.TotalAmount.Should().Be(33);
    }
} 