using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

[ApiController]
[Route("item")]
public class DeleteItemController : ControllerBase
{
    private readonly IDriver driver;
    private readonly IConfiguration configuration;
    private readonly Neo4jQuery neo4JQuery;
    private const string ITEM = "Item";
    private const string RETURN = "RETURN";
    public DeleteItemController()
    {
         var uri = "neo4j+s://783bf88e.databases.neo4j.io";
        var user = "neo4j";
        var password = "Wdmtz05nXObcb4urKI9lQ5tZDMFPrcsoW7DEGh6-gaQ";

        this.driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        neo4JQuery = new();
    }
    [Route("delete/{id}")]
    [HttpDelete]

    public async Task<ActionResult> ObrisiItemAsync(int id)
    {
        try
        {
            await driver.VerifyConnectivityAsync();
            await using var session = driver.AsyncSession();

            var testQuety = neo4JQuery.QueryByOneElement(ITEM,"id","id",RETURN);
            var deleteQuery = @"
            MATCH (n:Item {id: $id})
            DETACH DELETE n";
            var parameters = new Dictionary<string, object>
            {
                { "id", id }
            };

            var result = await neo4JQuery.ExecuteReadAsync(session,testQuety,parameters);
            if (result != null)
            {
                var res = await neo4JQuery.ExecuteWriteAsync(session,deleteQuery,parameters);
                return Ok("Node deleted successfully");
            }
            else return NotFound("Item not found!");

        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}