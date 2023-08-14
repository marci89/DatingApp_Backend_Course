using DatingAppService.API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppService.API.Controllers
{
	[ServiceFilter(typeof(LogUserActivity))]
	[ApiController]
	[Route("api/[controller]")]
	public class BaseApiController : ControllerBase
	{
	}
}
