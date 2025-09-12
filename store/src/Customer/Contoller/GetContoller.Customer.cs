using System.Net;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

[ApiController]
[Route("customer")]
public class GetCustomerController : ControllerBase
{
    private readonly IDriver driver;
    private readonly IConfiguration configuration;
    private readonly Neo4jQuery neo4JQuary;
    private const string RETURN = "RETURN";
    private const string CUSTOMER = "Customer";

    public GetCustomerController()
    {
        var uri = "neo4j+s://783bf88e.databases.neo4j.io";
        var user = "neo4j";
        var password = "Wdmtz05nXObcb4urKI9lQ5tZDMFPrcsoW7DEGh6-gaQ";

        this.driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        neo4JQuary = new();
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("get/all")]
    public async Task<ActionResult> GetAllCustomerAsync()
    {
        try
        {
            await driver.VerifyConnectivityAsync();
            await using var session = driver.AsyncSession();
            var query = $"MATCH (n:{CUSTOMER})RETURN n";
            var parameters = new Dictionary<string, object>
            {
                {"Customer",CUSTOMER}
            };
            var result = await session.ExecuteReadAsync(async tx =>
            {
                var response = await tx.RunAsync(query, parameters);
                var customers = new List<INode>();
                while (await response.FetchAsync())
                {
                    customers.Add(response.Current["n"].As<INode>());
                }
                return customers;
            });
            if (result != null)
            {
                return Ok(new
                {
                    message = "True",
                    customer = result.Select(s => s.Properties)
                });
            }
            else return NotFound("Customers not found");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "False", error = ex });
        }
    }
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> GetCustomerAsync(int id)
    {
        try
        {
            await driver.VerifyConnectivityAsync();
            await using var session = driver.AsyncSession();

            var query = neo4JQuary.QueryByOneElement("Customer", "id", "id", RETURN);
            var parameters = new Dictionary<string, object>
            {
                {"id",id}
            };
            var result = await neo4JQuary.ExecuteReadAsync(session, query, parameters);
            if (result != null)
            {
                return Ok(new
                {
                    message = "True",
                    Customer = result.Properties
                });
            }
            else return NotFound("Customer is not found");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "False", error = ex });
        }
    }
    [HttpGet]
    [Route("me")]
    public async Task<ActionResult> GetMe()
    {
        AuthCustomerController authCustomerController = new();
        try
        {
            string refreshToken = Request.Cookies["refreshToken"].ToString();
            Console.WriteLine(refreshToken);
            await driver.VerifyConnectivityAsync();
            await using var session = driver.AsyncSession();

            var query = neo4JQuary.QueryByOneElement("Customer", "refreshToken", "refreshToken", RETURN);
            // var parameters = new Dictionary<string, object>
            // {
            //     {"refreshToken",refreshToken}
            // };

            // var result = await neo4JQuary.ExecuteReadAsync(session,query, parameters);

            // if (result != null)
            // {
            //     return Ok(new ResponseTokenModel
            //     {
            //         AccessToken = authCustomerController.CreateJWT(new JwtModel
            //         {
            //             Email = result.Properties["email"]?.ToString(),
            //             UserId = result.Properties["id"]?.ToString(),
            //             Role = result.Properties["role"]?.ToString()
            //         }),
            //         UserId =int.Parse(result.Properties["id"]?.ToString()),
            //         Role = result.Properties["role"]?.ToString(),
            //     });
            // }
            //else
            return NotFound(new { message = "Customer is not found" });
        }
        catch (Exception ex)
        {
            return NotFound(new { message = "False", error = ex });
        }
    }
    [HttpGet]
    [Route("getOrders/{id}")]
    public async Task<ActionResult> GetAllOrdersOfCustomerById(int id)
    {
        try
        {
            await driver.VerifyConnectivityAsync();
            await using var session = driver.AsyncSession();
            var query = @"
            MATCH (c:Customer {id: $customerId})-[r]->(o:Order)
            RETURN o";
            var parameters = new { customerId = id };

            var result = await session.ExecuteReadAsync(async tx =>
            {
                var response = await tx.RunAsync(query, parameters);
                var orders = new List<INode>();
                while (await response.FetchAsync())
                {
                    orders.Add(response.Current["o"].As<INode>());
                }
                return orders;
            });
            if (result.FirstOrDefault() != null)
            {
                return Ok(new
                {
                    message = "True",
                    orders = result.Select(s => s.Properties)
                });
            }
            else return NotFound("This customer does not have any order yet");

        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Faild", error = ex });
        }
    }
    [HttpGet]
    [Route("getOrders/{id}/{price}")]
    public async Task<ActionResult> GetAllOrdersOfCustomerById(int id, int price)
    {
        try
        {
            await driver.VerifyConnectivityAsync();
            await using var session = driver.AsyncSession();
            var query = @"
            MATCH (c:Customer {id: $customerId})-[r]->(o:Order)
            WHERE o.price>$price
            RETURN o";
            var parameters = new { customerId = id, price = price };
            
            var result = await session.ExecuteReadAsync(async tx =>
            {
                var response = await tx.RunAsync(query, parameters);
                var orders = new List<INode>();
                while (await response.FetchAsync())
                {
                    orders.Add(response.Current["o"].As<INode>());
                }
                return orders;
            });
            if (result.FirstOrDefault() != null)
            {
                return Ok(new
                {
                    message = "True",
                    orders = result.Select(s => s.Properties)
                });
            }
            else return NotFound("This customer does not have order with price is more then "+price+" yet");
            
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Faild", error = ex });
        }
    }
}
