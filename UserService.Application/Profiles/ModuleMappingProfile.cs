using AutoMapper;
using UserService.Domain.DTOs;
using UserService.Application.Features.Permissions.Commands;
using UserService.Application.Features.Permissions.Queries;
using UserService.Application.Features.Roles.Commands;
using UserService.Application.Features.Roles.Queries;
using UserService.Domain.Entities;
using UserService.Application.Features.Users.Queries;
using UserService.Application.Features.Users.Commands;

namespace UserService.Application.Profiles;

public class ModuleMappingProfile : Profile
{
    public ModuleMappingProfile()
    {
        // User
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, ListUserDto>().ReverseMap();
        CreateMap<User, CreateUserDto>().ReverseMap();
        CreateMap<User, UpdateUserDto>().ReverseMap();

        CreateMap<UserDto, GetUserDto>().ReverseMap();
        CreateMap<UserDto, CreateUserDto>().ReverseMap();
        CreateMap<UserDto, UpdateUserDto>().ReverseMap();


        // UserRole
        CreateMap<UserRole, UserRoleDto>().ReverseMap();
        CreateMap<UserRole, CreateUserRoleDto>().ReverseMap();
        CreateMap<UserRoleDto, CreateUserRoleDto>().ReverseMap();


        // UserPermission
        CreateMap<UserPermission, UserPermissionDto>().ReverseMap();
        CreateMap<UserPermission, CreateUserPermissionDto>().ReverseMap();
        CreateMap<UserPermissionDto, CreateUserPermissionDto>().ReverseMap();


        // Role
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Role, ListRoleDto>().ReverseMap();
        CreateMap<Role, CreateRoleDto>().ReverseMap();
        CreateMap<Role, UpdateRoleDto>().ReverseMap();
        CreateMap<Role, DeleteRoleDto>().ReverseMap();

        CreateMap<RoleDto, GetRoleDto>().ReverseMap();
        CreateMap<RoleDto, CreateRoleDto>().ReverseMap();
        CreateMap<RoleDto, UpdateRoleDto>().ReverseMap();


        // RolePermission
        CreateMap<RolePermission, RolePermissionDto>().ReverseMap();
        CreateMap<RolePermission, CreateRolePermissionDto>().ReverseMap();
        CreateMap<RolePermission, UpdateRolePermissionDto>().ReverseMap();

        CreateMap<RolePermissionDto, CreateRolePermissionDto>().ReverseMap();
        CreateMap<RolePermissionDto, UpdateRolePermissionDto>().ReverseMap();


        // Permission
        CreateMap<Permission, PermissionDto>().ReverseMap();
        CreateMap<Permission, ListPermissionDto>().ReverseMap();
        CreateMap<Permission, CreatePermissionDto>().ReverseMap();
        CreateMap<PermissionDto, CreatePermissionDto>().ReverseMap();
        CreateMap<Permission, DeletePermissionDto>().ReverseMap();
    }
}
