using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH_Academy_of_Programming.Models
{
    public class UpdateResult
    {
        public string? Message {get; set;}
        public bool IsUpdated {get; set;}
        public bool Succeeded {get; set;}
    }
}