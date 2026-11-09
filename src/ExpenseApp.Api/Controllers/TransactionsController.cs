using System;
using ExpenseApp.Application.Dtos;
using ExpenseApp.Application.Services;
using ExpenseApp.Domain.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Api.Controllers;

public class TransactionsController(ITransactionUnitOfWork uowITransaction) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAllTransactions()
    {
        var result = await uowITransaction.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTransaction(int id)
    {
        var result = await uowITransaction.GetByIdAsync(id);

        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto transactionDto)
    {
        var transaction = new Transaction
        {
            Amount = transactionDto.Amount,
            Type = transactionDto.Type,
            Category = transactionDto.Category,
            Description = transactionDto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow
        };

        transaction = await uowITransaction.CreateAsync(transaction);

        return CreatedAtAction(nameof(GetTransaction),
            new { id = transaction.Id }, transaction);
    }

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
    }
}
