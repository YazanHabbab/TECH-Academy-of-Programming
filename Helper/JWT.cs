using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH_Academy_of_Programming.Helper
{
    public class JWT
    {
        public string Key { get; set;}
        public string Issuer { get; set;}
        public string Audience { get; set;}
        public double DurationInDays { get; set;}
    }
}