using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DLL
{
    public class Location
    {
        [Key]
        public int ID { set; get; }
        public string State { set; get; }
        public string Country { set; get; }
    }
}
