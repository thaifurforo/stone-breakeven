using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingService.Domain.Model;
using AccountingService.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountingService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        
        private readonly ReadModelContext _context;

        // GET: api/Account
        [HttpGet]
        public IEnumerable<string> GetAccount()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(long id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // POST: api/Account
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccountItem(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }

        // PUT: api/Account/5
        [HttpPut("{id}")]
        public void PutAccount(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        public void DeleteAccount(int id)
        {
        }
    }
}
