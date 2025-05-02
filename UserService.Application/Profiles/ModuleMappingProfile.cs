using AutoMapper;
using Sieve.Models;
using UserService.Application.DTOs;
using UserService.Application.Features.Permissions.Commands;
using UserService.Application.Features.Permissions.Queries;
using UserService.Domain.Entities;

namespace UserService.Application.Profiles;

public class ModuleMappingProfile : Profile
{
    public ModuleMappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();

        CreateMap<UserRole, UserRoleDto>().ReverseMap();

        CreateMap<Role, RoleDto>().ReverseMap();

        CreateMap<RolePermission, RolePermissionDto>().ReverseMap();

        CreateMap<SieveModel, ListPermissionDto>().ReverseMap();
        CreateMap<Permission, PermissionDto>().ReverseMap();
        CreateMap<Permission, ListPermissionDto>().ReverseMap();
        CreateMap<Permission, CreatePermissionDto>().ReverseMap();
        CreateMap<PermissionDto, CreatePermissionDto>().ReverseMap();
        CreateMap<Permission, DeletePermissionDto>().ReverseMap();
    }
}
