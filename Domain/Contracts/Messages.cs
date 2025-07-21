using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Contracts
{
    public record CreateUserCommand(
        Guid TempCollabId,
        string Names,
        string Surnames,
        string Email,
        DateTime? DeactivationDate
    );

    public record UserForCollabCommandMessage(
       Guid Id,
       PeriodDateTime PeriodDateTime,
       string Names,
       string Surnames,
       string Email,
       DateTime FinalDate
   );

    public record UserCreated(
        Guid CorrelationId,
        Guid UserId
    );

    public record UserCreationFaulted(
        Guid CorrelationId,
        string Email,
        string Reason
    );

}