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
            if (AccountList.IsEmptyAccounts())
            {
                return NoContent();
            }

            return Ok(AccountList.Accounts);
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var result = AccountList.Accounts.Find(account => account.Id == id);

            if (result is null)
            {
                return NotFound(new { message = "The given Id does not exist in the Accounts List." });
            }

            return Ok(result);
        }

        // POST: api/Account
        [HttpPost]
        public IActionResult CreateAccount([FromBody] CreateAccountRequest request)
        {
            try
            {
                request.Validate();

                var account = new Account(request.Document, request.Agency);
                AccountList.Accounts.Add(account);

                return Ok(account);
            }
            catch(BadHttpRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Account/5/deactivate
        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> PutAccount(int id)
        {
            try
            {
                var account = AccountList.Accounts.Find(account => account.Id == id);
                
                account.Number = account.Number;
                account.Agency = account.Agency;
                account.Amount = account.Amount;
                account.Status = false;
                account.OpeningDate = account.OpeningDate;
                account.ClosingDate = DateOnly.FromDateTime(DateTime.Now);
                account.Document = account.Document;
                
                return Ok(account);
        
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        public void DeleteAccount(int id)
        {
        }
    }
}
