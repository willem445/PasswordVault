using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PasswordVault.WebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService = null;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: password/modify
        [AllowAnonymous]
        [HttpPost]
        [HttpPost("authenticate")]
        public IActionResult Post([FromBody]AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            return Ok(user);
        }

        [HttpGet]
        public Int64 Get()
        {
            return 10;
        }
    }
}