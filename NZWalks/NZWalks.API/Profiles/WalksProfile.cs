using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class WalksProfile :Profile
    {
        public WalksProfile()
        {
            CreateMap<Models.DTO.Walk, Models.Domain.Walk>()
                .ReverseMap();

            CreateMap<Models.DTO.WalkDifficulty, Models.Domain.WalkDifficulty>().ReverseMap();
        }
    }
}
