using System;
using System.Collections.Generic;
using System.Text;

namespace DO

{
    /// <summary>
    /// Represents a User in the system
    /// </summary>
    public class User
    {
        public string UserName { get; set; }
        public Permission Admin { get; set; }
        public string password { get; set; }
        public bool Available { get; set; } = true;
    }
}
