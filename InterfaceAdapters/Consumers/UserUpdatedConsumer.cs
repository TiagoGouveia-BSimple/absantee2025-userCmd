using Application.IService;
using Application.Services;
using Domain.Messages;
using MassTransit;

public class UserUpdatedConsumer : IConsumer<UserUpdatedMessage>
{
    private readonly IUserService _userService;

    public UserUpdatedConsumer(IUserService userService)
    {
        _userService = userService;
    }
    public async Task Consume(ConsumeContext<UserUpdatedMessage> context)
    {
        var msg = context.Message;
        await _userService.UpdateUserConsumed(msg.Id, msg.Names, msg.Surnames, msg.Email, msg.PeriodDateTime);
    }
}
