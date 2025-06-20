using Application.Services;
using Domain.Messages;
using MassTransit;

public class UserCreatedConsumer : IConsumer<UserCreatedMessage>
{
    private readonly UserService _userService;

    public UserCreatedConsumer(UserService userService)
    {
        _userService = userService;
    }
    public async Task Consume(ConsumeContext<UserCreatedMessage> context)
    {
        var msg = context.Message;
        await _userService.CreateAsync(msg.Id, msg.Names, msg.Surnames, msg.Email, msg.PeriodDateTime);
    }
}
