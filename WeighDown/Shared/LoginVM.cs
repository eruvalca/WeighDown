using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeighDown.Shared
{
    public class LoginVM
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(25, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
