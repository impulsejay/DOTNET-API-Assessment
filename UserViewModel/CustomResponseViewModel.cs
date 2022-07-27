using DOTNET_API_Assessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_API_Assessment.UserViewModel
{
    public class CustomResponseViewModel
    {
        public int statusCode { set; get; }
        public string message { set; get; }
        public List<string> students { get; set; }

        public List<string> recipients { get; set; }
    }
}
