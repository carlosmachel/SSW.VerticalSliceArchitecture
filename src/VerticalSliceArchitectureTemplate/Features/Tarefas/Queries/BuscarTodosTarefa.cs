using System.Linq.Expressions;
using Carter;
using MediatR;
using VerticalSliceArchitectureTemplate.Common.CQRS;
using VerticalSliceArchitectureTemplate.Features.Tarefas.Domain;

namespace VerticalSliceArchitectureTemplate.Features.Tarefas.Queries;

public sealed record BuscarTodosTarefaQuery(
    int Page,
    int PageSize,
    string? SortColumn, 
    string? SortOrder, 
    bool? IsCompleted, 
    string? Text) : IQuery<PagedList<Tarefa>>;

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

internal class BuscarTodosTarefaQueryHandler(AppDbContext dbContext) : IQueryHandler<BuscarTodosTarefaQuery,PagedList<Tarefa>>
{
    public async Task<PagedList<Tarefa>> Handle(BuscarTodosTarefaQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Tarefa> query = dbContext.Tarefas;

        if (request.IsCompleted != null)
            query = query.Where(x => x.IsCompleted == request.IsCompleted);

        if (request.Text != null)
            query = query.Where(x => x.Text.Contains(request.Text));
        
        query = request.SortOrder?.ToLower() == "desc" 
            ? query.OrderByDescending(GetSortProperty(request)) 
            : query.OrderBy(GetSortProperty(request));
        
        return await PagedList<Tarefa>.CreateAsync(query, request.Page, request.PageSize, 
            cancellationToken);
    }

    private static Expression<Func<Tarefa, object>> GetSortProperty(BuscarTodosTarefaQuery request) =>
        request.SortColumn?.ToLower() switch
        {
            "isCompleted" => tarefa => tarefa.IsCompleted,
            _ => tarefa => tarefa.Text
        };
}

public class PagedList<T>
{
    private PagedList(List<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
    
    public List<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasNextPage => Page * PageSize < TotalCount;
    public bool HasPreviousPage => Page > 1;

    public static async Task<PagedList<T>> CreateAsync(
        IQueryable<T> query, int page, int pageSize,
        CancellationToken cancellationToken)
    {
        var totalCount = await query.CountAsync(cancellationToken: cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken: cancellationToken);
        return new(items, page, pageSize, totalCount);
    }
}
