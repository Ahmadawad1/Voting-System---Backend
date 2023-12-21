
using Backend.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CandidsController : ControllerBase
    {
        Candids candids;
        public CandidsController()
        {
            candids = new Candids();
        }
        [HttpPost]
        public string FilterCandids(long id)
        {
            return JsonConvert.SerializeObject(candids.FilterCandids(id));
        }
        [HttpGet]
        public string AllCandids()
        {
            return JsonConvert.SerializeObject(candids.GetCandids());
        }
        [HttpPost]
        public string GetCandid(long candidId)
        {
            return JsonConvert.SerializeObject(candids.GetCandid(candidId));
        }
        [HttpPost]
        public int DeleteCandid(long nationalID)
        {
            return candids.DeleteCandid(nationalID);
        }
        [HttpPost]
        public int AddCandid(long id,string email,int location,string bio,string name,int age,int gender)
        {
            return candids.AddCandid(id,email,location,bio,name,age,gender);
        }
        [HttpPost]
        public int UpdateCandid(string bio,long nationalID,int location)
        {
            return candids.UpdateCandid(bio,nationalID,location);
        }
    }
}
