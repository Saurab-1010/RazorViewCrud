using Microsoft.AspNetCore.Mvc.Rendering;

namespace RazorViewCrude.ViewModel
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            CategoryList = new List<SelectListItem>();
        }

        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int DisplayOrder { get; set; }
    
        public int CategoryId { get; set; }

        public List<SelectListItem> CategoryList { get; set; }
        public string? CategoryName { get; set; }
    }
}
