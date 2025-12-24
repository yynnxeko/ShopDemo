using AutoMapper;
using ShopDemo.Application.DTOs.RoleDtos;
using ShopDemo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.Mappings
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile() {
            // Map từ Role sang RoleUpdatedDto
            CreateMap<Role, RoleUpdatedDto>();
            CreateMap<Role, RoleCreatedDto>();
            CreateMap<Role, RoleDto>();
            // Map ngược lại từ RoleUpdatedDto sang Role
            CreateMap<RoleDto, Role>();

            CreateMap<RoleUpdatedDto, Role>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Users, opt => opt.Ignore());

            CreateMap<RoleCreatedDto, Role>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Users, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        }
    }
}
