using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class Enums
    {
        public enum ResponseType
        {
            NotFoundEmail,
            IncorrectPassord,
            Success,
            NotFoundFingerPrint,
            DatabaseError,
            UnableToVote,
            AlreadyFound
        }
        public enum AdminType
        {
            Government,
            IEC
        }
        public enum VoterStatus
        {
            Eligible,
            Forbidden
        }
        public enum CandidStatus
        {
            Active,
            Suspended
        }
        public enum Nationality
        {
            Jordan,
            Palestain,
            KSA,
            Iraq,
            Syria,
            Turkey,
            USA,
            Qatar,
            UAE,
            Egypt
        }
        public enum Locations
        {
            Amman,
            Zarqa,
            Irbid,
            Karak,
            Aqapa,
            Maan,
            Ajloun,
            Jarash,
            Madaba,
            Mafraq
        }
        public enum Gender
        {
            Male,
            Female
        }

    }
}
