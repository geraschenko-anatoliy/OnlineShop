using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }
        [Required(ErrorMessage = "Please enter a category name")]
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
