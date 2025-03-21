using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TECH_Academy_of_Programming.Models
{
    public class UserRegisterModel
    {
        [Required, MinLength(3)]
        public string firstName { get; set; }

        [Required, MinLength(3)]
        public string lastName { get; set; }


        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), MinLength(4)]
        public string Password { get; set; }
    }
}