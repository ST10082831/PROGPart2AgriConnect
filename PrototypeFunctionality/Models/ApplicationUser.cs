using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PrototypeFunctionality.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Products = new HashSet<Product>();
            ProductCategories = new HashSet<ProductCategory>();
        }

        [Required(ErrorMessage = "Please, provide first name.")]
        [StringLength(20, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please, provide last name.")]
        [StringLength(20, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please, provide user type.")]
        public int UserTypeId { get; set; } // 1 = Employee, 2 = Farmer

        [Required(ErrorMessage = "Please, provide city.")]
        [StringLength(20, MinimumLength = 2)]
        public string City { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}