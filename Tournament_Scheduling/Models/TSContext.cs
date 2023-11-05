using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Tournament_Scheduling.Models
{
    public class TSContext : DbContext
    {
        public DbSet<Team> teams { get; set; }
        public DbSet<Match> matches { get; set; }   
        public DbSet<Event> Events { get; set; }
    }
}