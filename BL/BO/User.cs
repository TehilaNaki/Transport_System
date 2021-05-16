using System;
using System.Collections.Generic;
using System.Text;

namespace BO
{
    /// <summary>
    /// Represents a User in the system
    /// </summary>
    public class User
        {
            public string UserName { get; set; }
            public Permission Admin { get; set; }
            public string password { get; set; }
        }
}
