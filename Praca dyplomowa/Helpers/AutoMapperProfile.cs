using AutoMapper;
using Praca_dyplomowa.Entities;
using Praca_dyplomowa.Models;

namespace Praca_dyplomowa.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();

            CreateMap<Training, TrainingJSON>();
            CreateMap<EditTrainingJSON, Training>();
            CreateMap<NewTrainingJSON, Training>();
        }
    }
}
