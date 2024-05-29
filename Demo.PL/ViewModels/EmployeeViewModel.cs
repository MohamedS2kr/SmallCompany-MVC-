using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System;
using System.Globalization;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        [Range(22, 45)]
        public int? Age { get; set; }
        public decimal Salary { get; set; }

        public string Address { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
        [DisplayName("Data Of Creation")]
        public DateTime DataOfCreation { get; set; }
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        public int? DepartmentId { get; set; } //FK
        public Department Department { get; set; }
    }
}
