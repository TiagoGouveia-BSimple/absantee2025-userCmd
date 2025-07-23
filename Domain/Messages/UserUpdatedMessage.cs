using Domain.Models;

namespace Domain.Messages;

public record UserUpdatedMessage(Guid Id, string Names, string Surnames, string Email, PeriodDateTime PeriodDateTime);
