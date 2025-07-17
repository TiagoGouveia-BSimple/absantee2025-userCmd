using Domain.Models;
using Domain.Messages;
using MassTransit;
using Application.IPublishers;

namespace InterfaceAdapters.Publishers;

public class MassTransitPublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishCreatedUserMessageAsync(Guid id, string names, string surnames, string email, PeriodDateTime periodDateTime)
    {
        await _publishEndpoint.Publish(new UserCreatedMessage(id, names, surnames, email, periodDateTime));
        await _publishEndpoint.Publish(new UserCreatedForCollab(id, names, surnames, email, periodDateTime));
    }
}
