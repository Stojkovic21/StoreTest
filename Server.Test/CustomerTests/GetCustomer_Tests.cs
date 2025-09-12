using Microsoft.AspNetCore.Mvc;

[TestFixture]
public class GetCustomer_Tests
{
    private static GetCustomerController _customer;
    [SetUp]
    public void SetUp()
    {
        _customer = new GetCustomerController();
    }
    [Test]
    [Ignore("Only if")]
    public async Task GetAllCustomersTest_NotFound()
    {
        var results = await _customer.GetAllCustomerAsync();

        Assert.That(results, Is.InstanceOf<NotFoundObjectResult>());
        var value = results as NotFoundObjectResult;
        Assert.That(value, Has.Property("Value").EqualTo("Customers not found"));
    }
    [Test]
    public async Task GetCustomersByIdTest_NotFound()
    {
        int id = 167;
        var results = await _customer.GetCustomerAsync(id);
        
        Assert.That(results, Is.InstanceOf<NotFoundObjectResult>());
        var value = results as NotFoundObjectResult;
        Assert.That(value, Has.Property("Value").EqualTo("Customer is not found"));
    }
    [Test]
    public async Task GetCustomersByIdTest_Ok()
    {
        int id = 0;
        var results = await _customer.GetCustomerAsync(id);

        Assert.That(results, Is.InstanceOf<OkObjectResult>());
        var value = results as OkObjectResult;
        var customer = value?.Value;
        var customerMessageValue = value?.Value?.GetType()?.GetProperty("message")?.GetValue(value.Value);
        Assert.That(customerMessageValue, Is.EqualTo("True"));
    }
}