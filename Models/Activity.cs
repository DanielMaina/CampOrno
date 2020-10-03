using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CampOrno.Models
{
    public class Activity
    {
        public int ID { get; set; }

        [Display(Name = "Activity")]
        [Required(ErrorMessage = "You cannot leave the name of the activity blank.")]
        [StringLength(50, ErrorMessage = "Activity name cannot be more than 50 characters long.")]
        public string Name { get; set; }

        [Display(Name = "Campers")]
        public ICollection<CamperActivity> CamperActivities { get; set; }
    }
}
