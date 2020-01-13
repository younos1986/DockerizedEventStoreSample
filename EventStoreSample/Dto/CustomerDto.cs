using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventStoreSample.Dto
{
    public class CustomerDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CreatedOn { get; set; }
    }
}
