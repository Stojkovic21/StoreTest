using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

[ApiController]
[Route("relationship")]
public class OrderCustomerRelationship : ControllerBase
{
    private readonly IDriver driver;
    private readonly IConfiguration configuration;
    private readonly Relationsip createRelationship;

    public OrderCustomerRelationship(IConfiguration configuration)
    {
        this.configuration = configuration;
        var uri = this.configuration.GetValue<string>("Neo4j:Uri");
        var user = this.configuration.GetValue<string>("Neo4j:Username");
        var password = this.configuration.GetValue<string>("Neo4j:Password");

        driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        createRelationship = new Relationsip(driver);
    }
    [HttpPost]
    [Route("customer/connect")]
    public async Task<ActionResult> ConncetRelationshiCustomerpAsync([FromBody] RelationshipModel relationshipModel)
    {
        var query = @"
            MATCH (a:Order {id: $targetId})
            MATCH (b:Customer {id: $sourceId})
            MERGE (b)-[r:" + relationshipModel.RelationshipType + @"]->(a)
            RETURN a, r, b";

        // Create parameters for the query
        var parameters = new Dictionary<string, object>
        {
            { "sourceId", relationshipModel.SourceId },
            { "targetId", relationshipModel.TargetId }
        };

        return await createRelationship.Relationship(query, parameters);
    }
    [HttpDelete]
    [Route("customer/breakup")]
    public async Task<ActionResult> BreakupRelationshipCustomerAsync([FromBody] RelationshipModel relationshipModel)
    {
        var query = @"
            MATCH (a:Order {id: $targetId})<-[r:" + relationshipModel.RelationshipType + @"]-(b:Customer {id: $sourceId})
            DELETE r
            RETURN a, b";

        // Create parameters for the query
        var parameters = new Dictionary<string, object>
        {
            { "sourceId", relationshipModel.SourceId },
            { "targetId", relationshipModel.TargetId }
        };

        return await createRelationship.Relationship(query, parameters);
    }
}