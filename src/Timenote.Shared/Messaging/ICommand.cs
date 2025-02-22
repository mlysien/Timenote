using MediatR;
using Timenote.Shared.Common;

namespace Timenote.Shared.Messaging;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

public interface IBaseCommand;