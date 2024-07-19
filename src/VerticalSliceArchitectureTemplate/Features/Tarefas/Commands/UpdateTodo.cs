using Carter;
using MediatR;
using VerticalSliceArchitectureTemplate.Common.CQRS;
using VerticalSliceArchitectureTemplate.Features.Tarefas.Domain;

namespace VerticalSliceArchitectureTemplate.Features.Tarefas.Commands;

public sealed record AlterarTarefaCommand(Guid Id, string Text) : ICommand<Unit>;

public sealed partial class AtualizarTarefa : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/tarefas/{id:guid}",
                async (Guid id, AlterarTarefaCommand command, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(command with { Id = id }, cancellationToken);
                    return Results.NoContent();
                })
            .WithName("AlterarTarefa")
            .WithSummary("AlterarTarefa")
            .WithDescription("Alterar Tarefa")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(nameof(Tarefa));
    }
    
    private static async ValueTask HandleAsync(AlterarTarefaCommand request, AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var todo = await dbContext.Tarefas.FindAsync([request.Id], cancellationToken);
        if (todo == null) throw new NotFoundException(nameof(Tarefas), request.Id);
        todo.Text = request.Text;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

internal class AlterarTarefaCommandHandler(AppDbContext dbContext) : ICommandHandler<AlterarTarefaCommand, Unit>
{
    public async Task<Unit> Handle(AlterarTarefaCommand request, CancellationToken cancellationToken)
    {
        var tarefa = await dbContext.Tarefas.FindAsync([request.Id], cancellationToken);
        if (tarefa == null) throw new NotFoundException(nameof(Tarefas), request.Id);
        tarefa.Text = request.Text;
        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
