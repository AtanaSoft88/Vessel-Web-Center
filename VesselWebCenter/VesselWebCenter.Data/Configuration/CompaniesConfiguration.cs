using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using VesselWebCenter.Data.ImportViewModels;
using VesselWebCenter.Data.Models;

namespace VesselWebCenter.Data.Configuration
{
    public class CompaniesConfiguration : IEntityTypeConfiguration<ManningCompany>
    {

        public void Configure(EntityTypeBuilder<ManningCompany> builder)
        {
            List<ManningCompany> companies = GetCompanies();
            builder.HasData(companies);           
        }

        private List<ManningCompany> GetCompanies()
        {                  
            var path = "bin\\Debug\\net6.0\\Configuration\\DataImportSets\\company.json";
            
            string jsonAsString = File.ReadAllText(path);
            var companiesDto = JsonConvert.DeserializeObject<CompanyViewModel[]>(jsonAsString);
            var companies = new List<ManningCompany>();

            foreach (var cDto in companiesDto)
            {
                var manningCompany = new ManningCompany() 
                {   Id = cDto.Id,     
                    Name = cDto.Name,
                    Country = cDto.Country,                   
                };               
                
                companies.Add(manningCompany);
            }
            return companies;
        }        
    }
}
