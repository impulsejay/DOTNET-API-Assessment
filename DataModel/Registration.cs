using System;
using System.Collections.Generic;

#nullable disable

namespace DOTNET_API_Assessment.DataModel
{
    public partial class Registration
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
