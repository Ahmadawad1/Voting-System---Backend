using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DLL
{
    public class Candid
    {
        [Key]
        public int ID { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string Image { set; get; }
        public string Bio { set; get; }
        public int Location { set; get; }
        public long NationalID { set; get; }
        public int Nationality { set; get; }
        public int Gender { set; get; }

        public int Age { set; get; }
        public int Voters { set; get; }
        
    }
}
