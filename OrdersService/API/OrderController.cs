using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.DTOs;
using Application.Commands;
using Application.Queries;

namespace API {
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(IMediator m) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateOrderRequest r){ var id=await m.Send(new CreateOrderCommand(r.UserId,r.Amount,r.Description)); return CreatedAtAction(nameof(GetById),new{id},id);}        

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetByUser([FromQuery]Guid userId){ var l=await m.Send(new GetOrdersByUserQuery(userId)); return Ok(l);}        

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(Guid id){ var d=await m.Send(new GetOrderByIdQuery(id)); if(d is null) return NotFound(); return Ok(d);}    }
}