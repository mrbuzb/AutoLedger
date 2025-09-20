using AutoLedger.Application.Dtos;
using AutoLedger.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutoLedger.Api.Endpoints;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/admin")
                   .WithTags("AdminManagement");

        userGroup.MapGet("/get-all", [Authorize(Roles = "Admin, SuperAdmin")]
        async (IRoleService _roleService) =>
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Results.Ok(roles);
        })
        .WithName("GetAllRoles");

        userGroup.MapGet("/get-all-users-by-role", [Authorize(Roles = "Admin, SuperAdmin")]
        [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any, NoStore = false)]
        async (string role, IRoleService _roleService) =>
        {
            var users = await _roleService.GetAllUsersByRoleAsync(role);
            return Results.Ok(new { success = true, data = users });
        })
            .WithName("GetAllUsersByRole");


        userGroup.MapDelete("/delete-user-by-id", [Authorize(Roles = "Admin, SuperAdmin")]
        async (long userId, HttpContext httpContext, IUserService userService) =>
        {
            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            await userService.DeleteUserByIdAsync(userId, role);
            return Results.Ok();
        })
        .WithName("DeleteUser");

        userGroup.MapPatch("/update-role", [Authorize(Roles = "SuperAdmin")]
        async (long userId, string userRole, IUserService userService) =>
        {
            await userService.UpdateUserRoleAsync(userId, userRole);
            return Results.Ok();
        })
        .WithName("UpdateUserRole");
        userGroup.MapPost("/create", [Authorize(Roles = "Admin, SuperAdmin")]
        async (string name, IExpenseCategoryService _cotegory) =>
            {
                var cotegory = await _cotegory.AddExpenseCategoryAsync(name);
                return Results.Ok(cotegory);
            })
            .WithName("CreatExpenseCategory");
        userGroup.MapDelete("/delete", [Authorize(Roles = "Admin, SuperAdmin")]
        async (long id, IExpenseCategoryService _cotegory) =>
            {
                await _cotegory.DeleteExpenseCategoryAsync(id);
                return Results.Ok();
            })
        .WithName("deleteExpenseCategory");
        userGroup.MapPut("/update", [Authorize(Roles = "Admin, SuperAdmin")]
        async (ExpenseCategoryResponseDto dto, IExpenseCategoryService _cotegory) =>
           {
               await _cotegory.UpdateExpenseCategoryAsync(dto);
               return Results.Ok();
           })
           .WithName("updateExpenseCategory");
    }
}
