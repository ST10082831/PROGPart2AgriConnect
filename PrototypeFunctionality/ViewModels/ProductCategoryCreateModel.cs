using System.ComponentModel.DataAnnotations;

namespace PrototypeFunctionality.ViewModels
{
    public class ProductCategoryCreateModel
    {
        [Required(ErrorMessage = "Please, provide name.")]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }

        public string? ApplicationUserId { get; set; }
    }
}