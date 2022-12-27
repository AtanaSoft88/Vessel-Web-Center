using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesselWebCenter.Services.ViewModels
{
	public class InformationViewModel
	{
		public int TotalVessels { get; set; }
        public int VesselsReadyForVoyageCount { get; set; }
        public int TotalCrewMembersHired { get; set; }		
		public int TotalFreeCrewMembers { get; set; }		

	}
}
