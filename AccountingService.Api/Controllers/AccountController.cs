using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Api.Contracts.v1.Response;
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
        public ActionResult<List<Account>> GetAllAccounts()
        {
            if (AccountList.IsEmptyTodoItems())
            {
                return NoContent();
            }

            return Ok(AccountList.Accounts);
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
        public IActionResult CreateToDo([FromBody] CreateAccountRequest request)
        {
            try
            {
                request.Validate();

                var todo = new Account(request.Document, request.Agency);
                AccountList.Accounts.Add(todo);

                return Ok(todo);
            }
            catch(BadHttpRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
