using Application.IService;
using Application.Services;
using Domain.Messages;
using MassTransit;

public class UserCreatedConsumer : IConsumer<UserCreatedMessage>
{
    private readonly IUserService _userService;

    public UserCreatedConsumer(IUserService userService)
    {
        _userService = userService;
    }
    public async Task Consume(ConsumeContext<UserCreatedMessage> context)
    {
        var msg = context.Message;
        await _userService.AddConsumed(msg.Id, msg.Names, msg.Surnames, msg.Email, msg.PeriodDateTime);
    }
}
