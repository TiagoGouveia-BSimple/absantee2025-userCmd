using Application.DTO;
using Application.IPublishers;
using Application.IService;
using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
namespace Application.Services;


public class UserService : IUserService
{
    private IUserRepository _userRepository;
    private IUserFactory _userFactory;
    private readonly IMessagePublisher _publisher;

    public UserService(IUserRepository userRepository, IUserFactory userFactory, IMessagePublisher publisher)
    {
        _userRepository = userRepository;
        _userFactory = userFactory;
        _publisher = publisher;
    }

    public async Task<CreatedUserDTO> Add(CreateUserDTO userDTO)
    {
        var user = await _userFactory.Create(userDTO.Names, userDTO.Surnames, userDTO.Email, userDTO.FinalDate);
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        await _publisher.PublishCreatedUserMessageAsync(user.Id, user.Names, user.Surnames, user.Email, user.PeriodDateTime);

        return new CreatedUserDTO(user.Id, user.Names, user.Surnames, user.Email, user.PeriodDateTime);
    }

    public async Task<IEnumerable<UserDTO>> GetAll()
    {
        var User = await _userRepository.GetAllAsync();
        return User.Select(u => new UserDTO
        {
            Id = u.Id,
            Names = u.Names,
            Surnames = u.Surnames,
            Email = u.Email,
            Period = u.PeriodDateTime
        });
    }

    public async Task<UserDTO?> GetById(Guid Id)
    {
        var User = await _userRepository.GetByIdAsync(Id);

        if (User == null) return null;

        return new UserDTO
        {
            Id = User.Id,
            Names = User.Names,
            Surnames = User.Surnames,
            Email = User.Email,
            Period = User.PeriodDateTime
        };
    }

    public async Task<UserDTO?> UpdateActivation(Guid Id, ActivationDTO activationDTO)
    {

        var User = (User?)await _userRepository.GetByIdAsync(Id);

        if (User != null)
        {
            await _userRepository.ActivationUser(Id, activationDTO.FinalDate);
            await _userRepository.SaveChangesAsync();
            return new UserDTO
            {
                Id = User.Id,
                Names = User.Names,
                Surnames = User.Surnames,
                Email = User.Email,
                Period = User.PeriodDateTime
            };
        }

        return null;
    }

    public async Task<UserDTO> UpdateUser(UserDTO userDTO)
    {
        var visitor = new UserDataModel()
        {
            Id = userDTO.Id,
            Names = userDTO.Names,
            Surnames = userDTO.Surnames,
            Email = userDTO.Email,
            PeriodDateTime = userDTO.Period
        };

        var User = _userFactory.Create(visitor);

        await _userRepository.UpdateUser(User);
        await _userRepository.SaveChangesAsync();

        await _publisher.PublishUpdatedUserMessageAsync(User.Id, User.Names, User.Surnames, User.Email, User.PeriodDateTime);

        return new UserDTO
        {
            Id = User.Id,
            Names = User.Names,
            Surnames = User.Surnames,
            Email = User.Email,
            Period = User.PeriodDateTime
        };
    }

    public async Task UpdateUserConsumed(Guid id, string names, string surnames, string email, PeriodDateTime periodDateTime)
    {
        var visitor = new UserDataModel()
        {
            Id = id,
            Names = names,
            Surnames = surnames,
            Email = email,
            PeriodDateTime = periodDateTime
        };

        var User = _userFactory.Create(visitor);

        await _userRepository.UpdateUser(User);
        await _userRepository.SaveChangesAsync();
    }

    public async Task<bool> Exists(Guid Id)
    {
        return await _userRepository.Exists(Id);
    }

    public async Task AddConsumed(Guid id, string names, string surnames, string email, PeriodDateTime periodDateTime)
    {
        if (await Exists(id)) return;

        var visitor = new UserDataModel()
        {
            Id = id,
            Names = names,
            Surnames = surnames,
            Email = email,
            PeriodDateTime = periodDateTime
        };

        var user = _userFactory.Create(visitor);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task<IUser> CreateSagaUserAsync(string names, string surnames, string email, DateTime? deactivationDate)
    {
        if (await _userRepository.GetByEmailAsync(email) != null)
        {
            throw new ArgumentException($"O email '{email}' já está em uso.");
        }

        var user = await _userFactory.Create(names, surnames, email, deactivationDate ?? DateTime.MaxValue);
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        await _publisher.PublishCreatedUserMessageAsync(user.Id, user.Names, user.Surnames, user.Email, user.PeriodDateTime);

        return user; 
    }
}
