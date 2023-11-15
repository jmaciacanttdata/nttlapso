using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
namespace NTTLapso.Models.DataDump
{ 
    public class Schedule
    {
        public string id_employee { get; set; }
        public DateTime date {  get; set; }
        public float hours { get; set; }

    }
}
