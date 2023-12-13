using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Tournament_Scheduling.Models
{
    [Table("Team")]
    public class Team
    {
        public int id { get; set; }
        public int EventID { get; set; }
        public string Name { get; set; }
        [Key]
        public string keyparams { get; set; }
    }
}