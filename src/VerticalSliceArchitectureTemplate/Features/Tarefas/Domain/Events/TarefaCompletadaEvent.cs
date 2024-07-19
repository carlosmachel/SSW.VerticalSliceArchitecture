using MediatR;

namespace VerticalSliceArchitectureTemplate.Features.Tarefas.Domain.Events;

public record TarefaCompletadaEvent(Guid TodoId) : INotification;