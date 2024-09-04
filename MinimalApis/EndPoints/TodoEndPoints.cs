/*
 * @Author: Jun
 * @Description:
 */

using MinimalApis.Minimal;

namespace MinimalApis.EndPoints;

public class TodoEndPoints: IEndPoint
{
    public void MapEndPoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Todo");
        // swagger分组
        group.WithTags("Todo");
        group.MapGet("/", () => "Get").WithSummary("Get接口");
        group.MapPost("/", () => "Post").WithSummary("Post接口");
        group.MapDelete("/", () => "Delete").WithSummary("Delete接口");
        group.MapPut("/", () => "Put").WithSummary("Put接口");
    }
}