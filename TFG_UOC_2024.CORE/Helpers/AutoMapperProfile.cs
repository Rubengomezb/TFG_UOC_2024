using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.DB.Models.Identity;
using System.IdentityModel.Claims;
using TFG_UOC_2024.DB.Models;
using TFG_UOC_2024.CORE.Models.ApiModels;

namespace TFG_UOC_2024.CORE.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Contact.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Contact.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Contact.Email))
                .ForMember(dest => dest.FoodType, opt => opt.MapFrom(src => src.Contact.FoodType));
            CreateMap<ApplicationUser, UserSimpleDTO>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Contact.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Contact.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Contact.Email))
                .ForMember(dest => dest.FoodType, opt => opt.MapFrom(src => src.Contact.FoodType));
            CreateMap<UserDTO, ApplicationUser>();
            CreateMap<UserInput, ApplicationUser>()
                .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => new Contact() { FirstName = src.FirstName, LastName = src.LastName, Email = src.Email, PhoneNumber = src.PhoneNumber, FoodType = src.FoodType}));

            CreateMap<UserSimpleDTO, UserDTO>();

            CreateMap<ApplicationUserRole, UserRoleDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
                .ForMember(dest => dest.RoleNormalizedName, opt => opt.MapFrom(src => src.Role.NormalizedName));

            CreateMap<ApplicationRole, RoleDTO>();
            CreateMap<RoleDTO, ApplicationRole>();
            CreateMap<Claim, ClaimDTO>();

            CreateMap<Contact, ContactSimpleDTO>();
            CreateMap<Contact, ContactDTO>();

            CreateMap<RecipeFavorite, UserFavorite>();
            CreateMap<Recipe,RecipeDTO>();
            CreateMap<RecipeDTO, Recipe>().ReverseMap();
            CreateMap<Category, CategoryDTO>();

            CreateMap<Ingredient, IngredientDTO>();
            CreateMap<Category, IngredientCategoryDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<RecipeApi, RecipeDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.label))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.image))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.url));

            CreateMap<IngredientApi, IngredientDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.text))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.image))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => (double)src.quantity))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.foodCategory));

            CreateMap<IngredientDTO, Ingredient>();
            CreateMap<Menu, MenuDTO>();
            CreateMap<MenuDTO, Menu>();

            // make sure all datetime values are UTC
            ValueTransformers.Add<DateTime>(val => !((DateTime?)val).HasValue ? val : DateTime.SpecifyKind(val, DateTimeKind.Utc));
            ValueTransformers.Add<DateTime?>(val => val.HasValue ? DateTime.SpecifyKind(val.Value, DateTimeKind.Utc) : val);
        }
    }
}
