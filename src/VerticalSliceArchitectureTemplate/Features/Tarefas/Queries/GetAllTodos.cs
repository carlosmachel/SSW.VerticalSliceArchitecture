
namespace VerticalSliceArchitectureTemplate.Features.Tarefas.Queries;

/*
[Handler]
public sealed partial class GetAllTodos
{
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/todos",
                ([AsParameters] Query query, Handler handler, CancellationToken cancellationToken)
                    => handler.HandleAsync(query, cancellationToken))
            .Produces<IReadOnlyList<Todo>>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(nameof(Todo));
    }
    
    public sealed record Query(bool? IsCompleted = null);

    private static async ValueTask<IReadOnlyList<Todo>> HandleAsync(Query request, AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var todos = await dbContext.Todos
            .Where(x => request.IsCompleted == null || x.IsCompleted == request.IsCompleted)
            .ToListAsync(cancellationToken);

        return todos.AsReadOnly();
    }
}
*/