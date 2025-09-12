using Microsoft.AspNetCore.Mvc;

[TestFixture]
public class DeleteItem_Tests
{
    public static DeleteItemController _item;
    [SetUp]
    public void SetUp()
    {
        _item = new DeleteItemController();
    }
    [Test]
    public async Task DeleteItem_ItemNotFound()
    {
        var id = 168;

        var results = await _item.ObrisiItemAsync(id);
        Assert.That(results, Is.InstanceOf<NotFoundObjectResult>());

        var notFoundObjectResult = results as NotFoundObjectResult;
        Assert.That(notFoundObjectResult, Has.Property("Value").EqualTo("Item not found!"));
    }
    [Test]
    [Ignore("Ignor a test")]    
    public async Task DeleteItem_Ok()
    {
        var id = 2;

        var results = await _item.ObrisiItemAsync(id);
        Assert.That(results, Is.InstanceOf<OkObjectResult>());

        var okObjectResult = results as OkObjectResult;
        Assert.That(okObjectResult, Has.Property("Value").EqualTo("Node deleted successfully"));
    }
    
}