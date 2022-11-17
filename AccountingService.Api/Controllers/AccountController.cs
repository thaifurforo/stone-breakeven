using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AccountingService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMediator _mediator;

   
        public AccountController(IAccountRepository accountRepository, IMediator mediator)
        {
            this._accountRepository = accountRepository;
            this._mediator = mediator;
        }

        // GET: api/account
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountRepository.GetAllAccounts();
            if (accounts.IsNullOrEmpty())
            {
                return NoContent();
            }

            return Ok(accounts);
        }

        // GET: api/account/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount([FromRoute] GetByAccountId request)
        {
            var account = await _accountRepository.GetAccountById(request.Id);

            if (account is null)
            {
                return NotFound(new { message = "The given Id does not exist in the Accounts Database" });
            }

            return Ok(account);
        }

        // POST: api/account
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command)
        {

            var result = await _mediator.Send(command);

            return Ok(result);

        }

        // POST: api/account/5/deactivate
        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> DeactivateAccount([FromRoute] GetByAccountId request)
        {
                var obj = new DeactivateAccountCommand { Id = request.Id };
                var result = await _mediator.Send(obj);

                return Ok(result);
        }
    }
}
