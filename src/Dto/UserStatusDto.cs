using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.src.Dto
{
    public class UserStatusDto
    {
        [StringLength(255)]
        public string? Reason { get; set; }
        public bool Status { get; set; }
    }
}