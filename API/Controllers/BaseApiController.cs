using Microsoft.AspNetCore.Mvc;

namespace API;

[ServiceFilter(typeof(LogUserActivity))] // là một attribute trong ASP.NET Core được sử dụng để áp dụng một Action Filter (trong trường hợp này là LogUserActivity) vào một controller hoặc action cụ thể.
[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    
}