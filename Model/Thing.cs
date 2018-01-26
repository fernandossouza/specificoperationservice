using System.ComponentModel.DataAnnotations.Schema;

namespace specificoperationservice.Model
{
    public class Thing
    {
        
        public int thingId { get; set; }
        public string thingName { get; set; }
        public string thingCode { get; set; }
        public int? currentThingId { get; set; }
        [NotMapped]
        public Thing currentThing { get; set; }
        public string phisycalConn{get;set;}
    }
}