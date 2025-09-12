using Item.Models;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

[ApiController]
[Route("item")]
public class EditItemController : ControllerBase
{
    private readonly IDriver driver;
    private readonly IConfiguration configuration;
    private readonly Neo4jQuery neo4JQuery;
    public EditItemController()
    {
        var uri = "neo4j+s://783bf88e.databases.neo4j.io";
        var user = "neo4j";
        var password = "Wdmtz05nXObcb4urKI9lQ5tZDMFPrcsoW7DEGh6-gaQ";

        this.driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        neo4JQuery = new();
    }
    [HttpPut]
    [Route("put/{id}")]
    public async Task<ActionResult> EditItemAsync([FromBody] ItemModel updatedItem, int id)
    {
        try
        {
            await driver.VerifyConnectivityAsync();
            await using var session = driver.AsyncSession();
            if (updatedItem.Price < 0 || updatedItem.NetoQuantity < 0 || updatedItem.AvailableQuantity<0)
            {
                return BadRequest("Some value have incoract value");
            }
            var query = @"
            MATCH (n:Item {id: $id})
            SET n += $properties
            RETURN n";

            var parameters = new Dictionary<string, object>
            {
                { "id", id },
                { "properties", new Dictionary<string, object>
                    {
                        { "name", updatedItem.Name },
                        { "price", updatedItem.Price },
                        { "netoQuantity",updatedItem.NetoQuantity},
                        { "availableQuantity", updatedItem.AvailableQuantity }
                    }
                }
            };
            var updatedNode = await neo4JQuery.ExecuteWriteAsync(session,query,parameters);
            if (updatedNode != null)
            {
                var properties = updatedNode.Properties;

                return Ok(new
                {
                    message = "Item updated successfully!",
                    updatedItem = properties
                });
            }
            else
            {
                return NotFound("Item not found!");
            }
        }
        catch (Exception ex)
        {
            //Console.WriteLine(ex);
            return BadRequest(ex);
        }
    }
}