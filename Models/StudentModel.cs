using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_API_Assessment.Models
{
    public class StudentModel
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public bool IsSuspended { get; set; }
    }
}
