using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_Poc.Model;
using WebApi_Poc.Services;

namespace WebApi_Poc.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.GetTokenAndUserInfo(userParam.Username, userParam.Password);
            if(user==null)
            {
                return BadRequest(new { message="Invalid Credential."});
            }
            return Ok(user);

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]User userParam)
        {
            if(userParam.Id!=0)
            {
                var user = _userService.SaveUser(userParam);
                return Ok();
            }
            return BadRequest(new { message = "Invalid Information." });

        }

        [HttpPost("login")]
        public IActionResult Login([FromBody]User userParam)
        {
            return Ok("You Are Authorized");
        }
    }
}