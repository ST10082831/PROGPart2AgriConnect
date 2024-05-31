using System.ComponentModel.DataAnnotations;

namespace PrototypeFunctionality.Models
{
    public class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Please, provide name.")]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please, provide application user id.")]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}