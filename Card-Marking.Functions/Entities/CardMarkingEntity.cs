using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Card_Marking.Functions.Entities
{
    public class CardMarkingEntity : TableEntity
    {
        public int TypeMarking { get; set; }
        public DateTime DateMarking { get; set; }
        public int IdEmployee { get; set; }
        public bool IsConsolidated { get; set; }
    }

}
