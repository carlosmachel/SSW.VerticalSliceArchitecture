
using Carter;
using MediatR;
using VerticalSliceArchitectureTemplate.Common.CQRS;
using VerticalSliceArchitectureTemplate.Features.Tarefas.Domain;

namespace VerticalSliceArchitectureTemplate.Features.Tarefas.Commands;

    
public sealed record DeletarTarefaCommand(Guid Id) : ICommand<Unit>;

public sealed partial class DeletarTarefa: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/tarefas/{id:guid}",
                async ([FromRoute] Guid id, ISender sender, CancellationToken cancellationToken) =>
                {
                    await sender.Send(new DeletarTarefaCommand(id), cancellationToken);
                    return Results.NoContent();
                })
            .WithName("DeletarTarefa")
            .WithSummary("DeletarTarefa")
            .WithDescription("Deletar Tarefa")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(nameof(Tarefa));
    }
}

internal class DeletarTarefaCommandHandler(AppDbContext dbContext)
    : ICommandHandler<DeletarTarefaCommand, Unit>
{
    public async Task<Unit> Handle(DeletarTarefaCommand request, CancellationToken cancellationToken)
    {
        var todo = await dbContext.Tarefas.FindAsync([request.Id], cancellationToken);
        if (todo == null) throw new NotFoundException(nameof(Tarefas), request.Id);
        dbContext.Tarefas.Remove(todo);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
