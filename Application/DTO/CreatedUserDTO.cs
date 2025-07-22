using Domain.Models;

namespace Application.DTO;

public record CreatedUserDTO(Guid Id, string Names, string Surnames, string Email, PeriodDateTime Period);
