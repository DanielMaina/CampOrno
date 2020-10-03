using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CampOrno.Models
{
    public class CamperActivity
    {
        public CamperActivity()
        {
            //Defaults
            Fee = 5m;
            NumberOfSessions = 5;
        }
        public int ID { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName="decimal(9,2)")]//So only 5 bytes to store in SQL Server
        [Range(0,9999999.99,ErrorMessage ="Invalid Fee Amount")]
        public decimal Fee { get; set; }

        [Display(Name = "Number Of Sessions")]
        [Range(1,int.MaxValue, ErrorMessage = "Number Of Sessions invalid: must be a reasonable number greater then zero")]
        public int NumberOfSessions { get; set; }

        [Required(ErrorMessage ="You must select a camper")]
        [Display(Name = "Camper")]
        public int CamperID { get; set; }
        public Camper Camper { get; set; }

        [Required(ErrorMessage = "You must select the activity.")]
        [Display(Name = "Activity")]
        public int ActivityID { get; set; }
        public Activity Activity { get; set; }

    }
}
