using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_API_Assessment.Models
{
    public class RegistrationModel
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }

        public int StudentId { get; set; }
    }
}
