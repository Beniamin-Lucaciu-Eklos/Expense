using System;
using ExpenseApp.Application.Dtos;
using ExpenseApp.Application.Transactions.CreateTransaction;
using ExpenseApp.Application.Transactions.TransactionQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Api.Controllers;

public class TransactionsController(ISender sender) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAllTransactions()
    {
        var result = await sender.Send(new GetAllTransactionsQuery());

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTransaction(int id)
    {
        var result = await sender.Send(new GetTransactionByIdQuery(id));

        return result.IsSuccess ? Ok(result.Value) : NotFound();
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
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetTransaction),
            new { id = result.Value }, result.Value);
    }
    /*

  

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TransactionDto transactionDto)
    {
        var transaction = new Transaction
        {
            Id = id,
            Amount = transactionDto.Amount,
            Type = transactionDto.Type,
            Category = transactionDto.Category,
            Description = transactionDto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow
        };

        var result = await uowITransaction.UpdateAsync(transaction);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await uowITransaction.Delete(id);
        return result is not null ? Ok(result) : NotFound();
    }*/
}