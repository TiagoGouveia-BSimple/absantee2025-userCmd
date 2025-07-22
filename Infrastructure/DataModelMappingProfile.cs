using AutoMapper;
using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Resolvers;

namespace Infrastructure;

public class DataModelMappingProfile : Profile
{
    public DataModelMappingProfile()
    {
        CreateMap<IUser, UserDataModel>();
        CreateMap<UserDataModel, IUser>()
            .ConvertUsing<UserDataModelConverter>();
    }

}