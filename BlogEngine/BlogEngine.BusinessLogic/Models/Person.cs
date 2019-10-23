using System;
using System.Collections.Generic;

namespace BlogEngine.BusinessLogic.Models
{
    public partial class Person
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual Role Role { get; set; }
    }
}
