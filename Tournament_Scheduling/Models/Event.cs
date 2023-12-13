using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tournament_Scheduling.Models
{
    [Table("Event")]
    public class Event : BaseEntity
    {
        
        public int Eventid{ get; set; }
        public string name { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string keyparams { get; set; }
    }
}