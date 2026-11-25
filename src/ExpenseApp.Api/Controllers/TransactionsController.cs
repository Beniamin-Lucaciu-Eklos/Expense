using System;
using System.ComponentModel.DataAnnotations;
using ExpenseApp.Application.Dtos;
using ExpenseApp.Application.Transactions.CreateTransaction;
using ExpenseApp.Application.Transactions.DeleteTransaction;
using ExpenseApp.Application.Transactions.TransactionQueries;
using ExpenseApp.Application.Transactions.UpdateTransaction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Api.Controllers;

[Authorize]
public class TransactionsController(ISender sender) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetTransactions()
    {
        var result = await sender.Send(new GetAllTransactionsQuery());

        return Ok(result.Value);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetTransactionsPage(
        CancellationToken cancellationToken,
        [FromQuery, Range(1, 1_000_000)] int page = 1,
        [FromQuery, Range(1, 100)] int pageSize = 50)
    {
        var result = await sender.Send(
            new GetTransactionsPageQuery(page, pageSize),
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleError(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetMonthlySummary(
        [FromQuery, Range(1, 12)] int month,
        [FromQuery, Range(1900, 9999)] int year,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetMonthlyTransactionSummaryQuery(year, month),
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleError(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("summary/all-time")]
    public async Task<IActionResult> GetTransactionTotals(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetTransactionTotalsQuery(), cancellationToken);
        if (result.IsFailure)
        {
            return HandleError(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTransaction(int id)
    {
        var result = await sender.Send(new GetTransactionByIdQuery(id));

        if (result.IsFailure)
        {           
            return HandleError(result.Error);
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
            return HandleError(result.Error);
        }

        return CreatedAtAction(nameof(GetTransaction),
            new { id = result.Value }, result.Value);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        var result = await sender.Send(new DeleteTransactionCommand(id));

        if (result.IsFailure)
        {            
            return HandleError(result.Error);
        }

        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionDto transactionDto)
    {
        var result = await sender.Send(new UpdateTransactionCommand(id, transactionDto));

        if (result.IsFailure)
        {
            return HandleError(result.Error);
        }

        return NoContent();
    }
}