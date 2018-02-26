using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace specificoperationservice.Model
{
    public class Tag
    {
        [Key]
        [JsonIgnore]
        public int internalId { get; set; }
        public int tagId { get; set; }
        public string tagName { get; set; }
        public string tagGroup{get;set;}
        public string tagDescription { get; set; }
        public int thingGroupId { get; set; }
        public string physicalTag { get; set; }
        public ThingGroup thingGroup{get;set;}
    }
}