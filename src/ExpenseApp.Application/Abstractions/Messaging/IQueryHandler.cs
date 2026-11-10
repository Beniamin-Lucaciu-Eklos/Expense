using ExpenseApp.Domain.Abstractions;
using MediatR;

namespace ExpenseApp.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
   where TQuery : IQuery<TResponse>
{
}