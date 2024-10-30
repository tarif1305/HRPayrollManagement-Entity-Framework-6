using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace HRPayrollManagement.Models
{
    public class Payroll
    {
        [Key]
        public int PayrollId { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [Range(10000, 2000000)]
        public decimal Salary { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deductions { get; set; }

        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}