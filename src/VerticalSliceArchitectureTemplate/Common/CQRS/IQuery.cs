using MediatR;

namespace VerticalSliceArchitectureTemplate.Common.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : notnull;