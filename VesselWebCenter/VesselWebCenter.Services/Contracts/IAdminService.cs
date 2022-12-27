using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Services.Contracts
{
	public interface IAdminService
	{
		Task<InformationViewModel> ShowGeneralInfo();
	}
}
