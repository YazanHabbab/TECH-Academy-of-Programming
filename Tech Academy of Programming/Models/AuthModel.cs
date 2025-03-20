using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH_Academy_of_Programming.Models
{
    public class AuthModel
    {
        public string? Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiresOn { get; set; }
    }
}