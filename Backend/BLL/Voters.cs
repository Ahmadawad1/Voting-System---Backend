using Backend.DLL;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Backend.Entities.Enums;

namespace Backend.BLL
{
    public class Voters
    {
        Context context;
        public Voters()
        {
            context = new Context();
        }
        public VotersResponse GetVoterInfo(int id) {
            var i = context.Voters.SingleOrDefault(v => v.ID ==id);
            VotersResponse candidResponse = new VotersResponse();
            candidResponse.ID = i.ID;
            candidResponse.Age = i.Age;
            candidResponse.Email = i.Email;
            candidResponse.Gender = i.Gender == 0 ? "Male" : "Female";
            candidResponse.Name = i.Name;
            candidResponse.Image = i.Image;
            candidResponse.Nationality = Enum.GetName(typeof(Nationality), i.Nationality);
            candidResponse.Location = Enum.GetName(typeof(Locations), i.Location);
            candidResponse.NationalID = i.NationalID;
            candidResponse.CandidID = i.CandidID;
            candidResponse.Password = i.Password;
            candidResponse.IsVoted = i.IsVoted == 1 ? "Voted" : "Not Yet";
            candidResponse.Status = i.Status == 0 ? "Eligible" : "Forbidden";
            return candidResponse;
        }
        public List<VotersResponse> GetVoters()
        {
            var list = context.Voters.Where(v => v.ID > 0).ToList();
            var stringifiedList = new List<VotersResponse>();
            foreach (var i in list)
            {
                VotersResponse candidResponse = new VotersResponse();
                candidResponse.ID = i.ID;
                candidResponse.Age = i.Age;           
                candidResponse.Email = i.Email;
                candidResponse.Gender = i.Gender == 0 ? "Male" : "Female";
                candidResponse.Name = i.Name;
                candidResponse.Image = i.Image;
                candidResponse.Nationality = Enum.GetName(typeof(Nationality), i.Nationality);
                candidResponse.Location = Enum.GetName(typeof(Locations), i.Location);
                candidResponse.NationalID = i.NationalID;
                candidResponse.CandidID = i.CandidID;
                candidResponse.Password = i.Password;
                candidResponse.IsVoted = i.IsVoted == 1 ? "Voted" : "Not Yet";
                candidResponse.Status = i.Status == 0 ? "Eligible" : "Forbidden";
                candidResponse.Reason = GetReason(i);
                stringifiedList.Add(candidResponse);
            }
            return stringifiedList;
        }
        public int UpdateVoter(int status, long nationalID, int location, string password)
        {
            try
            {
                var voter = context.Voters.SingleOrDefault(v => v.NationalID == nationalID);
               if(status == 1)
                Unvote(nationalID);
               
                if(location != -1)
                voter.Location = location;
                if(status != -1)
                voter.Status = status;
                context.SaveChanges();
                return Convert.ToInt32(ResponseType.Success);
            }
            catch (Exception)
            {
                return Convert.ToInt32(ResponseType.DatabaseError);
            }
        }
        private string GetReason(Voter voter)
        {
            if (voter.Status == 0) return "";
            if (voter.Age < 18) return "Voter is out of Age";
            else if (voter.IsVoted == 1) return "Already Voted";
            else if (voter.Nationality != 0) return "Not Jordaninan";
            else return "";
        }
        public int ChangeStatus(int status,int nationalID)
        {
            try
            {
                var voter = context.Voters.SingleOrDefault(v => v.NationalID == nationalID);
                Unvote(nationalID);
                voter.Status = status;              
                context.SaveChanges();
                return Convert.ToInt32(ResponseType.Success);
            }
            catch (Exception)
            {
                return Convert.ToInt32(ResponseType.DatabaseError);
            }
        }
        public DashboardData Dashboard()
        {
            try
            {
                List<VotingResults> votingResults = new List<VotingResults>();
                DashboardData dashboardData = new DashboardData();
                List<VotersResponse> allVoters = GetVoters();
                int numberOfVoters = allVoters.Count();
                double numberOfVotes = allVoters.Where(x => x.IsVoted == "Voted").Count();
                double unvoted = allVoters.Where(x => x.IsVoted =="Not Yet").Count();
                double percentageOfVoting = (numberOfVotes / numberOfVoters) * 100;
                dashboardData.VotingPercentage = percentageOfVoting;
                dashboardData.Voted = numberOfVotes;
                dashboardData.Unvoted = unvoted;
                dashboardData.TotalVoters = numberOfVoters;
                dashboardData.AllowedVoters = allVoters.Where(X => X.Status == "Eligible").ToList().Count();
                dashboardData.NotAllowedVoters = allVoters.Where(X => X.Status == "Forbidden").ToList().Count();
                for (int i = 0; i <= 9; i++)
                {
                    votingResults.Add(GetWinner(i));
                }
                dashboardData.VotingResults = votingResults;
                return dashboardData;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private VotingResults GetWinner(int location)
        {
            var cityCandids = context.Candids.Where(c => c.Location == location).ToList();
            if (cityCandids.Count() > 0)
            {
                var sortedList = cityCandids.OrderByDescending(i => i.Voters);
                if (sortedList.Count() > 1)
                {
                  var  firstTwo = sortedList.Take(2).ToList();
                    if(firstTwo[0].Voters == firstTwo[1].Voters)
                    {
                        return new VotingResults { City = Enum.GetName(typeof(Locations), location), Winner = null };
                    }
                    else
                    {
                        return new VotingResults { City = Enum.GetName(typeof(Locations), location), Winner = firstTwo[0] };
                    }
                }
                else
                {
                    if (sortedList.ToList()[0].Voters != 0)
                    {
                        return new VotingResults { City = Enum.GetName(typeof(Locations), location), Winner = sortedList.ToList()[0] };
                    }
                    else
                    {
                        return new VotingResults { City = Enum.GetName(typeof(Locations), location), Winner = null };
                    }
                }
              
            }
            else
            {
                return new VotingResults { City = Enum.GetName(typeof(Locations), location), Winner = null };
            }
        }
        
        public int DeleteVoter(long nationalID)
        {
            try
            {
                
                var voter = context.Voters.SingleOrDefault(v => v.NationalID == nationalID);
                int candidID = voter.CandidID;
                context.Voters.Remove(voter);
               if(candidID >0)
                context.Candids.SingleOrDefault(c => c.ID == candidID).Voters--;
                context.SaveChanges();
                return Convert.ToInt32(ResponseType.Success);
            }
            catch (Exception)
            {
                return Convert.ToInt32(ResponseType.DatabaseError);
            }
        }
       
        public int AddVoter(long id, string email, int location, string password, string name, int age, int gender)
        {
            try
            {
                var x = context.Voters.SingleOrDefault(x => x.NationalID == id);
                if (x != null)
                {
                    return Convert.ToInt32(ResponseType.AlreadyFound);
                }
                else
                {
                    Voter voter = new Voter();
                    voter.Age = age;
                    voter.CandidID = 0;
                    voter.Email = email;
                    voter.Gender = gender;
                    voter.Image = "/assets/icon/1.png";
                    voter.IsVoted = 0;
                    voter.Location = location;
                    voter.NationalID = id;
                    voter.Nationality = 0;
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
                    voter.Password = passwordHash;
                    voter.Status = age > 18 ? 0 : 1;
                    voter.Name = name;


                    context.Voters.Add(voter);
                    context.SaveChanges();
                    return Convert.ToInt32(ResponseType.Success);
                }
            }
            catch (Exception ex)
            {
                return Convert.ToInt32(ResponseType.DatabaseError);
            }
        }
        public int GetVotingStatus(long nationalID)
        {
            

                var voter = context.Voters.SingleOrDefault(v => v.NationalID == nationalID);

                return voter.IsVoted;



        }
        public int GetVotedCandid(long nationalID)
        {
            var voter = context.Voters.SingleOrDefault(v => v.NationalID == nationalID);
           return voter.CandidID;
        }
        public string GetVoterLocation(long nationalID)
        {
            var voter = context.Voters.SingleOrDefault(v => v.NationalID == nationalID);
            return Enum.GetName(typeof(Locations), voter.Location);
        }
        public int Unvote(long nationalID)
        {
            try
            {

                var voter = context.Voters.SingleOrDefault(v => v.NationalID == nationalID);
                var candidID = voter.CandidID;
                voter.IsVoted = 0;
                voter.CandidID = 0;
                context.Candids.SingleOrDefault(c => c.ID == candidID).Voters--;
                context.SaveChanges();
                Logger.Log(Enum.GetName(typeof(Locations), voter.Location), voter.ID, candidID, "Delete Vote");
                return Convert.ToInt32(ResponseType.Success);
            }
            catch (Exception)
            {
                return Convert.ToInt32(ResponseType.DatabaseError);
            }
        }
        public int Vote(long nationalID, int candidID)
        {
            try
            {
                List<string> cities = new List<string> { "Amman", "Zarqa", "Irbid", "Karak", "Aqapa", "Maan", "Ajloun", "Jarash", "Madaba", "Mafraq"};
                var threads = new List<Thread>();
                //foreach (var city in cities)
                //{
                //    Thread newThread = new Thread(new ParameterizedThreadStart(ProcessVotes));
                //    newThread.Name = city;
                //    threads.Add(newThread);            
                //}
                var voter = context.Voters.SingleOrDefault(v => v.NationalID == nationalID);
                var candid = context.Candids.SingleOrDefault(c => c.ID == candidID);
                string voterCity = Enum.GetName(typeof(Locations), voter.Location);
             //   var thread = threads.FirstOrDefault(x => x.Name == voterCity);
                VotingData votingData = new VotingData { CandidID = candidID, NationalID = nationalID, Voter = voter };
                VoterStatus votingStatus = CheckStatus(voter, candid);
                if (votingStatus == VoterStatus.Eligible)
                {
                   ProcessVotes(votingData);
                    return Convert.ToInt32(ResponseType.Success);
                }
                else
                {
                    return Convert.ToInt32(ResponseType.UnableToVote);
                }


            }
            catch (Exception ex)
            {
                return Convert.ToInt32(ResponseType.DatabaseError);
            }
        }
        private void ProcessVotes(VotingData data)
        {
         
            int candidID = data.CandidID;
            long nationalID = data.NationalID;
            long voterID = data.NationalID;
            var candid = context.Candids.SingleOrDefault(c => c.ID == candidID);
            var voter = context.Voters.SingleOrDefault(v => v.NationalID == nationalID);           
            VoterStatus votingStatus = CheckStatus(data.Voter, candid);
            if (votingStatus == VoterStatus.Eligible)
            {
                candid.Voters++;
                voter.CandidID = candid.ID;
                voter.IsVoted = 1;
                Logger.Log(Enum.GetName(typeof(Locations),voter.Location),voterID,candidID,"New Vote");
                context.SaveChanges();              
            }

           

        }
        private VoterStatus CheckStatus(Voter voter, Candid candid)
        {
            if (voter.Age < 18) return VoterStatus.Forbidden;
            else if (voter.Location != candid.Location) return VoterStatus.Forbidden;
            else if (voter.Nationality != Convert.ToInt16(Nationality.Jordan)) return VoterStatus.Forbidden;
            else if (voter.IsVoted ==1) return VoterStatus.Forbidden;
            else return VoterStatus.Eligible;
        }
    
        

        }
    public class VotersResponse
    {
      
           
            public int ID { set; get; }
            public string Name { set; get; }
            public string Email { set; get; }
            public long NationalID { set; get; }
            public string Password { set; get; }
            public string Image { set; get; }
            public string Reason { set; get; }

            public string Location { set; get; }
            public string Nationality { set; get; }
            public string Gender { set; get; }
            public string Status { set; get; }
            public int Age { set; get; }
            public string IsVoted { set; get; }
            public int CandidID { set; get; }

        
    }
    public class VotingData { 
    public int CandidID { set; get; }
    public long NationalID { set; get; }
        public Voter Voter { set; get; }
    }
    public class DashboardData
    {
        public double VotingPercentage { set; get; }
        public double Voted { set; get; }
        public double Unvoted { set; get; }
        public int TotalVoters { set; get; }
        public int AllowedVoters { set; get; }
        public int NotAllowedVoters { set; get; }


        public List<VotingResults> VotingResults { set; get; }
    }
    public class VotingResults
    {
        public string City { set; get; }
        public Candid Winner { set; get; }

    }
  
}
