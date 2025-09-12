using Item.Models;
using Microsoft.AspNetCore.Mvc;

namespace ItemTests;

[TestFixture]
public class PostItem_Tests
{
    private static AddItemController _item;
    [SetUp]
    public void SetUp()
    {
        _item = new AddItemController();
    }
    [Test]
    public async Task PostItem_WithIncorectValue_BadRequest()
    {
        var newItem = new ItemModel { Id = 2, Name = "Smoki", Price = 50, NetoQuantity = 45, AvailableQuantity = -10 };

        var results = await _item.AddItemAsync(newItem);

        Assert.That(results, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestObjectResult = results as BadRequestObjectResult;
        Assert.That(badRequestObjectResult, Has.Property("Value").EqualTo("Some value have incoract value"));
    }
    [Test]
    public async Task PostItem_ItemAlreadyExists_BadRequest()
    {
        var newItem = new ItemModel { Id = 1, Name = "Smoki", Price = 50, NetoQuantity = 45, AvailableQuantity = 10 };

        var results = await _item.AddItemAsync(newItem);

        Assert.That(results, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestObjectResult = results as BadRequestObjectResult;
        Assert.That(badRequestObjectResult, Has.Property("Value").EqualTo("Node " + newItem.Id + " is already exist"));
    }
    // [Test]
    // public async Task PostItem_ItemSuccessfullyAdded_OK()
    // {
    //     var newItem = new ItemModel { Id = 2, Name = "Smoki", Price = 50, NetoQuantity = 45, AvailableQuantity = 10 };

    //     var results = await _item.AddItemAsync(newItem);

    //     Assert.That(results, Is.InstanceOf<OkResult>());
    // }
}