using Backend.DLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Backend.Entities.Enums;

namespace Backend.BLL
{
    public class Authentication
    {
        Context context;
        public Authentication()
        {
            context = new Context();
        }

        public int ValidateAdmin(string email,string password)
        {
            try
            {
                var admin = context.Admins.SingleOrDefault(u => u.Email == email);
                if (admin == null) return Convert.ToInt32(ResponseType.NotFoundEmail);
                else if (admin.Password != password) return Convert.ToInt32(ResponseType.IncorrectPassord);
                else return Convert.ToInt32(ResponseType.Success);
            }
            catch (Exception)
            {
                return Convert.ToInt32(ResponseType.DatabaseError);
            }
        }
        public int GetAdminType(string email)
        {
          return  context.Admins.SingleOrDefault(u => u.Email == email).Type;
        }
       
        public int ValidateVoter(long id,string password)
        {
            try
            {
                var voter = context.Voters.SingleOrDefault(u => u.NationalID == id);
                if (voter == null) return Convert.ToInt32(ResponseType.NotFoundEmail);
                else if (!BCrypt.Net.BCrypt.Verify(password, voter.Password)) return Convert.ToInt32(ResponseType.IncorrectPassord);
                else return Convert.ToInt32(ResponseType.Success);
            }
            catch (Exception)
            {
                return Convert.ToInt32(ResponseType.DatabaseError);
            }
        }
    }
}
