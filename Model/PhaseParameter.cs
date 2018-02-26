using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace specificoperationservice.Model
{
    public class PhaseParameter
    {
        [Key]
        [JsonIgnore]
        public int internalId { get; set; }
        [Required]
        [MaxLength(50)]
        public string setupValue { get; set; }
        public string setupValueMin { get; set; }
        public string setupValueMax { get; set; }
        [Required]
        [MaxLength(50)]
        public string measurementUnit { get; set; }
        [MaxLength(50)]
        public string minValue { get; set; }
        [MaxLength(50)]
        public string maxValue { get; set; }
        public Tag tag { get; set; }
    }
}