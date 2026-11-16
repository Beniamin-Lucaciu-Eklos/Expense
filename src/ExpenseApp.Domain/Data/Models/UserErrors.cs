using System;
using ExpenseApp.Domain.Abstractions;

namespace ExpenseApp.Domain.Data.Models;

public static class UserErrors
{
    public static Error EmailAlreadyExists = new("UserErrors.NotFound",
  "The user with email already exists");

}
