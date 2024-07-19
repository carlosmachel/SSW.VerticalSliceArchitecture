using MediatR;

namespace VerticalSliceArchitectureTemplate.Features.Tarefas.Domain.Events;

public record TarefaCriadaEvent(Guid TodoId) : INotification;