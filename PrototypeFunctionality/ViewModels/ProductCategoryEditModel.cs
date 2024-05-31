using System.ComponentModel.DataAnnotations;

namespace PrototypeFunctionality.ViewModels
{
    public class ProductCategoryEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, provide name.")]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please, provide application user id.")]
        public string ApplicationUserId { get; set; }
    }
}