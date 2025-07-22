using Domain.Interfaces;
using Domain.Visitor;

namespace Domain.Factory;

public interface IUserFactory
{
    public Task<IUser> Create(string names, string surnames, string email, DateTime deactivationDate);
    public IUser Create(IUserVisitor userVisitor);
}