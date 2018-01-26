using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace specificoperationservice.Model
{
    public class ThingGroup
    {
        [Key]
        [JsonIgnore]
        public int internalId { get; set; }
        public int thingGroupId { get; set; }
        public string groupName { get; set; }
        public string groupCode { get; set; }
        public ICollection<int> thingsIds{get;set;}
        public ICollection<Tag> tags { get; set; }
    }
}