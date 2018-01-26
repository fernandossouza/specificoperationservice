using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace specificoperationservice.Model
{
    public class ProductionOrder
    {
        public int productionOrderId { get; set; }
        [Required]
        public Recipe recipe { get; set; }
        [Required]
        [MaxLength(50)]
        public string productionOrderNumber { get; set; }
        [Required]
        public int? productionOrderTypeId { get; set; }
        [NotMapped]
        public string typeDescription { get; set; }
        public int? quantity { get; set; }
        public string currentStatus { get; set; }
        public int? currentThingId { get; set; }
        [NotMapped]
        public Thing currentThing { get; set; }

        
    }
}