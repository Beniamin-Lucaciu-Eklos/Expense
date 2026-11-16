using ExpenseApp.Application.Abstractions.Messaging;
using ExpenseApp.Domain.Abstractions;
using ExpenseApp.Domain.Data.Models;

namespace ExpenseApp.Application.Authorization;

internal class RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork uow) : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
{
    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var dto = command.RegisterUserDto;
        if (await userRepository.ExistsAsync(dto.Email))
            Result.Failure<RegisterUserResponse>(UserErrors.EmailAlreadyExists);

        var user = User.Create(dto.Email, dto.Password);
        await userRepository.AddAsync(user);
        await uow.SaveChangesAsync();

        return Result.Success(new RegisterUserResponse(user.Token));
    }
}