using Microsoft.AspNetCore.Mvc;

[TestFixture]
public class PutCustomers_Tests
{
    private static EditCustomerController _customer;
    [SetUp]
    public void SetUp()
    {
        _customer = new EditCustomerController();
    }
    [Test]
    public async Task PutCustomer_IncorrectEmailFormat_BadRequest()
    {
        var customer = new KupacModel { Email = "luka-elfak.rs", Password = "password", Ime = "Luka", Prezime = "Stojkovic", BrTel = "06545654" };

        var results = await _customer.EditCustomerAsync(customer);

        Assert.That(results, Is.InstanceOf<BadRequestObjectResult>());
        var value = results as BadRequestObjectResult;
        Assert.That(value, Has.Property("Value").EqualTo("Incorrect email format"));
    }
    [Test]
    public async Task PutCustomer_IncorrectPassword_BadRequest()
    {
        var customer = new KupacModel { Email = "luka@elfak.rs", Password = "password", Ime = "Luka", Prezime = "Stojkovic", BrTel = "06545654" };

        var results = await _customer.EditCustomerAsync(customer);

        Assert.That(results, Is.InstanceOf<BadRequestObjectResult>());
        var value = results as BadRequestObjectResult;
        Assert.That(value, Has.Property("Value").EqualTo("Incorrect email or password"));
    }
    [Test]
    public async Task PutCustomer_CustomerDoesNotFound_BadRequest()
    {
        var customer = new KupacModel { Email = "testirenjesoftvera@elfak.rs", Password = "password", Ime = "Luka", Prezime = "Stojkovic", BrTel = "06545654" };

        var results = await _customer.EditCustomerAsync(customer);

        Assert.That(results, Is.InstanceOf<BadRequestObjectResult>());
        var value = results as BadRequestObjectResult;
        Assert.That(value, Has.Property("Value").EqualTo("Incorrect email or password"));
    }
}