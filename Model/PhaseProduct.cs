using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace specificoperationservice.Model
{
    public enum PhaseProductType
    {
        scrap,
        finished,
        semi_finished,
    }
    public class PhaseProduct
    {
        [Key]
        [JsonIgnore]
        public int internalId { get; set; }
        [Required]
        [MaxLength(50)]
        public string value { get; set; }
        [Required]
        [MaxLength(50)]
        public string measurementUnit { get; set; }
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public PhaseProductType phaseProductType { get; set; }
        public Product product { get; set; }
    }
}