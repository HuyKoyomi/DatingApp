
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AdminController(UserManager<AppUser> userManager) : BaseApiController
{
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users-with-roles")]
    public async Task<ActionResult> GetUsersWithRoles()
    {
        var user = await userManager.Users
         .OrderBy(x => x.UserName)
         .Select(x => new
         {
             x.Id,
             Username = x.UserName,
             Roles = x.UserRoles.Select(r => r.Role.Name).ToList()
         }).ToListAsync();
        return Ok(user);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("edit-roles/{username}")]
    public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
    {
        if (string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role");

        // Find by username
        var user = await userManager.FindByNameAsync(username);
        if (user == null) return NotFound();

        // get new roles list
        var selectedRoles = roles.Split(",").ToArray(); // => string to array
        var userRoles = await userManager.GetRolesAsync(user); // get roles ddang có trong db

        // add new roles for user
        // selectedRoles.Except(userRoles): => Lọc ra các vai trò mà user chưa có, tránh bị thêm trùng.
        var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
        if (!result.Succeeded) return BadRequest("Failed to add to roles");

        // delete roles không còn được chọn
        result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
        if (!result.Succeeded) return BadRequest("Failed to remove from roles");

        return Ok(await userManager.GetRolesAsync(user));
    }

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpGet("photos-to-moderate")]
    public ActionResult GetPhotosForModeration()
    {
        return Ok("Only admin or moderation can see this");
    }
}
