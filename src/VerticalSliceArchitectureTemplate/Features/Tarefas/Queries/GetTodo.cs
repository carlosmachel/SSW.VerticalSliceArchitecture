using Carter;
using MediatR;
using VerticalSliceArchitectureTemplate.Common.CQRS;
using VerticalSliceArchitectureTemplate.Features.Tarefas.Domain;

namespace VerticalSliceArchitectureTemplate.Features.Tarefas.Queries;

public sealed record BuscarTarefaQuery(Guid Id) : IQuery<Tarefa>;

public sealed partial class GetTodo : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tarefas/{id:guid}",
                (Guid id, ISender sender, CancellationToken cancellationToken)
                    =>
                {
                    var result = sender.Send(new BuscarTarefaQuery(id), cancellationToken);
                    return result;
                })
            .WithName("BuscarTarefa")
            .WithSummary("BuscarTarefa")
            .WithDescription("Buscar Tarefa")
            .Produces<Tarefa>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(nameof(Tarefa));
    }
}

internal class BuscarTodosQueryHandler(AppDbContext dbContext) : IQueryHandler<BuscarTarefaQuery,Tarefa>
{
    public async Task<Tarefa> Handle(BuscarTarefaQuery request, CancellationToken cancellationToken)
    {
        var todo = await dbContext.Tarefas.FindAsync([request.Id], cancellationToken);
        if (todo == null) throw new NotFoundException(nameof(Tarefa), request.Id);
        return todo; 
    }
}
