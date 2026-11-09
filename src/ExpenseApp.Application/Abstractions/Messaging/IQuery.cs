using ExpenseApp.Domain.Data.Abstractions;
using MediatR;

namespace ExpenseApp.Application.Abstractions.Messaging;

public interface IQuery<TResponse> :
    IRequest<Result<TResponse>>
{
}