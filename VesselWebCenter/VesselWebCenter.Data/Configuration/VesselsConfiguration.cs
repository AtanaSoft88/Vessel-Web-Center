using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using VesselWebCenter.Data.Configuration.ImportViewModels;
using VesselWebCenter.Data.Enums;
using VesselWebCenter.Data.Models;

namespace VesselWebCenter.Data.Configuration
{
    public class VesselsConfiguration : IEntityTypeConfiguration<Vessel>
    {
        public void Configure(EntityTypeBuilder<Vessel> builder)
        {
            List<Vessel> vessels = GetVessels();
            builder.HasData(vessels);
        }
        private List<Vessel> GetVessels()
        {                  //Configuration\DataImportSets
            var path = "bin\\Debug\\net6.0\\Configuration\\DataImportSets\\vessels.json";

            string jsonAsString = File.ReadAllText(path);
            var vesselsDto = JsonConvert.DeserializeObject<VesselViewModel[]>(jsonAsString);
            var vessels = new List<Vessel>();

            foreach (var vDto in vesselsDto)
            {
                var vessel = new Vessel()
                {
                    Id = vDto.Id,
                    Name = vDto.Name,
                    CallSign = vDto.CallSign,
                    BreadthMax = vDto.BreadthMax,
                    LengthOverall = vDto.LengthOverall,
                    VesselType = (VesselType)vDto.VesselType,
                    CargoTypeOnBoard = vDto.CargoTypeOnBoard,
                    ManningCompanyId = vDto.ManningCompanyId,
                };

                vessels.Add(vessel);
            }
            return vessels;
        }
    }
}
