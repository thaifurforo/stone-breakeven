using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using AccountingService.Repository;
using Credit.NetCore.Framework.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace AccountingService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        
        private readonly IDbContext _context;

        public AccountController(IDbContext context)
        {
            _context = context;
        }

        public AccountController()
        {
            
        }

        // GET: api/Account
        [HttpGet]
        public ActionResult<List<Account>> GetAllAccounts()
        {
            if (_context.Accounts.IsNullOrEmpty())
            {
                return NoContent();
            }

            return Ok(_context.Accounts);
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(int id)
        {
            var account = _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound(new { message = "The given Id does not exist in the Accounts List." });
            }

            return Ok(account);
        }

        // POST: api/Account
        [HttpPost]
        public IActionResult CreateAccount([FromBody] CreateAccountRequest request)
        {
            try
            {
                request.Validate();
                
                var account = new Account(request.Document, request.Agency);
                _context.Accounts.Add(account);

                _context.SaveChanges(true);

                return Ok(account);
            }
            catch(BadHttpRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Account/5/deactivate
        [HttpPost("{id}/deactivate")]
        public IActionResult DeactivateAccount([FromRoute] int id)
        {
            try
            {
                
                var account = _context.Accounts.Find(id);
                account.DeactivateAccount();

                return Ok(account);
        
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
