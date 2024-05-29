using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
	public class ApplicationUser : IdentityUser
	{
        public string Fname { get; set; }
        public string Lname { get; set; }
        public bool IsAgree { get; set; }
    }
}
