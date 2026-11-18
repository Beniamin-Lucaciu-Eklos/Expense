using System;
using System.Security.Claims;
using ExpenseApp.Application.Dtos;
using ExpenseApp.Application.Transactions.CreateTransaction;
using ExpenseApp.Application.Transactions.DeleteTransaction;
using ExpenseApp.Application.Transactions.TransactionQueries;
using ExpenseApp.Application.Transactions.UpdateTransaction;
using ExpenseApp.Domain.Data.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Api.Controllers;

[Authorize]
public class TransactionsController(ISender sender) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAllTransactions()
    {
        var result = await sender.Send(new GetAllTransactionsQuery());

        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTransaction(int id)
    {
        var result = await sender.Send(new GetTransactionByIdQuery(id));

        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                TransactionErrors.Codes.NotFound => NotFound(result.Error.Name),

                TransactionErrors.Codes.UnAuthorized => BadRequest(result.Error.Name),

                _ => BadRequest(result.Error)
            };
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto transactionDto)
    {
        var command = new CreateTransactionCommand(
            transactionDto.Type,
            transactionDto.Amount,
            transactionDto.Category,
            transactionDto.Description);

        var result = await sender.Send(command);

        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                TransactionErrors.Codes.NotFound => NotFound(result.Error.Name),

                TransactionErrors.Codes.UnAuthorized => BadRequest(result.Error.Name),

                _ => BadRequest(result.Error)
            };
        }

        return CreatedAtAction(nameof(GetTransaction),
            new { id = result.Value }, result.Value);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await sender.Send(new DeleteTransactionCommand(id));

        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                TransactionErrors.Codes.NotFound => NotFound(result.Error.Name),

                TransactionErrors.Codes.UnAuthorized => BadRequest(result.Error.Name),

                _ => BadRequest(result.Error)
            };
        }

        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TransactionDto transactionDto)
    {
        var result = await sender.Send(new UpdateTransactionCommand(id, transactionDto));

        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                TransactionErrors.Codes.NotFound => NotFound(result.Error.Name),

                TransactionErrors.Codes.UnAuthorized => BadRequest(result.Error.Name),

                _ => BadRequest(result.Error)
            };
        }

        return NoContent();
    }
}