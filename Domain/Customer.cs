using System;

namespace Domain
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
