using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.EmployeeManagementModel
{
    public class AdminUserVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; } 
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please select a role.")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must contain 8 characters, an uppercase letter, a lowercase letter, a digit, and a special character.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Mobile number is required.")]
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "Please select a gender.")]
        public int GenderId { get; set; }
        [Required(ErrorMessage = "Please select a country.")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Please select a province.")]
        public int ProvinceId { get; set; }
        [Required(ErrorMessage = "Please select a district.")]
        public int DistrictId { get; set; }
        [Required(ErrorMessage = "Please select a muncipality.")]
        public int MunicipalityId { get; set; }
        public DateTime? DOBAD { get; set; }
        public bool IsActive { get; set; }
    }

    public class AdminUserUpdateVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string MobileNumber { get; set; }
        public int GenderId { get; set; }
        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int MunicipalityId { get; set; }
        public DateTime DOBAD { get; set; }
        public bool IsActive { get; set; }
    }

}
