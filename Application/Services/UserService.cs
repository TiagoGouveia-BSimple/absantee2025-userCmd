using Application.DTO;
using Application.IPublishers;
using AutoMapper;
using Domain.Contracts;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore.Update.Internal;
namespace Application.Services;


public class UserService : IUserService
{
    private IUserRepository _userRepository;
    private IUserFactory _userFactory;
    private readonly IMessagePublisher _publisher;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IUserFactory userFactory, IMapper mapper, IMessagePublisher publisher)
    {
        _userRepository = userRepository;
        _userFactory = userFactory;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<UserDTO> Add(UserDTO userDTO)
    {
        var user = await _userFactory.Create(userDTO.Names, userDTO.Surnames, userDTO.Email, userDTO.FinalDate);
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        await _publisher.PublishCreatedUserMessageAsync(user.Id, user.Names, user.Surnames, user.Email, user.PeriodDateTime);

        return _mapper.Map<User, UserDTO>(user);
    }

    public async Task<IEnumerable<IUser>> GetAll()
    {
        var User = await _userRepository.GetAllAsync();
        return User;
    }

    public async Task<IUser?> GetById(Guid Id)
    {
        var User = await _userRepository.GetByIdAsync(Id);
        return User;
    }

    public async Task<UserDTO?> UpdateActivation(Guid Id, ActivationDTO activationDTO)
    {

        var User = (User?)await _userRepository.GetByIdAsync(Id);

        if (User != null)
        {
            await _userRepository.ActivationUser(Id, activationDTO.FinalDate);
            await _userRepository.SaveChangesAsync();
        }
        return _mapper.Map<User, UserDTO>(User);
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
