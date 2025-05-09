using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.src.Helpers.RequestHelpers
{
    public class UserParams : PaginationParams
    {
        public bool? Status { get; set; }
        public DateTime? RegistrationDateFrom { get; set; }
        public DateTime? RegistrationDateTo { get; set; }
        public string? SearchTerm { get; set; }
    }
}