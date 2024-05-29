using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Employee :BaseEntity
    {
        [Range(22, 45)]
        public int? Age { get; set; }
        public decimal Salary { get; set; }
        
        public string Address { get; set; }
        public  bool  IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
        public string  ImageName { get; set; }
        public int? DepartmentId { get; set; } //FK
        public Department Department { get; set; }

    }
}
