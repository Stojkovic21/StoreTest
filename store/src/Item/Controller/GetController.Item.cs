using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

[ApiController]
[Route("item")]
public class GetItemController : ControllerBase
{
    private readonly IDriver driver;
    private readonly IConfiguration configuration;
    private readonly Neo4jQuery neo4JQuery;
    private const string ITEM = "Item";
    private const string RETURN = "RETURN";
    public GetItemController()
    {
        var uri = "neo4j+s://783bf88e.databases.neo4j.io";
        var user = "neo4j";
        var password = "Wdmtz05nXObcb4urKI9lQ5tZDMFPrcsoW7DEGh6-gaQ";

        this.driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        neo4JQuery = new();
    }
    [Route("get/all")]
    [HttpGet]
    public async Task<ActionResult> GetAllItemsAsync()
    {
        try
        {
            await driver.VerifyConnectivityAsync();
            await using var session = driver.AsyncSession();

            var query = @"MATCH (n:Item) RETURN n";
            var parameters = new Dictionary<string, object> { };

            var result = await session.ExecuteReadAsync(async tx =>
            {
                var response = await tx.RunAsync(query, parameters);
                var nodes = new List<INode>();
                while (await response.FetchAsync())
                {
                    nodes.Add(response.Current["n"].As<INode>());
                }

                return nodes;
            });

            if (result != null)
            {
                var items = result.Select(node => node.Properties).ToList();

                return Ok(new
                {
                    message = "True",
                    items
                });
            }
            else
                return NotFound(new { message = "False" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "False", error = ex });
        }
    }
    [Route("get/id:{id}")]
    [HttpGet]
    public async Task<ActionResult> GetItemByIdAsync(int id)
    {
        try
        {
            await driver.VerifyConnectivityAsync();
            await using var session = driver.AsyncSession();

            var query = neo4JQuery.QueryByOneElement(ITEM,"id","id",RETURN);
            var parameters = new Dictionary<string, object>
            {
                { "id", id }
            };

            var result = await neo4JQuery.ExecuteReadAsync(session,query,parameters);
            if (result != null)
            {
                var properties = result.Properties;

                return Ok(new
                {
                    message = "True",
                    item = properties
                });
            }
            else
                return NotFound(new { message = "Item not found" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "False", error = ex });
        }
    }
    [Route("get/naziv:{naziv}")]
    [HttpGet]
    public async Task<ActionResult> GetItemNameAsync(string naziv)
    {
        try
        {
            await driver.VerifyConnectivityAsync();
            await using var session = driver.AsyncSession();

            var query = neo4JQuery.QueryByOneElement(ITEM,"name","name",RETURN);
            var parameters = new Dictionary<string, object>
            {
                { "name", naziv }
            };

            var result = await neo4JQuery.ExecuteReadAsync(session,query,parameters);
            if (result != null)
            {
                var properties = result.Properties;

                return Ok(new
                {
                    message = "True",
                    item = properties
                });
            }
            else
                return NotFound(new { message = "Item not found" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "False", error = ex });
        }
    }
}