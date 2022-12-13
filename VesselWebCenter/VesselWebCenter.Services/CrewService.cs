using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;
using VesselWebCenter.Data.Models;
using VesselWebCenter.Data.Repositories;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Services
{
    public class CrewService : ICrewService
    {
        private readonly IRepository repo;

        public CrewService(IRepository _repo)
        {
            this.repo = _repo;
        }
        public async Task<IQueryable<CrewAllViewModel>> GetAll()
        {
            var crewAll = repo.AllReadonly<CrewMember>().Select(x => new CrewAllViewModel()
            {
                CrewMemberId = x.Id,
                VesselName = x.Vessel.Name,
                ManningCompanyName = x.Vessel.ManningCompany.Name,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Age = x.Age,
                Nationality = x.Nationality,
                HiredToVessel = x.IsPartOfACrew,
                DateHired = x.IsPartOfACrew == true ? x.DateHired.Value.Date.ToString("dd-MM-yyyy") : "N/A",
            }).AsQueryable();
            return crewAll;
        }

        public async Task<bool> AddCrewMemberToDataBase(CrewMemberViewModel model)
        {
            var crewMember = new CrewMember()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Nationality = model.Nationality,
                Age = model.Age,
            };
            var crewMembersDb = await repo.AllReadonly<CrewMember>().ToListAsync();
            if (crewMembersDb.Any(x=>x.FirstName==crewMember.FirstName 
                                  && x.LastName == crewMember.LastName 
                                  && x.Age == crewMember.Age 
                                  && x.Nationality==crewMember.Nationality))                
            {
                return false;
            }
            else
            {
                await repo.AddAsync<CrewMember>(crewMember);
                await repo.SaveChangesAsync();
                return true;
                
            }

        }       

        public async Task<IEnumerable<SelectListItem>> GetAllAvailableCrewMembers()
        {
            var dropDownMembers = await repo.AllReadonly<CrewMember>().Where(x => x.IsPartOfACrew == false).Select(x => new SelectListItem
            {
                Value = $"{x.FirstName} {x.LastName}, Age: {x.Age}, Nationality: [{x.Nationality}]",
                Text = x.Id.ToString(),

            }).ToListAsync();


            return dropDownMembers;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllCrewMembers(int vesselId)
        {
            var dropDownMembers = await repo.AllReadonly<CrewMember>().Include(x=>x.Vessel).Where(x => x.IsPartOfACrew == true && x.VesselId==vesselId).Select(x => new SelectListItem
            {
                Value = $"{x.FirstName} {x.LastName}, Age: {x.Age}, Nationality: [{x.Nationality}]",
                Text = x.Id.ToString(),

            }).ToListAsync();


            return dropDownMembers;
        }

        public async Task AddCrewMemberToVessel(CrewMembersDropDownViewModel model)
        {
            var vessel = await repo.GetByIdAsync<Vessel>(model.VesselId);
            
            var crewMember = await repo.GetByIdAsync<CrewMember>(model.memberId);
            if (vessel != null && crewMember != null)
            {
                crewMember.IsPartOfACrew = true;
                crewMember.VesselId = model.VesselId;
                crewMember.Vessel = vessel;
                crewMember.DateHired = DateTime.UtcNow;
                vessel.CrewMembers.Add(crewMember);
                await repo.SaveChangesAsync();
            }

        }

        public async Task RemovedCrewMemberFromVessel(CrewMembersDropDownViewModel model)
        {
            var vessel = await repo.AllReadonly<Vessel>().Include(x=>x.CrewMembers).Where(x=>x.Id==model.VesselId).FirstOrDefaultAsync();

            var crewMember = await repo.GetByIdAsync<CrewMember>(model.memberId);
            if (vessel != null && crewMember != null)
            {
                crewMember.IsPartOfACrew = false;
                crewMember.VesselId = null;                
                crewMember.DateHired = null;
                vessel.CrewMembers.Remove(crewMember);
                await repo.SaveChangesAsync();
            }

        }       
    }
}
