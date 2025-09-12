using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

[ApiController]
[Route("customer")]
public class DeleteCustomerController : ControllerBase
{
    private readonly IDriver driver;
    private readonly IConfiguration configuration;
    private readonly Neo4jQuery neo4JQuery;
    private const string DELETE="DELETE";
    private const string CUSTOMER = "Customer";
    private const string RETURN = "RETURN";
    public DeleteCustomerController()
    {
        var uri = "neo4j+s://783bf88e.databases.neo4j.io";
        var user = "neo4j";
        var password = "Wdmtz05nXObcb4urKI9lQ5tZDMFPrcsoW7DEGh6-gaQ";

        driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        neo4JQuery = new();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("auth")]
    public IActionResult AutenticateOnlyEndpoint()
    {
        return Ok("You are authenticated");
    }

    //[Authorize(Roles = "Admin,User")]
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<ActionResult> DeleteCustomerAsync(int id)
    {
        try
        {
            await driver.VerifyConnectivityAsync();
            await using var session = driver.AsyncSession();

            var parameters = new Dictionary<string, object>
            {
                {"id",id}
            };
            var testQuety = neo4JQuery.QueryByOneElement(CUSTOMER,"id","id",RETURN);
            var deleteQuery = neo4JQuery.QueryByOneElement(CUSTOMER, "id", "id", DELETE);
            var result = await neo4JQuery.ExecuteReadAsync(session,testQuety,parameters);
            if (result != null)
            {
                await session.ExecuteWriteAsync(async tx =>
                {
                    await tx.RunAsync(deleteQuery, parameters);
                    return Ok(new { message = "Nodes deleted successfully!" });
                });
                return BadRequest("false");
            }
            else return NotFound("Customer not found");
        }
        catch (Exception ex)
        {
            return NotFound(new { message = "False", error = ex });
        }
    }
}