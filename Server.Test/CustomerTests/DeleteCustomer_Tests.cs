using Microsoft.AspNetCore.Mvc;

[TestFixture]
public class DeleteCustomer_Tests
{
    private static DeleteCustomerController _customer;
    [SetUp]
    public void SetUp()
    {
        _customer = new DeleteCustomerController();
    }
    [Test]
    public async Task DeleteCustomer_NotFound()
    {
        int id = 167;

        var results = await _customer.DeleteCustomerAsync(id);

        Assert.That(results, Is.InstanceOf<NotFoundObjectResult>());
        var value = results as NotFoundObjectResult;
        Assert.That(value, Has.Property("Value").EqualTo("Customer not found"));
    }
}