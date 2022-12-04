using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselWebCenter.Data.Constants;

namespace VesselWebCenter.Services.ViewModels
{
	public class CrewAllViewModel
	{
        public int? VesselId { get; set; }
        public string FirstName { get; set; } = null!;       
        public string LastName { get; set; } = null!;        
        public int Age { get; set; }        
        public string Nationality { get; set; } = null!;
        public bool HiredToVessel { get; set; }
    }
}
