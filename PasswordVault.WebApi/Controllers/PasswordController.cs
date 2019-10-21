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
        public IEnumerable<Password> Get(string uuid)
        {
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

            return passwords;
        }

        // POST: password/add
        [HttpPost("add")]
        public Int64 Post([FromBody]DatabasePassword addDatabasePassword)
        {
            Int64 uniqueID = _dbContext.AddPassword(addDatabasePassword);

            return uniqueID;
        }

        // POST: password/modify
        [HttpPost("modify")]
        public IActionResult Post([FromBody]List<DatabasePassword> passwords)
        {
            if (passwords.Count != 2 || passwords == null || passwords[0] == null || passwords[1] == null)
            {
                return BadRequest();
            }

            _dbContext.ModifyPassword(passwords[0], passwords[1]);

            return Ok();
        }

        // DELETE: password/uuid/uniqueid
        [HttpDelete("{useruuid}/{uniqueid}")]
        public void Delete(string useruuid, Int64 uniqueid)
        {
            _dbContext.DeletePassword(new DatabasePassword(uniqueid, useruuid, null, null, null, null, null, null));
        }
    }
}
