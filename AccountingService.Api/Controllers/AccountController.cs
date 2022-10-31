using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Api.Contracts.v1.Response;
using AccountingService.Domain.Model;
using AccountingService.Repository;
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
        public async Task<ActionResult<List<Account>>> GetAllAccounts()
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
            var account = AccountList.Accounts.Find(account => account.Id == id);

            if (account is null)
            {
                return NotFound(new { message = "The given Id does not exist in the Accounts List." });
            }

            return Ok(account);
        }

        // POST: api/Account
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
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

        // POST: api/Account/5/deactivate
        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> PutAccount(int id)
        {
            try
            {
                var account = AccountList.Accounts.Find(account => account.Id == id);
                
                account.Status = false;
                account.ClosingDate = DateOnly.FromDateTime(DateTime.Now);
                
                return Ok(account);
        
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
