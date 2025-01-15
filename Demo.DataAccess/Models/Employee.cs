using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataAccess.Models
{
    public class Employee
    {
        public int Id { get; set; }  //PK

        [Required(ErrorMessage ="Name Is Required")]
        [MaxLength(50,ErrorMessage ="Max Length is 50 chars")]
        [MinLength(5,ErrorMessage ="Min Length is 5 chars")]
        public string Name { get; set; }

        [Range(22,25,ErrorMessage ="Age Must be In Range From 22 to 35")]
        public int? Age { get; set; }

        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage ="Address Must be Like 123-Street-City-Country")]
        public string Address {  get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
