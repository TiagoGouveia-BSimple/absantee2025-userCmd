using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.Visitor;
using Infrastructure.DataModel;
using Infrastructure.Resolvers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepositoryEF : GenericRepositoryEF<IUser, User, UserDataModel>, IUserRepository
{
    private readonly IMapper _mapper;
    public UserRepositoryEF(AbsanteeContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<Guid>> GetByNamesAsync(string names)
    {
        if (string.IsNullOrWhiteSpace(names))
            return new List<Guid>();

        var usersDM = await _context.Set<UserDataModel>()
            .Where(u => EF.Functions.Like(u.Names, $"%{names}%"))
            .ToListAsync();

        var userIds = usersDM.Select(u => u.Id);

        return userIds;
    }

    public async Task<IEnumerable<Guid>> GetBySurnamesAsync(string surnames)
    {
        if (string.IsNullOrWhiteSpace(surnames))
            return new List<Guid>();

        var usersDM = await _context.Set<UserDataModel>()
            .Where(u => EF.Functions.Like(u.Surnames, $"%{surnames}%"))
            .ToListAsync();

        return usersDM.Select(u => u.Id);
    }

    public async Task<IEnumerable<Guid>> GetByNamesAndSurnamesAsync(string names, string surnames)
    {
        if (string.IsNullOrWhiteSpace(names) && string.IsNullOrWhiteSpace(surnames))
            return new List<Guid>();

        var usersDM = await _context.Set<UserDataModel>()
            .Where(u =>
                (string.IsNullOrWhiteSpace(names) || EF.Functions.Like(u.Names, $"%{names}%")) &&
                (string.IsNullOrWhiteSpace(surnames) || EF.Functions.Like(u.Surnames, $"%{surnames}%"))
            )
            .ToListAsync();

        return usersDM.Select(u => u.Id);
    }


    public async Task<Guid?> GetByEmailAsync(string email)
    {
        var userDM = await _context.Set<UserDataModel>().FirstOrDefaultAsync(c => c.Email == email);

        if (userDM == null)
        {
            return null;
        }

        return userDM.Id;
    }

    public override IUser? GetById(Guid id)
    {
        var userDM = _context.Set<UserDataModel>().FirstOrDefault(c => c.Id == id);

        if (userDM == null)
            return null;

        var user = _mapper.Map<UserDataModel, IUser>(userDM);
        return user;
    }

    public override async Task<IUser?> GetByIdAsync(Guid id)
    {
        var userDM = await _context.Set<UserDataModel>().FirstOrDefaultAsync(u => u.Id == id);

        if (userDM == null)
            return null;

        var user = _mapper.Map<UserDataModel, IUser>(userDM);
        return user;
    }

    public async Task<IEnumerable<IUser>> GetByIdsAsync(List<Guid> userIdsOfCollab)
    {
        var usersDM = await _context.Set<UserDataModel>()
            .Where(u => userIdsOfCollab.Contains(u.Id))
            .ToListAsync();

        return usersDM.Select(u => _mapper.Map<IUser>(u));
    }

    public async Task<bool> Exists(Guid ID)
    {
        var userDM = await _context.Set<UserDataModel>().FirstOrDefaultAsync(u => u.Id == ID);
        if (userDM == null)
            return false;
        return true;
    }
    public async Task<IUser?> ActivationUser(Guid Id, DateTime FinalDate)
    {

        var userDataModel = await _context.Set<UserDataModel>()
                .FirstAsync(u => u.Id == Id);

        userDataModel.PeriodDateTime = new PeriodDateTime(userDataModel.PeriodDateTime._initDate, FinalDate.ToUniversalTime());
        _context.Entry(userDataModel).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        var user = _mapper.Map<UserDataModel, IUser>(userDataModel);
        return user;
    }

    public async Task<IUser?> UpdateUser(IUser user)
    {
        var userDM = await _context.Set<UserDataModel>().FirstOrDefaultAsync(u => u.Id == user.Id);

        if (userDM == null) return null;

        userDM.Names = user.Names;
        userDM.Surnames = user.Surnames;
        userDM.Email = user.Email;
        userDM.PeriodDateTime = user.PeriodDateTime;

        _context.Set<UserDataModel>().Update(userDM);
        return _mapper.Map<UserDataModel, IUser>(userDM);
    }
}
