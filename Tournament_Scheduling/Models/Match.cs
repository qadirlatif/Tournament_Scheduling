using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tournament_Scheduling.Models
{
    [Table("Match")]
    public class Match
    {
        public int id { get; set; }
        public int EventId { get; set; }
        public int TeamA_id { get; set; }
        public int TeamB_id { get; set; }
        public int TeamA_score { get; set; }
        public int TeamB_score { get; set; }
        public DateTime MatchDate { get; set; }
        public string Venue { get; set; }
        [Key]
        public string keyparams { get; set; }
    }
}