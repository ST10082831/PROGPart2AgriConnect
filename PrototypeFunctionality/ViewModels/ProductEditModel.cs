using System.ComponentModel.DataAnnotations;

namespace PrototypeFunctionality.ViewModels
{
    public class ProductEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, provide name.")]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please, provide product category.")]
        public int ProductCategoryId { get; set; }

        [Required(ErrorMessage = "Please, select production date.")]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }

        [Required(ErrorMessage = "Please, provide details.")]
        [StringLength(500, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        public string Details { get; set; }

        public string? ApplicationUserId { get; set; }
    }
}