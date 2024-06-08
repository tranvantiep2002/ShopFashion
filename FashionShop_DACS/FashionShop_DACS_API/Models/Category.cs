using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FashionShop_DACS.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [JsonIgnore]
        public List<Product>? Products { get; set; }

    }
}
