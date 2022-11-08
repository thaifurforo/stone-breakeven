using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using Credit.NetCore.Framework.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace AccountingService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // GET: api/Account
        [HttpGet]
        public ActionResult<List<Account>> GetAllAccounts()
        {
            var accounts = _accountRepository.GetAllAccounts();
            if (accounts.IsNullOrEmpty())
            {
                return NoContent();
            }

            return Ok(accounts);
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(int id)
        {
            var account = _accountRepository.GetAccountById(id);

            if (account is null)
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
                _accountRepository.AddAccount(account);

                _accountRepository.Save();

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

                var account = _accountRepository.DeactivateAccount(id);
                _accountRepository.Save();

                return Ok(account);
        
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
