using Application;
using Application.Commands;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountsController(IMediator mediator) => _mediator = mediator;
        
        [HttpPost]
        [ProducesResponseType(typeof(CreateAccountResponse), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var accountId = await _mediator.Send(new CreateAccountCommand(request.UserId));
                var response = new CreateAccountResponse { AccountId = accountId };
                return CreatedAtAction(nameof(GetBalance),
                                       new { userId = request.UserId },
                                       response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        
        [HttpGet("balance/{userId:guid}")]
        [ProducesResponseType(typeof(GetBalanceResponse), 200)]
        public async Task<IActionResult> GetBalance([FromRoute] Guid userId)
        {
            var balance = await _mediator.Send(new GetBalanceQuery(userId));
            return Ok(new GetBalanceResponse { Balance = balance });
        }
        
        [HttpPost("deposit")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(NotFoundObjectResult), 404)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<IActionResult> Deposit([FromBody] DepositMoneyRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _mediator.Send(new DepositMoneyCommand(request.AccountId, request.Amount));
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { error = "Account not found" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        
        [HttpPost("withdraw")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(NotFoundObjectResult), 404)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawMoneyRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _mediator.Send(new WithdrawMoneyCommand(request.AccountId, request.Amount));
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { error = "Account not found" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}