using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordVault.Data;
using PasswordVault.Models;

namespace PasswordVault.WebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private IDatabase _dbContext = null;

        public PasswordController(IDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: password/uuid
        [HttpGet("{uuid}", Name = "Get")]
        public IActionResult Get(string uuid)
        {
            var userUuid = User.Identity.Name;

            if (userUuid != uuid)
            {
                return BadRequest();
            }

            List<Password> passwords = new List<Password>();

            foreach (var item in _dbContext.GetUserPasswordsByGUID(uuid))
            {
                Password password = new Password(
                    item.UniqueID,
                    item.Application,
                    item.Username,
                    item.Email,
                    item.Description,
                    item.Website,
                    item.Passphrase 
                );

                passwords.Add(password);
            }

            return Ok(passwords);
        }

        // POST: password/add
        [HttpPost("{uuid}/add")]
        public IActionResult Post(string uuid, [FromBody]DatabasePassword addDatabasePassword)
        {
            var userUuid = User.Identity.Name;

            if (userUuid != uuid)
            {
                return BadRequest();
            }

            if (addDatabasePassword == null)
            {
                return BadRequest();
            }

            Int64 uniqueID = _dbContext.AddPassword(addDatabasePassword);

            return Ok(uniqueID);
        }

        // POST: password/modify
        [HttpPost("{uuid}/modify")]
        public IActionResult Post(string uuid, [FromBody]List<DatabasePassword> passwords)
        {
            var userUuid = User.Identity.Name;

            if (userUuid != uuid)
            {
                return BadRequest();
            }

            if (passwords.Count != 2 || passwords == null || passwords[0] == null || passwords[1] == null)
            {
                return BadRequest();
            }

            bool result = _dbContext.ModifyPassword(passwords[0], passwords[1]);

            return Ok();
        }

        // DELETE: password/uuid/uniqueid
        [HttpDelete("{uuid}/{uniqueid}")]
        public IActionResult Delete(string uuid, Int64 uniqueid)
        {
            var userUuid = User.Identity.Name;

            if (userUuid != uuid)
            {
                return BadRequest();
            }

            bool result = _dbContext.DeletePassword(new DatabasePassword(uniqueid, uuid, null, null, null, null, null, null));

            return Ok();
        }
    }
}
