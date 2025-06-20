using Domain.Models;

namespace Domain.Messages;

public record UserCreatedMessage(Guid Id, string Names, string Surnames, string Email, PeriodDateTime PeriodDateTime);
