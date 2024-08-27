using System.ComponentModel.DataAnnotations;

namespace EmployeeImportApp.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string? FirstName { get; set; } 

        [Required]
        [StringLength(100)]
        public string? LastName { get; set; } 

        [Required]
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; } 

        [StringLength(100)]
        public string? Mobile { get; set; } 

        [StringLength(100)]
        public string? PayrollNumber { get; set; }

        public string? DateOfBirth { get; set; } 

        [StringLength(200)]
        public string? Address { get; set; } 

        [StringLength(200)]
        public string? Address2 { get; set; }

        [StringLength(20)]
        public string? Postcode { get; set; } 

        public string? StartDate { get; set; }

        public bool IsEditing { get; set; } = false;
    }
}
