using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fake.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }

        DateTime InputDate { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
