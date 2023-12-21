using Backend.BLL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        Authentication authentication;
       public AuthController()
        {
            authentication = new Authentication();
        }
       [HttpGet]
       public string TestMethod()
        {
            return "Ahmad AWAD";
        }
        [HttpPost]
        public int AdminLogin(string email,string password)
        {
            return authentication.ValidateAdmin(email,password); 
           
        }
        [HttpPost]
        public int GetAdminType(string email)
        {
            return authentication.GetAdminType(email);
        }
        [HttpPost]
        public int VoterLogin(long id, string password)
        {
             return authentication.ValidateVoter(id, password);
           
        }

    }
}
