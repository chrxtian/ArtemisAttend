using ArtemisAttend.API.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtemisAttend.API.Profiles
{
    public class UsersProfiles: Profile
    {
        public UsersProfiles()
        {
            CreateMap<Entities.User, Models.UserDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<Models.UserForCreationDto, Entities.User>();
        }
    }
}
