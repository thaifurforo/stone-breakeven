using AccountingService.Api.Contracts.v1.Requests;
using AccountingService.Domain.Commands;
using AccountingService.Domain.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AccountingService.Api.Controllers;

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
    public async Task<IActionResult> GetAllTransactions()
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
    public async Task<IActionResult> GetTransactionById([FromRoute] GetByTransactionId request)
    {
        var transactionId = Guid.Parse(request.Id);
        var transaction = await _transactionRepository.GetTransactionById(transactionId);

        if (transaction is null)
        {
            return NotFound(new { message = "The given Id does not exist in the Transactions Database" });
        }

        return Ok(transaction);
    }
        
    // GET: api/transaction/account/5
    [HttpGet("account/{Id}")]
    public async Task<IActionResult> GetTransactionsByAccount([FromRoute] GetByAccountId request)
    {
        var transactionsByAccount = await _transactionRepository.GetTransactionsByAccount(request.Id);

        if (transactionsByAccount.IsNullOrEmpty())
        {
            return NotFound(new { message = "The given Account does not have any transactions registered" });
        }

        return Ok(transactionsByAccount);
    }
        
    // GET: api/transaction/credit_account/5
    [HttpGet("credit_account/{Id}")]
    public async Task<IActionResult> GetCreditTransactionsByAccount([FromRoute] GetByAccountId request)
    {
        var creditTransactionsByAccount = await _transactionRepository.GetCreditTransactionsByAccount(request.Id);

        if (creditTransactionsByAccount.IsNullOrEmpty())
        {
            return NotFound(new { message = "The given Account does not have any credit transactions registered" });
        }

        return Ok(creditTransactionsByAccount);
    }
        
    // GET: api/transaction/debit_account/5
    [HttpGet("debit_account/{Id}")]
    public async Task<IActionResult> GetDebitTransactionsByAccount([FromRoute] GetByAccountId request)
    {
        var debitTransactionsByAccount = await _transactionRepository.GetDebitTransactionsByAccount(request.Id);

        if (debitTransactionsByAccount.IsNullOrEmpty())
        {
            return NotFound(new { message = "The given Account does not have any debit transactions registered" });
        }

        return Ok(debitTransactionsByAccount);
    }

    // POST: api/transaction
    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
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
}