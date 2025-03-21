using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TECH_Academy_of_Programming.Models
{
    public class UpdatedUserModel
    {
        [MinLength(3)]
        public string? firstName { get; set; }

        [MinLength(3)]
        public string? lastName { get; set; }


        [EmailAddress]
        public string? Email { get; set; }

        [DataType(DataType.Password), MinLength(4)]
        public string? oldPassword { get; set; }

        [DataType(DataType.Password), MinLength(4)]
        public string? newPassword { get; set; }
    }
}