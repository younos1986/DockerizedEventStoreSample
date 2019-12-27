using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStoreSample.Commands;
using FluentValidation;

namespace EventStoreSample.CommandValidators
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(customer => customer.FirstName).NotEmpty();
            RuleFor(customer => customer.LastName).NotEmpty();
        }
    }
}
