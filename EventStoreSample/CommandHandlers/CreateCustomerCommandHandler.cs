using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using EventStoreSample.Commands;
using EventStoreSample.Dto;
using EventStoreSample.Events;
using EventStoreSample.MongoDbConfigs;
using MediatR;

namespace EventStoreSample.CommandHandlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        readonly IMediator _mediator;
        private readonly IMongoDb _mongoDb;
        public CreateCustomerCommandHandler(
            IMediator mediator,
            IMongoDb mongoDb)
        {
            _mediator = mediator;
            _mongoDb = mongoDb;
        }

        public async Task<CustomerDto> Handle(CreateCustomerCommand createCustomerCommand, CancellationToken cancellationToken)
        {
            Domain.Customer customer = new Customer()
            {
                Id = Guid.NewGuid(),
                FirstName = createCustomerCommand.FirstName,
                LastName = createCustomerCommand.LastName,
                CreatedOn = DateTime.Now

            };

            await _mongoDb.InsertOneAsync(customer);

            // Raising Event ...
            await _mediator.Publish(new CustomerCreatedEvent(customer.FirstName, customer.LastName, customer.CreatedOn), cancellationToken);

            return new CustomerDto()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                CreatedOn = customer.CreatedOn.ToLongDateString()
            };
        }
    }
}
