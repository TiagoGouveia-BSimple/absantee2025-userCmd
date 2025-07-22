using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Infrastructure.DataModel;

namespace Infrastructure.Resolvers;

public class UserDataModelConverter : ITypeConverter<UserDataModel, IUser>
{
    private readonly IUserFactory _UserFactory;

    public UserDataModelConverter(IUserFactory UserFactory)
    {
        _UserFactory = UserFactory;
    }

    public IUser Convert(UserDataModel source, IUser destination, ResolutionContext context)
    {
        return _UserFactory.Create(source);
    }

    public bool UpdateDataModel(UserDataModel userDataModel, IUser userDomain)

    {
        userDataModel.Id = userDomain.Id;

        // pode ser necessário mais atualizações, e com isso o retorno não ser sempre true
        // contudo, porque userDataModel está a ser gerido pelo DbContext, para atualizarmos a DB, é este que tem de ser alterado, e não criar um novo

        userDataModel.PeriodDateTime._initDate = userDomain.PeriodDateTime._initDate;
        userDataModel.PeriodDateTime._finalDate = userDomain.PeriodDateTime._finalDate;
        return true;
    }
}