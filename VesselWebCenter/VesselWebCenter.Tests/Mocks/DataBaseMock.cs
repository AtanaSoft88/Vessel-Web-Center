using Microsoft.EntityFrameworkCore;
using VesselWebCenter.Data;

namespace VesselWebCenter.Tests.Mocks
{
    public static class DataBaseMock
    {
        public static  VesselAppDbContext Instance
        {
            get 
            {
                var contextOptions = new DbContextOptionsBuilder<VesselAppDbContext>()
                    .UseInMemoryDatabase("Base" + new Random().Next(1,1000).ToString())
                    .Options;
                return new VesselAppDbContext(contextOptions);
            }
        }
    }
}
