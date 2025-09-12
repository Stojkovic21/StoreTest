using System.Reflection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ItemTests;

[TestFixture]
public class GetItem_Tests
{
    private static GetItemController _item;
    [SetUp]
    public void SetUp()
    {
        _item = new GetItemController();
    }
    [Test]
    public async Task GetItem_WithID()
    {
        var id = 1;
        //ACT
        var results = await _item.GetItemByIdAsync(id);

        //ASSERT
        Assert.That(results, Is.InstanceOf<OkObjectResult>());
    }
    [Test]
    public async Task GetItem_WithID_NotFound()
    {
        var id = 168;
        //ACT
        var results = await _item.GetItemByIdAsync(id);

        //ASSERT
        Assert.That(results, Is.InstanceOf<NotFoundObjectResult>());
        var notFoundResult = results as NotFoundObjectResult;
        var value = notFoundResult?.Value;
        var message = value?.GetType().GetProperty("message");
        var messageValue = message?.GetValue(value);
        Assert.That(messageValue, Is.EqualTo("Item not found"));
    }
    [Test]
    public async Task GetItem_WithID_IsBigBottle()
    {
        var id = 0;
        //ACT
        var results = await _item.GetItemByIdAsync(id);

        //ASSERT
        Assert.That(results, Is.InstanceOf<OkObjectResult>());
        var findItem = results as OkObjectResult;
        var value = findItem?.Value;
        var item = value?.GetType().GetProperty("item")?.GetValue(value);
        var itemDict = item as Dictionary<string, object>;
    
        Assert.That(itemDict?["id"], Is.EqualTo(id));
        Assert.That(itemDict?["netoQuantity"], Is.GreaterThanOrEqualTo(500));
    }
    [TearDown]
    public void TearDown()
    {
        
    }
    
}