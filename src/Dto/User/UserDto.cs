using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bogus.DataSets;

namespace ECommerce.src.Dto.User
{
    public class UserDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? Commune { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }

        public DateTime RegistrationDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool Status { get; set; }

    }
}