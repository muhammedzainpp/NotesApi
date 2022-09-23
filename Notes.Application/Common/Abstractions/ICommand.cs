using MediatR;

namespace Notes.Application.Common.Abstractions;

public interface ICommand<out TResponse> : IRequest<TResponse>
{}
