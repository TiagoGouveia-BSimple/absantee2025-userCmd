using Application.DTO;
using Domain.Interfaces;
using Domain.Models;

namespace Application.IService;

public interface IUserService
{
    Task<CreatedUserDTO> Add(CreateUserDTO userDTO);
    Task AddConsumed(Guid id, string names, string surnames, string email, PeriodDateTime periodDateTime);
    Task<bool> Exists(Guid Id);
    Task<IEnumerable<UserDTO>> GetAll();
    Task<UserDTO?> GetById(Guid Id);
    Task<UserDTO?> UpdateActivation(Guid Id, ActivationDTO activationDTO);
    Task<UserDTO> UpdateUser(UserDTO userDTO);
    Task<UserDTO> UpdateUserConsumed(Guid id, string names, string surnames, string email, PeriodDateTime periodDateTime);

    Task<IUser> CreateSagaUserAsync(string names, string surnames, string email, DateTime? deactivationDate);
}
