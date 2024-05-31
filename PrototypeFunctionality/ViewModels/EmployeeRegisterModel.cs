using System.ComponentModel.DataAnnotations;

namespace PrototypeFunctionality.ViewModels
{
    public class EmployeeRegisterModel
    {
        [Required(ErrorMessage = "Please, provide first name.")]
        [StringLength(20, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please, provide last name.")]
        [StringLength(20, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please, provide valid email address.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please, provide strong password.")]
        [StringLength(100, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password.")]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password did not match!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please, provide user type.")]
        public int UserTypeId { get; set; } // 1 = Employee, 2 = Farmer

        [Required(ErrorMessage = "Please, provide city.")]
        [StringLength(20, MinimumLength = 2)]
        public string City { get; set; }
    }
}