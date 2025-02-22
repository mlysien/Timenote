using MediatR;
using Timenote.Shared.Common;

namespace Timenote.Shared.Messaging;

public interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>;