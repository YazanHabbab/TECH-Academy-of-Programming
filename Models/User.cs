using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TECH_Academy_of_Programming.Models
{
    public class User : IdentityUser
    {
        [Required]
        public override string UserName { get; set; }

        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        [Required]
        public override string Email { get; set; }

        [Required]
        public DateTime created_at { get; set; }

        [Required]
        public DateTime updated_at { get; set; }

    }
}
