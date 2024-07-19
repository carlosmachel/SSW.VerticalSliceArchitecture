using Carter;
using MediatR;
using VerticalSliceArchitectureTemplate.Common.CQRS;
using VerticalSliceArchitectureTemplate.Features.Tarefas.Domain;

namespace VerticalSliceArchitectureTemplate.Features.Tarefas.Queries;

public sealed record BuscarTodosTarefaQuery(bool? IsCompleted = null) : IQuery<IReadOnlyList<Tarefa>>;

public sealed partial class BuscarTodosTarefa : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tarefas",
                ([AsParameters] BuscarTodosTarefaQuery query, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = sender.Send(query, cancellationToken);
                    return Results.Ok(result);
                })
            .WithName("BuscarTodosTarefa")
            .WithSummary("BuscarTodosTarefa")
            .WithDescription("Buscar Todos Tarefa")
            .Produces<IReadOnlyList<Tarefa>>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(nameof(Tarefa));
    }
}

internal class BuscarTodosTarefaQueryHandler(AppDbContext dbContext) : IQueryHandler<BuscarTodosTarefaQuery,IReadOnlyList<Tarefa>>
{
    public async Task<IReadOnlyList<Tarefa>> Handle(BuscarTodosTarefaQuery request, CancellationToken cancellationToken)
    {
        var todos = await dbContext.Tarefas
            .Where(x => request.IsCompleted == null || x.IsCompleted == request.IsCompleted)
            .ToListAsync(cancellationToken);

        return todos.AsReadOnly();
    }
}
