using MediatR;

namespace Notes.Application.Common.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{}
