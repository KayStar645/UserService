using AutoMapper;
using UserService.Application.DTOs;
using UserService.Application.Features.Permissions.Commands;
using UserService.Application.Features.Permissions.Queries;
using UserService.Application.Features.Roles.Commands;
using UserService.Application.Features.Roles.Queries;
using UserService.Domain.Entities;

namespace UserService.Application.Profiles;

public class ModuleMappingProfile : Profile
{
    public ModuleMappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();

        CreateMap<UserRole, UserRoleDto>().ReverseMap();
        CreateMap<UserRole, CreateUserRoleDto>().ReverseMap();
        CreateMap<UserRoleDto, CreateUserRoleDto>().ReverseMap();

        CreateMap<UserPermission, UserPermissionDto>().ReverseMap();
        CreateMap<UserPermission, CreateUserPermissionDto>().ReverseMap();
        CreateMap<UserPermissionDto, CreateUserPermissionDto>().ReverseMap();

        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Role, ListRoleDto>().ReverseMap();
        CreateMap<Role, CreateRoleDto>().ReverseMap();
        CreateMap<Role, DeleteRoleDto>().ReverseMap();

        CreateMap<RoleDto, GetRoleDto>().ReverseMap();
        CreateMap<RoleDto, CreateRoleDto>().ReverseMap();

        CreateMap<RolePermission, RolePermissionDto>().ReverseMap();
        CreateMap<RolePermission, CreateRolePermissionDto>().ReverseMap();
        CreateMap<RolePermissionDto, CreateRolePermissionDto>().ReverseMap();

        CreateMap<Permission, PermissionDto>().ReverseMap();
        CreateMap<Permission, ListPermissionDto>().ReverseMap();
        CreateMap<Permission, CreatePermissionDto>().ReverseMap();
        CreateMap<PermissionDto, CreatePermissionDto>().ReverseMap();
        CreateMap<Permission, DeletePermissionDto>().ReverseMap();
    }
}
