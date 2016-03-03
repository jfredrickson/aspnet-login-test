using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginTest.ViewModels
{
    public class RoleAssignment
    {
        public int RoleID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}