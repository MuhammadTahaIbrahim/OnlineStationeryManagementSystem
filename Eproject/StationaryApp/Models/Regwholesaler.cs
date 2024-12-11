using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StationaryApp.Models
{
    public partial class Regwholesaler
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(10, MinimumLength = 8, ErrorMessage = "Username must be between 8 and 10 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username must contain only letters and numbers.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 16 characters.")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[\W_]).*$", ErrorMessage = "Password must contain at least one number and one special character.")]
        public string? UserPass { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email should be in a valid format.")]
        [RegularExpression(@"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email should be like this abc123@gmail.com")]
        public string? UserEmail { get; set; }


        public string? UserStatus { get; set; }
    }
}