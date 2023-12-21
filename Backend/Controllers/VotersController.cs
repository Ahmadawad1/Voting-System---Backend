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
    public class VotersController : ControllerBase
    {
        Voters voters;
        public VotersController()
        {
            voters = new Voters();
        }
        [HttpGet]
        public string AllVoters()
        {
            return JsonConvert.SerializeObject(voters.GetVoters());
        }
        [HttpPost]
        public int UpdateVoter(int status,long nationalID,int location,string password)
        {
            return voters.UpdateVoter(status,nationalID,location,password);
        }
        [HttpPost]
        public string GetVoterInfo(int id)
        {
            return JsonConvert.SerializeObject(voters.GetVoterInfo(id));
        }
        [HttpPost]
        public int DeleteVoter(long nationalID)
        {
            return voters.DeleteVoter(nationalID);
        }
        [HttpPost]
        public int AddVoter(long id,string email, int location,string password,string name,int age,int gender)
        {
            return voters.AddVoter(id,email,location,password,name,age,gender);
        }

        [HttpPost]
        public int Vote(long nationalID,int candidID)
        {
            return voters.Vote(nationalID,candidID);
        }
        [HttpPost]
        public int Unvote(long nationalID)
        {
            return voters.Unvote(nationalID);
        }
        [HttpPost]
        public int GetVoterStatus(long nationalID)
        {
            return voters.GetVotingStatus(nationalID);
        }
        [HttpPost]
        public string GetVoterLocation(long nationalID)
        {
            return JsonConvert.SerializeObject(voters.GetVoterLocation(nationalID));
        }
        [HttpPost]
        public int GetVotedCandid(long nationalID)
        {
            return voters.GetVotedCandid(nationalID);
        }
        [HttpGet]
        public string Dashboard()
        {
            return JsonConvert.SerializeObject(voters.Dashboard());
        }
    }
}
