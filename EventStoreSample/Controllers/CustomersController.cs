using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStoreSample.Commands;
using EventStoreSample.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventStoreSample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand createCustomerCommand)
        {
            CustomerDto customer = await _mediator.Send(createCustomerCommand);
            return Ok(customer);
        }
    }
}
