using Item.Models;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
[ApiController]
[Route("item")]
public class AddItemController : ControllerBase
{
    private readonly IDriver driver;
    private readonly IConfiguration configuration;
    private readonly Neo4jQuery neo4JQuery;
    private const string ITEM = "Item";
    private const string RETURN = "RETURN";
    public AddItemController()
    {
        var uri = "neo4j+s://783bf88e.databases.neo4j.io";
        var user = "neo4j";
        var password = "Wdmtz05nXObcb4urKI9lQ5tZDMFPrcsoW7DEGh6-gaQ";

        this.driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        neo4JQuery = new();
    }
    [Route("add")]
    [HttpPost]
    public async Task<ActionResult> AddItemAsync([FromBody] ItemModel item)
    {
        try
        {
            await driver.VerifyConnectivityAsync();
            await using var session = driver.AsyncSession();
            if (item.Price < 0 || item.NetoQuantity < 0 || item.AvailableQuantity<0)
            {
                return BadRequest("Some value have incoract value");
            }
            var testQuety = neo4JQuery.QueryByOneElement(ITEM,"id","id",RETURN);
            var query = @"
            CREATE (n:Item {id:$id, name: $name, price: $price, netoQuantity: $netoQuantity, availableQuantity: $availableQuantity})";

            var parameters = new Dictionary<string, object>
            {
                { "id", item.Id },
                { "name", item.Name },
                { "price", item.Price },
                { "netoQuantity", item.NetoQuantity },
                { "availableQuantity", item.AvailableQuantity },
            };
            var result = await neo4JQuery.ExecuteReadAsync(session,testQuety,parameters);
            if (result == null)
            {
                var res = await neo4JQuery.ExecuteWriteAsync(session,query,parameters);
            }
            else return BadRequest("Node " + item.Id + " is already exist" );
            return Ok("Node added successfully");
        }
        catch (Exception ex)
        {
            //Console.WriteLine(ex);
            return BadRequest(ex);
        }
    }
}