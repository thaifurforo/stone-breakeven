using AccountManagementService.Api.Contracts.v1.Requests;
using AccountManagementService.Domain.Commands;
using AccountManagementService.Domain.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AccountManagementService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMediator _mediator;

   
        public AccountController(IAccountRepository accountRepository, IMediator mediator)
        {
            _accountRepository = accountRepository;
            _mediator = mediator;
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
        [HttpGet("{Id}")]
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

            try
            { 
                var result = await _mediator.Send(command);
                return new OkObjectResult(result) { StatusCode = StatusCodes.Status201Created };
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }

        }

        // POST: api/account/5/deactivate
        [HttpPost("{Id}/deactivate")]
        public async Task<IActionResult> DeactivateAccount([FromRoute] GetByAccountId request)
        {
            try
            {
                var obj = new DeactivateAccountCommand { Id = request.Id };
                var result = await _mediator.Send(obj);

                return new OkObjectResult(result) { StatusCode = StatusCodes.Status201Created };
            }
            catch (NullReferenceException ex)
            {
                return new NotFoundObjectResult(ex.Message);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
