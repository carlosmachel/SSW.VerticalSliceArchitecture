using FluentAssertions;
using VerticalSliceArchitectureTemplate.Features.Tarefas.Domain;
using VerticalSliceArchitectureTemplate.Features.Tarefas.Domain.Events;

namespace VerticalSliceArchitectureTemplate.Unit.Tests.Features.Tarefas.Models;

public class TarefaTests
{
    [Fact]
    public void Tarefa_Complete_ShouldUpdateCompleted()
    {
        // Arrange
        var item = new Tarefa
        {
            Id = Guid.NewGuid(),
            Text = "minha tarefa"
        };
        
        // Act
        item.Complete();
        
        // Assert
        item.IsCompleted.Should().BeTrue();
    }
    
    [Fact]
    public void Todo_Complete_ShouldAddEvent()
    {
        // Arrange
        var item = new Tarefa
        {
            Id = Guid.NewGuid(),
            Text = "My todo item"
        };
        
        // Act
        item.Complete();
        
        // Assert
        item.StagedEvents.Should().ContainSingle(x => x is TarefaCompletadaEvent, "because the item was completed");
    }
}