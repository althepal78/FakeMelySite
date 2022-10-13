using Fake.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace Fake.Models
{
    
    public class Menu
    {
        [Key]
        public Guid ID { get; set; }


        [PastDate]
        public DateTime DateColumn { get; set; }

        public ICollection<ProductMenu>? ProductMenus { get; set; }

    }
}
