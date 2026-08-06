using Catalog.API.Products.GetProducts;

namespace Catalog.API.Products.GetProductsByCategory
{
    //public record GetProductsByCategoryRequest();
    public record GetProductsByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductsByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
            {
                try
                {
                    var result = await sender.Send(new GetProductsByCategoryQuery(category));

                    var response = result.Adapt<GetProductsByCategoryResponse>();

                    return Results.Ok(response);
                }
                catch (ProductNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            })
            .WithName("GetProductsByCategory")
            .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Products By Category")
            .WithDescription("Get Products By Category");
        }
    }
}
