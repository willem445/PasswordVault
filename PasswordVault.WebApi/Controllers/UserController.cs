using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordVault.Models;
using PasswordVault.Data;

namespace PasswordVault.WebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        public UserController()
        {

        }

        // AUTHENTICATE (LOGIN)
        // POST: user/authenticate
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Post([FromBody]AuthenticateModel model)
        {
            string uuid = null;

            //var user = _userService.Authenticate(model.Username, model.Password);

            return Ok(uuid);
        }

        // VERIFY PASSWORD
        [HttpPost("{uuid}")]
        public IActionResult Post(string uuid, [FromBody]string password)
        {
            bool result = false;

            //var user = _userService.Authenticate(model.Username, model.Password);

            return Ok(result);
        }

        // GET USER
        [HttpGet("{uuid}", Name = "Get")]
        public IActionResult Get(string uuid)
        {
            var userUuid = User.Identity.Name;

            if (userUuid != uuid)
            {
                return BadRequest();
            }

            return Ok();
        }

        // ADD USER
        // POST: user/add
        [AllowAnonymous]
        [HttpPost("add")]
        public IActionResult Post([FromBody]User addUser)
        {
            bool result = false;

            if (addUser == null)
            {
                return BadRequest();
            }

            //bool result = _userService.Add(addUser);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        // MODIFY USER
        // POST: user/modify
        [HttpPost("modify")]
        public IActionResult Post(string uuid, [FromBody]List<User> users) // PASS UNIQUE UUID OF USER AND NEW USER OBJECT INSTEAD OF 2 USER OBJECTS
        {
            var userUuid = User.Identity.Name;

            if (userUuid != uuid)
            {
                return BadRequest();
            }

            //if (users.Count != 2 || users == null || users[0] == null || users[1] == null)
            //{
            //    return BadRequest();
            //}

            //bool result = _userService.Modify(users[0], users[1]);

            return Ok();
        }

        // DELETE USER
        // DELETE: password/uuid/uniqueid
        [HttpDelete("{uuid}/{uniqueid}")]
        public IActionResult Delete(string uuid, Int64 uniqueid)
        {
            var userUuid = User.Identity.Name;

            if (userUuid != uuid)
            {
                return BadRequest();
            }

            //bool result = _userService.Delete(uuid);

            return Ok();
        }
    }
}