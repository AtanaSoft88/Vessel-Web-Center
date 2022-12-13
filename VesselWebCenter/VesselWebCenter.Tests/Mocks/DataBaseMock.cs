using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    .UseInMemoryDatabase("A" + new Random().Next(1,100).ToString())
                    .Options;
                return new VesselAppDbContext(contextOptions);
            }
        }
    }
}
