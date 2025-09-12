using Microsoft.AspNetCore.Mvc;

[TestFixture]
public class GetOrdersByCustomer_Tests
{
    private static GetCustomerController _customer;
    [SetUp]
    public void SetUp()
    {
        _customer = new GetCustomerController();
    }
    [Test]
    public async Task GetAllOrders_BuyCustomerId_Ok()
    {
        int id = 1;

        var results = await _customer.GetAllOrdersOfCustomerById(id);

        Assert.That(results, Is.InstanceOf<OkObjectResult>());
    }
    [Test]
    public async Task GetAllOrders_BuyCustomerId_NotFoundId()
    {
        int id = 167;

        var results = await _customer.GetAllOrdersOfCustomerById(id);

        Assert.That(results, Is.InstanceOf<NotFoundObjectResult>());
        var value = results as NotFoundObjectResult;
        Assert.That(value, Has.Property("Value").EqualTo("This customer does not have any order yet"));
    }
    [Test]
    public async Task GetAllOrders_BuyCustomerId_WithPriceLimit_NotFoundId()
    {
        int id = 1;
        int price = 100;
        var results = await _customer.GetAllOrdersOfCustomerById(id, price);

        Assert.That(results, Is.InstanceOf<OkObjectResult>());
        var value = results as OkObjectResult;
        var orders = value?.Value?.GetType().GetProperty("orders");
        var ordersDict = orders?.GetValue(value?.Value) as IEnumerable<object>;
        Assert.That(ordersDict?.Count(), Is.EqualTo(2));
    }
}