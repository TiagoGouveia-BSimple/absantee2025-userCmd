using Domain.Models;

public interface IMessagePublisher
{
    Task PublishCreatedUserMessageAsync(Guid id, string names, string surnames, string email, PeriodDateTime periodDateTime);
}
