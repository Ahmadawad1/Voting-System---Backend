using Backend.DLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.BLL
{
    public class Logger
    {
     
        public static void Log(string location, long voterID, int candidID, string type)
        {
            try
            {
                var votingProcess = new Log
                {
                    CandidID = candidID,
                    VoterID = voterID,
                    Location = location,
                    Action = type,
                    Time = DateTime.Now
                };
                Context context = new Context();
                context.Logs.Add(votingProcess);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
