using System;
using System.Collections.Generic;
using System.Text;

namespace Card_Marking.Common.Models
{
    public class CardMarking
    {
        public int TypeMarking { get; set; }
        public DateTime DateMarking { get; set; }
        public int IdEmployee { get; set; }
        public bool IsConsolidated { get; set; }



    }
}
