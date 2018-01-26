using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace specificoperationservice.Model
{
    public class Product
    {
        [Key]
        [JsonIgnore]
        public int internalId { get; set; }
        public int productId { get; set; }
        [Required]
        [MaxLength(50)]
        public string productName { get; set; }
        [MaxLength(100)]
        public string productDescription { get; set; }
        [MaxLength(50)]
        public string productCode { get; set; }
        [MaxLength(50)]
        public string productGTIN { get; set; }
    }
}