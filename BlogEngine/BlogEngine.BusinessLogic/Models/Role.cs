using System;
using System.Collections.Generic;

namespace BlogEngine.BusinessLogic.Models
{
    public partial class Role
    {
        public Role()
        {
            Person = new HashSet<Person>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual ICollection<Person> Person { get; set; }
    }
}
