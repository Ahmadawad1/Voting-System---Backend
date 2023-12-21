using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DLL
{
    public class Log
    {
        [Key]
        public int ID { set; get; }
        public long VoterID { set; get; }
        public int CandidID { set; get; }
        public DateTime Time { set; get; }
        public string Location { set; get; }
        public string Action { set; get; }
    }
}
