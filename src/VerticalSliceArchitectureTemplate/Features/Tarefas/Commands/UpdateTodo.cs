

namespace VerticalSliceArchitectureTemplate.Features.Tarefas.Commands;

/*
public sealed partial class AtualizarTarefa : ICarterModule
{
    public sealed record Command(Guid Id, string Text);

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/todos/{id:guid}",
                async (Guid id, Command command, ISender sender) =>
                {
                    var request = command with
                    {
                        Id = id // TODO: Remove this duplication
                    };
                    var result = await sender.Send(command);
                    return Results.NoContent();
                })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(nameof(Todo));
    }
    
    private static async ValueTask HandleAsync(Command request, AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var todo = await dbContext.Todos.FindAsync([request.Id], cancellationToken);

        if (todo == null) throw new NotFoundException(nameof(Todo), request.Id);

        todo.Text = request.Text;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    
}
*/