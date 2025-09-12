using Microsoft.AspNetCore.Mvc;

[TestFixture]
public class PostCustomer_Tests
{
    private static AuthCustomerController _customer;
    [SetUp]
    public void SetUp()
    {
        _customer = new AuthCustomerController();
    }
    [Test]
    public async Task SignUpCustomerTest_BadRequest()
    {
        var customer = new KupacModel { Id = 100, Email = "luka@elfak.rs", Password = "password", Ime = "Luka", Prezime = "Stojkovic", BrTel = "06545654", Role = "User" };

        var results = await _customer.SignUp(customer);
        Assert.That(results, Is.InstanceOf<ConflictObjectResult>());
        var value = results as ConflictObjectResult;
        Assert.That(value, Has.Property("Value").EqualTo("Customer already exists"));
    }
    [Test]
    public async Task SignUpCustomerTest_EmailFormat_BadRequest()
    {
        var customer = new KupacModel { Id = 100, Email = "luka-elfak.rs", Password = "password", Ime = "Luka", Prezime = "Stojkovic", BrTel = "06545654", Role = "User" };

        var results = await _customer.SignUp(customer);
        Assert.That(results, Is.InstanceOf<BadRequestObjectResult>());
        var value = results as BadRequestObjectResult;
        Assert.That(value, Has.Property("Value").EqualTo("Incorrect email format"));
    }
    [Test]
    public async Task LoginCustomerTest_BadRequest()
    {
        var email = "testirenjesoftvera@elfak.rs";
        var password = "password";

        var results = await _customer.Login(new LoginModel(email, password));
        Assert.That(results, Is.InstanceOf<BadRequestObjectResult>());
        var value = results as BadRequestObjectResult;
        Assert.That(value, Has.Property("Value").EqualTo("Incorrect email or password"));
    }
    [Test]
    public async Task LoginpCustomerTest_EmailFormat_BadRequest()
    {
        var email = "testirenjesoftvera-elfak.rs";
        var password = "password";

        var results = await _customer.Login(new LoginModel(email, password));
        Assert.That(results, Is.InstanceOf<BadRequestObjectResult>());
        var value = results as BadRequestObjectResult;
        Assert.That(value, Has.Property("Value").EqualTo("Incorrect email format"));
    }
}