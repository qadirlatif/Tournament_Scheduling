using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tournament_Scheduling.Models
{
    [Table("PointsTable")]
    public class PointsTable : BaseEntity
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public int EventID { get; set; }
        public int Matches { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public int Points { get; set; }
        public string Keyparams { get; set; }
    }
}