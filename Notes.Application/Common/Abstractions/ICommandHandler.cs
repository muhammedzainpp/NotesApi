using MediatR;

namespace Notes.Application.Common.Abstractions;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
{}
