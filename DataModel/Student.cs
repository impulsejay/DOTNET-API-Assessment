using System;
using System.Collections.Generic;

#nullable disable

namespace DOTNET_API_Assessment.DataModel
{
    public partial class Student
    {
        public Student()
        {
            Registrations = new HashSet<Registration>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string IsSuspended { get; set; }

        public virtual ICollection<Registration> Registrations { get; set; }
    }
}
