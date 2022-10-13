using Fake.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace Fake.Models.ViewModels
{
    public class MenuVM
    {
        public Guid ID { get; set; }
        public Guid DishId { get; set; }

        [PastDate]
        public DateTime DateColumn { get; set; }
        public List<Product>? Food { get; set; }
        public ICollection<ProductMenu>? ProMenu { get; set; }
        public List<Menu>? Menu { get; set; }
    }
}
