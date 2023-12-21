using Backend.DLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Backend.Entities.Enums;

namespace Backend.BLL
{
    public class Candids
    {
        Context context;
        public Candids()
        {
            context = new Context();
        }
        public int UpdateCandid(string bio, long nationalID, int location)
        {
            try
            {
                var candid = context.Candids.SingleOrDefault(v => v.NationalID == nationalID);
                candid.Bio = bio;
              
                if (location != -1)
                    candid.Location = location;
              
                context.SaveChanges();
                return Convert.ToInt32(ResponseType.Success);
            }
            catch (Exception)
            {
                return Convert.ToInt32(ResponseType.DatabaseError);
            }
        }
        public List<CandidResponse> FilterCandids(long id)
        {
            var list = context.Candids.Where(v => v.NationalID ==  id).ToList();
            var stringifiedList = new List<CandidResponse>();
            foreach (var i in list)
            {
                CandidResponse candidResponse = new CandidResponse();
                candidResponse.ID = i.ID;
                candidResponse.Age = i.Age;
                candidResponse.Bio = i.Bio;
                candidResponse.Email = i.Email;
                candidResponse.Gender = i.Gender == 0 ? "Male" : "Female";
                candidResponse.Name = i.Name;
                candidResponse.Image = i.Image;
                candidResponse.Nationality = "Jordanian";
                candidResponse.Location = Enum.GetName(typeof(Locations), i.Location);
                candidResponse.NationalID = i.NationalID;
                candidResponse.Voters = i.Voters;
                stringifiedList.Add(candidResponse);
            }
            return stringifiedList;

        }
        public CandidResponse GetCandid(long candidID)
        {

          var i =  context.Candids.SingleOrDefault(v => v.ID == candidID);
            CandidResponse candidResponse = new CandidResponse();
            candidResponse.ID = i.ID;
            candidResponse.Age = i.Age;
            candidResponse.Bio = i.Bio;
            candidResponse.Email = i.Email;
            candidResponse.Gender = i.Gender == 0 ? "Male":"Female";
            candidResponse.Name = i.Name;
            candidResponse.Image = i.Image;
            candidResponse.Nationality = "Jordanian";
            candidResponse.Location = Enum.GetName(typeof(Locations), i.Location);
            candidResponse.NationalID = i.NationalID;
            candidResponse.Voters = i.Voters;
            return candidResponse;
        }
            public List<CandidResponse> GetCandids()
        {
            var list =  context.Candids.Where(v => v.ID > 0).ToList();
            var stringifiedList = new List<CandidResponse>();
            foreach(var i in list)
            {
                CandidResponse candidResponse = new CandidResponse();
                candidResponse.ID = i.ID;
                candidResponse.Age = i.Age;
                candidResponse.Bio = i.Bio;
                candidResponse.Email = i.Email;
                candidResponse.Gender = i.Gender == 0 ? "Male" : "Female";
                candidResponse.Name = i.Name;
                candidResponse.Image = i.Image;
                candidResponse.Nationality = "Jordanian";
                candidResponse.Location = Enum.GetName(typeof(Locations),i.Location);
                candidResponse.NationalID = i.NationalID;
                candidResponse.Voters = i.Voters;
                stringifiedList.Add(candidResponse);
            }
            return stringifiedList;

        }
   
        public int DeleteCandid(long nationalID)
        {
            try
            {

                var candid = context.Candids.SingleOrDefault(v => v.NationalID == nationalID);
                int candidID = candid.ID;
                List<Voter> voters = context.Voters.Where(v => v.CandidID == candidID).ToList();
                foreach(var voter in voters)
                {
                    voter.CandidID = 0;
                    voter.IsVoted = 0;
                }
                context.Candids.Remove(candid);       
                context.SaveChanges();
                return Convert.ToInt32(ResponseType.Success);
            }
            catch (Exception)
            {
                return Convert.ToInt32(ResponseType.DatabaseError);
            }
        }
       
       
        public int AddCandid(long id, string email, int location, string bio, string name, int age, int gender)
        {
            try
            {
                var x = context.Candids.SingleOrDefault(Y => Y.Email == email || Y.NationalID==id);
                if (x != null)
                {
                    return Convert.ToInt32(ResponseType.AlreadyFound);
                }
                else
                {
                    Candid candid = new Candid();
                    candid.Age = age;
                    candid.Bio = bio;
                    candid.Email = email;
                    candid.NationalID = id;
                    candid.Nationality = 0;
                    candid.Voters = 0;
                    candid.Location = location;
                    candid.Name = name;
                    candid.Image = "/assets/icon/1.png";
                    candid.Gender = gender;

                    context.Candids.Add(candid);
                    context.SaveChanges();
                    return Convert.ToInt32(ResponseType.Success);
                }
            }
            catch (Exception ex)
            {
                return Convert.ToInt32(ResponseType.DatabaseError);
            }
        }
    }
    public class CandidResponse
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string Image { set; get; }
        public string Bio { set; get; }
        public string Location { set; get; }
        public long NationalID { set; get; }
        public string Nationality { set; get; }
        public string Gender { set; get; }

        public int Age { set; get; }
        public int Voters { set; get; }
    }

}
