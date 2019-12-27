using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace EventStoreSample.Events
{
    public class CustomerCreatedEvent : INotification
    {
        public CustomerCreatedEvent(string firstName, string lastName, DateTime createdOn)
        {
            FirstName = firstName;
            LastName = lastName;
            CreatedOn = createdOn;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public DateTime CreatedOn { get; }
    }
}
