using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tournament_Scheduling.Models;

namespace Tournament_Scheduling.View_Model
{
    public class MatchesViewModel
    {
        public int matchid { get; set; }
        public int EventId { get; set; }
        public string TeamA_Name { get; set; }
        public string TeamB_Name { get; set; }
        public int TeamA_score { get; set; }
        public int TeamB_score { get; set; }
        public DateTime MatchDate { get; set; }
        public string Venue { get; set; }
    }
    public class Venue
    {
        public string Name { get; set; }   
    }
}