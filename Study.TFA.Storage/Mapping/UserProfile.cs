using AutoMapper;
using Study.TFA.Domain.UseCases.SignIn;

namespace Study.TFA.Storage.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, RecognisedUser>();
        }
    }
}
