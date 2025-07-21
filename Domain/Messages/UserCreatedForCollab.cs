using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Messages
{
    public record UserCreatedForCollab(Guid Id, string Names, string Surnames, string Email, PeriodDateTime PeriodDateTime);

}