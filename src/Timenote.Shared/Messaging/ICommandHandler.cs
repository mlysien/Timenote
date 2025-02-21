using MediatR;
using Timenote.Shared.Common;
using Timenote.Shared.Kernel;

namespace Timenote.Shared.Messaging;

public interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>;