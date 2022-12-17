using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.Text.Json;
using api.Helpers;

namespace api.DTOs
{
    public class RegisterDto
    {
        [Required] public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }
        [Required] public string KnownAs { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public DateOnly? DateOfBirth { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Country { get; set; }
    }



}
