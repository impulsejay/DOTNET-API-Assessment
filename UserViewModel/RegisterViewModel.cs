using DOTNET_API_Assessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_API_Assessment.UserViewModel
{
    public class RegisterViewModel
    {
        public string teacher { set; get; }
        public List<string> students { set; get; }
    }
}
