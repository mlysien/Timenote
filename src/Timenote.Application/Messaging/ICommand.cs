using MediatR;
using Timenote.Application.Common;

namespace Timenote.Application.Messaging;

/// <summary>
/// Abstraction for commands that not returning complex result
/// </summary>
public interface ICommand : IRequest<Result>, IBaseCommand;

/// <summary>
/// Abstraction for commands that returning complex result
/// </summary>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

/// <summary>
/// Command marker
/// </summary>
public interface IBaseCommand;