﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.DB.Models.Identity;
using System.IdentityModel.Claims;

namespace TFG_UOC_2024.CORE.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Contact.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Contact.LastName));
            CreateMap<ApplicationUser, UserSimpleDTO>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Contact.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Contact.LastName));

            CreateMap<UserDTO, ApplicationUser>();
            CreateMap<UserInput, ApplicationUser>()
                .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => new Contact() { FirstName = src.FirstName, LastName = src.LastName, Email = src.Email }));

            CreateMap<ApplicationUserRole, UserRoleDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
                .ForMember(dest => dest.RoleNormalizedName, opt => opt.MapFrom(src => src.Role.NormalizedName));

            CreateMap<ApplicationRole, RoleDTO>();
            CreateMap<RoleDTO, ApplicationRole>();
            CreateMap<Claim, ClaimDTO>();

            CreateMap<Contact, ContactSimpleDTO>();
            CreateMap<Contact, ContactDTO>();

            // make sure all datetime values are UTC
            ValueTransformers.Add<DateTime>(val => !((DateTime?)val).HasValue ? val : DateTime.SpecifyKind(val, DateTimeKind.Utc));
            ValueTransformers.Add<DateTime?>(val => val.HasValue ? DateTime.SpecifyKind(val.Value, DateTimeKind.Utc) : val);
        }
    }
}