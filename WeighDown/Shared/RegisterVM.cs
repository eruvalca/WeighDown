using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeighDown.Shared
{
    public class RegisterVM
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(25, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(25, MinimumLength = 8)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
    }
}
