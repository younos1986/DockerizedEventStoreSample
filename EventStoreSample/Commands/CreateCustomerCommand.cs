using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStoreSample.Dto;
using MediatR;

namespace EventStoreSample.Commands
{
    public class CreateCustomerCommand : IRequest<CustomerDto>
    {
        public CreateCustomerCommand(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; }

        public string LastName { get; }
    }
}
