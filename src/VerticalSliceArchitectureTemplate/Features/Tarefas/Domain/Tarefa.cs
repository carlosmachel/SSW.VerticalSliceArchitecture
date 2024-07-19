using System.ComponentModel.DataAnnotations;
using FluentValidation;
using VerticalSliceArchitectureTemplate.Common.Domain;
using VerticalSliceArchitectureTemplate.Features.Tarefas.Domain.Events;

namespace VerticalSliceArchitectureTemplate.Features.Tarefas.Domain;

public class Tarefa : BaseEntity
{
    public Tarefa()
    {
        StagedEvents.Add(new TarefaCriadaEvent(Id));
    }
    
    public Guid Id { get; init; }
    
    public string Text { get; set; } = string.Empty;
    public string Opcional { get; set; } = string.Empty;
    
    public bool IsCompleted { get; private set; }
    
    /// <exception cref="InvalidOperationException">Throws when trying to complete an already completed item</exception>
    public void Complete()
    {
        if (IsCompleted)
        {
            throw new InvalidOperationException("Todo is already completed");
        }

        IsCompleted = true;
        StagedEvents.Add(new TarefaCompletadaEvent(Id));
    }
}

public class TarefaValidator : AbstractValidator<Tarefa> 
{
    public TarefaValidator()
    {
        RuleFor(tarefa => tarefa.Text).NotNull();
        RuleFor(tarefa => tarefa.Opcional).NotNull().NotEmpty();
    }
}
