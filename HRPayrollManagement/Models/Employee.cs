using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRPayrollManagement.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]

        public HttpPostedFileBase Upload { get; set; }

        [Display(Name = "Image"), ScaffoldColumn(false)]
        public string ImagePath { get; set; }
        [NotMapped]
        public string Operation { get; set; } = "";





        public virtual List<Payroll> Payrolls { get; set; } = new List<Payroll>();
    }
}