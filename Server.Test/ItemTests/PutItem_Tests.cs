using Item.Models;
using Microsoft.AspNetCore.Mvc;

namespace ItemTests;
[TestFixture]
public class PutItem_Tests
{
    private static EditItemController _item;
    [SetUp]
    public void SetUp()
    {
        _item = new EditItemController();
    }
    [Test]
    public async Task PutItem_WithIncorectValue_BadRequest()
    {
        var newItem = new ItemModel { Id = 2, Name = "Smoki", Price = 50, NetoQuantity = 45, AvailableQuantity = -10 };

        var results = await _item.EditItemAsync(newItem,newItem.Id);

        Assert.That(results, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestObjectResult = results as BadRequestObjectResult;
        Assert.That(badRequestObjectResult, Has.Property("Value").EqualTo("Some value have incoract value"));
    }
    [Test]
    public async Task PutItem_ItemDontExists_NotFound()
    {
        var newItem = new ItemModel { Id = 167, Name = "Smoki", Price = 50, NetoQuantity = 45, AvailableQuantity = 10 };

        var results = await _item.EditItemAsync(newItem,newItem.Id);

        Assert.That(results, Is.InstanceOf<NotFoundObjectResult>());
        var notFoundObjectResult = results as NotFoundObjectResult;
        Assert.That(notFoundObjectResult, Has.Property("Value").EqualTo("Item not found!"));
    }
    [Test]
    public async Task PutItem_ItemSuccessfullyUpdated_OK()
    {
        var newItem = new ItemModel { Id = 0, Name = "Pepsi", Price = 135, NetoQuantity = 2000, AvailableQuantity = 15 };

        var results = await _item.EditItemAsync(newItem, newItem.Id);

        Assert.That(results, Is.InstanceOf<OkObjectResult>());

        var response = results as OkObjectResult;
        var responseProp = response?.Value?.GetType()?.GetProperty("message")?.GetValue(response?.Value);
        Assert.That(responseProp, Is.EqualTo("Item updated successfully!"));
    }
}