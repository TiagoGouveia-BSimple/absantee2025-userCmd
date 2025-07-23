using Domain.Models;

namespace Application.DTO;

public record UpdateUserDTO(Guid Id, string Names, string Surnames, string Email, PeriodDateTime Period);
