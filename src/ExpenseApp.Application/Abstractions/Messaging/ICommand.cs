using ExpenseApp.Domain.Data.Abstractions;
using MediatR;

namespace ExpenseApp.Application.Abstractions.Messaging;


public interface IBaseCommand
{

}

public interface ICommand : IRequest<Result>, IBaseCommand
{
}

public interface ICommand<TResponse>
    : IRequest<Result<TResponse>>, IBaseCommand
{
}