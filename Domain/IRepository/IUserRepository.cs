using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.IRepository
{
    public interface IUserRepository : IGenericRepositoryEF<IUser, User, IUserVisitor>
    {
        Task<IUser?> ActivationUser(Guid Id, DateTime FinalDate);
        Task<bool> Exists(Guid ID);

        Task<IEnumerable<IUser>> GetByIdsAsync(List<Guid> userIdsOfCollab);

        Task<IUser?> UpdateUser(IUser user_);
    }
}