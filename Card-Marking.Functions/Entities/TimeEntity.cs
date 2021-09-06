using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Card_Marking.Functions.Entities
{
    public class TimeEntity : TableEntity
    {
        public int IdEmployee { get; set; }
        public int MinutesWorked { get; set; }
        public DateTime DateWorked { get; set; }
    }
}
