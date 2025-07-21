using Application.DTO;
using Domain.Interfaces;
using Domain.Models;

public interface IUserService
{
    Task<UserDTO> Add(UserDTO userDTO);
    Task AddConsumed(Guid id, string names, string surnames, string email, PeriodDateTime periodDateTime);
    Task<bool> Exists(Guid Id);
    Task<IEnumerable<IUser>> GetAll();
    Task<IUser?> GetById(Guid Id);
    Task<UserDTO?> UpdateActivation(Guid Id, ActivationDTO activationDTO);
    Task<IUser> CreateSagaUserAsync(string names, string surnames, string email, DateTime? deactivationDate);
}
