using Carter;
using MediatR;
using VerticalSliceArchitectureTemplate.Common.CQRS;
using VerticalSliceArchitectureTemplate.Features.Tarefas.Domain;

namespace VerticalSliceArchitectureTemplate.Features.Tarefas.Commands;

public sealed record CriarTarefaCommand(string Text) : ICommand<CriarTarefaResult>;

public sealed record CriarTarefaResult(Guid Id);

public sealed partial class CriarTarefa : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/tarefas",
                async (CriarTarefaCommand command, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(command, cancellationToken);
                    return Results.Created($"/tarefas/{result.Id}", result);
                })
            .WithName("CriarTarefa")
            .WithSummary("CriarTarefa")
            .WithDescription("Criar Tarefa")
            .Produces<CriarTarefaResult>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(nameof(Tarefa));
    }
}

internal class CriarTarefaCommandHandler(AppDbContext dbContext)
    : ICommandHandler<CriarTarefaCommand, CriarTarefaResult>
{
    public async Task<CriarTarefaResult> Handle(CriarTarefaCommand request, CancellationToken cancellationToken)
    {
        var tarefa = new Tarefa
        {
            Text = request.Text
        };

        await dbContext.Tarefas.AddAsync(tarefa, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CriarTarefaResult(tarefa.Id);
    }
}