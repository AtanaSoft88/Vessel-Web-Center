using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Services.Contracts
{
	public interface IManningCompanyService
	{
		Task<IEnumerable<ManningCompanyViewModel>> GetAllCompanies();
		Task<IEnumerable<ManningCompaniesVesselsViewModel>> GetVessels(int compId);
	}
}
