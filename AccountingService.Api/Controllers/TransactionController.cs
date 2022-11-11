using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using AccountingService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace TransactioningService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMediator _mediator;

   
        public TransactionController(ITransactionRepository transactionRepository, IMediator mediator)
        {
            this._transactionRepository = transactionRepository;
            this._mediator = mediator;
        }

        // GET: api/transaction
        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> GetAllTransactions()
        {
            var transactions = await _transactionRepository.GetAllTransactions();
            if (transactions.IsNullOrEmpty())
            {
                return NoContent();
            }

            return Ok(transactions);
        }

        // GET: api/transaction/id/5
        [HttpGet("id/{id}")]
        public async Task<ActionResult<Transaction>> GetTransactionById(Guid id)
        {
            var transaction = await _transactionRepository.GetTransactionById(id);

            if (transaction is null)
            {
                return NotFound(new { message = "The given Id does not exist in the Transactions Database" });
            }

            return Ok(transaction);
        }
        
        // GET: api/transaction/account/5
        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<Transaction>> GetTransactionsByAccount(int accountId)
        {
            var transaction = await _transactionRepository.GetTransactionsByAccount(accountId);

            if (transaction is null)
            {
                return NotFound(new { message = "The given Account does not have any transactions registered" });
            }

            return Ok(transaction);
        }
        
        // GET: api/transaction/credit_account/5
        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<Transaction>> GetCreditTransactionsByAccount(int accountId)
        {
            var transaction = await _transactionRepository.GetCreditTransactionsByAccount(accountId);

            if (transaction is null)
            {
                return NotFound(new { message = "The given Account does not have any transactions registered" });
            }

            return Ok(transaction);
        }
        
        // GET: api/transaction/dedit_account/5
        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<Transaction>> GetDebitTransactionsByAccount(int accountId)
        {
            var transaction = await _transactionRepository.GetDebitTransactionsByAccount(accountId);

            if (transaction is null)
            {
                return NotFound(new { message = "The given Account does not have any transactions registered" });
            }

            return Ok(transaction);
        }

        // POST: api/transaction
        [HttpPost]
        public async Task<IActionResult> Transaction([FromBody] CreateTransactionCommand command, CreateAccountCommand command2)
        {

            var result = await _mediator.Send(command);

            return Ok(result);

        }
        
    }
}
