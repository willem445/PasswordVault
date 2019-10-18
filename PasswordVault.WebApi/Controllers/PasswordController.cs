using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordVault.Data.Standard;
using PasswordVault.Models.Standard;

namespace PasswordVault.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        // GET: api/Password/5
        [HttpGet("{uuid}", Name = "Get")]
        public IEnumerable<Password> Get(string uuid)
        {
            IDatabase _dbcontext = new SQLiteDatabase();

            List<Password> passwords = new List<Password>();

            foreach (var item in _dbcontext.GetUserPasswordsByGUID(uuid))
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

        // POST: api/Password
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Password/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
