using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public async Task AddCrewMemberToDataBase(CrewMemberViewModel model)
        {
            var crewMember = new CrewMember()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Nationality = model.Nationality,
                Age = model.Age,
            };
            if (!repo.AllReadonly<CrewMember>().Contains(crewMember))
            {
                await repo.AddAsync<CrewMember>(crewMember);
                await repo.SaveChangesAsync();
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
        public async Task GetCrewMember(CrewMembersDropDownViewModel model)
        {
            var vessel = await repo.GetByIdAsync<Vessel>(model.VesselId);
            var crewMember = await repo.GetByIdAsync<CrewMember>(model.memberId);
            if (vessel != null && crewMember != null)
            {
                crewMember.IsPartOfACrew = true;
                crewMember.VesselId = model.VesselId;
                crewMember.Vessel = vessel;
                vessel.CrewMembers.Add(crewMember);
                await repo.SaveChangesAsync();
            }

        }
    }
}
