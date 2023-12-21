using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DLL
{
    public class Context : DbContext
    {
        public Context()
        {
        }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
       public virtual DbSet<Admin> Admins { set; get; }
       public virtual DbSet<Candid> Candids { set; get; }
       public virtual DbSet<Log> Logs { set; get; }
       public virtual DbSet<Voter> Voters { set; get; }
       public virtual DbSet<Location> Locations { set; get; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ElectionSystem;Integrated Security=SSPI;");

            }
        }
    }
}
