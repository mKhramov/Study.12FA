using AutoMapper;
using Study.TFA.API.Models;

namespace Study.TFA.API.Mapping
{
    public class ApiProfile: Profile
    {
        public ApiProfile() 
        {
            CreateMap<Domain.Models.Forum, Forum>();
            CreateMap<Domain.Models.Topic, Topic>();
        }
    }
}
