using NuGet.Packaging;

namespace VesselWebCenter.Tests.DataPopulation
{
    public class DataPopulator
    {
        private static string LetterRandomizator(int count, bool isLower)
        {
            string randomName = "";
            if (isLower)
            {                
                for (int i = 0; i < count; i++)
                {
                    var randomChar = ((char)new Random().Next(97, 123));
                    randomName += randomChar;
                }
                return randomName;
            }
            else
            {                
                for (int i = 0; i < count; i++)
                {
                    var randomChar = ((char)new Random().Next(65, 91));
                    randomName += randomChar;
                }
                return randomName;
            }
            
        }
        internal static List<T> PortOfCallPopulator<T>(List<T> model)
            where T : PortOfCall, new()
        {
            bool isUpper = false;
            bool isLower = true;
            var portList = new List<T>();
            for (int i = 1; i <= 11; i++)
            {
                var ss = new T 
                {
                    Id = i,
                    Country = "A" + (char)new Random().Next(97, 123),
                    PortName = LetterRandomizator(10, isLower),
                    Latitude = $"15.{i} N",
                    Longitude = $"{i}.30 E",
                    UNLocode = Guid.NewGuid().ToString().Split("-").First().ToString(),
                    Vessels = new List<Vessel>()
                    {
                      new Vessel
                      {
                          Id = i,
                          Name="Maveric",
                          LengthOverall=190,
                          BreadthMax = 20,
                          CallSign = LetterRandomizator(10,isUpper),
                          IsLaden = true,
                          ManningCompanyId=1,
                          VesselImageUrl="",
                          VesselType=0,
                          CargoTypeOnBoard="",
                          ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                      },


                    }

                };
                portList.Add(ss);


            };
            model.AddRange(portList);
            model.AddRange(new List<T>
            {
                new T
                {
                   Id = 12,
                   Country = "Egypt",
                   PortName = "Alexandria",
                   Latitude = "15.30 N",
                   Longitude = "18.30 E",
                   UNLocode = "UNCODE0",
                   Vessels = new List<Vessel>()
                   {
                      new Vessel
                      {
                          Id = 12,
                          Name="Maveric",
                          LengthOverall=190,
                          BreadthMax = 20,
                          CallSign ="xxxx",
                          IsLaden = true,
                          ManningCompanyId=1,
                          VesselImageUrl="",
                          VesselType=0,
                          CargoTypeOnBoard="",
                          ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                      },


                   }

                },
                new T
                {
                    Id = 13,
                    Country="Germany",
                    PortName="Hamburg",
                    Latitude="55.30 N",
                    Longitude="6.30 E",
                    UNLocode = "UNCODE1",
                    Vessels = new List<Vessel>()
                    {
                        new Vessel
                        {
                            Id = 13,
                            Name="Maveric",
                            LengthOverall=190,
                            BreadthMax = 20,
                            CallSign ="xxxx",
                            IsLaden = true,
                            ManningCompanyId=1,
                            VesselImageUrl="",
                            VesselType=0,
                            CargoTypeOnBoard="",
                            ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                        },


                    }

                },
                new T
                {
                    Id = 15,
                    Country="Bulgaria",
                    PortName="Varna",
                    Latitude="40.30 N",
                    Longitude="28.30 E",
                    UNLocode = "UNCODE1",
                    Vessels = new List<Vessel>()
                    {
                        new Vessel
                        {
                            Id = 15,
                            Name="Maveric",
                            LengthOverall=190,
                            BreadthMax = 20,
                            CallSign ="xxxx",
                            IsLaden = true,
                            ManningCompanyId=1,
                            VesselImageUrl="",
                            VesselType=0,
                            CargoTypeOnBoard="",
                            ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                        },
                        new Vessel
                        {
                            Id = 16,
                            Name="Maveric",
                            LengthOverall=190,
                            BreadthMax = 20,
                            CallSign ="xxxx",
                            IsLaden = true,
                            ManningCompanyId=1,
                            VesselImageUrl="",
                            VesselType=0,
                            CargoTypeOnBoard="",
                            ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                        },
                        new Vessel
                        {
                            Id = 17,
                            Name="Maveric",
                            LengthOverall=190,
                            BreadthMax = 20,
                            CallSign ="xxxx",
                            IsLaden = true,
                            ManningCompanyId=1,
                            VesselImageUrl="",
                            VesselType=0,
                            CargoTypeOnBoard="",
                            ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                        },
                        new Vessel
                        {
                            Id = 18,
                            Name="Maveric",
                            LengthOverall=190,
                            BreadthMax = 20,
                            CallSign ="xxxx",
                            IsLaden = true,
                            ManningCompanyId=1,
                            VesselImageUrl="",
                            VesselType=0,
                            CargoTypeOnBoard="",
                            ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                        },

                    }

                },
                new T
                {
                    Id = 14,
                    Country="USA",
                    PortName="New Orleans",
                    Latitude="57.30 N",
                    Longitude="54.30 W",
                    UNLocode = "UNCODE1",
                    Vessels = new List<Vessel>()
                    {
                        new Vessel
                        {
                            Id = 14,
                            Name="Maveric",
                            LengthOverall=190,
                            BreadthMax = 20,
                            CallSign ="xxxx",
                            IsLaden = true,
                            ManningCompanyId=1,
                            VesselImageUrl="",
                            VesselType=0,
                            CargoTypeOnBoard="",
                            ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                        },
                        new Vessel
                        {
                            Id = 15,
                            Name="Maveric",
                            LengthOverall=190,
                            BreadthMax = 20,
                            CallSign ="xxxx",
                            IsLaden = true,
                            ManningCompanyId=1,
                            VesselImageUrl="",
                            VesselType=0,
                            CargoTypeOnBoard="",
                            ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                        },

                    }

                }
                
            });
            return model;

        }

        internal static List<T> CrewPopulator<T>(List<T> model)
            where T : CrewMember, new()
        {
            model.AddRange(new List<T>
            {
                new T
                {
                    Id = 1,
                    FirstName = "Pesho",
                    LastName = "Goshev",
                    Nationality = "Bgn",
                    Age = 18,
                    IsPartOfACrew = true,
                    VesselId = 5
                },
                new T
                {
                    Id = 2,
                    FirstName = "Stoycho" ,
                    LastName = "Marinov",
                    Age = 34,
                    Nationality = "Bg",
                    IsPartOfACrew = false,
                    VesselId = null

                },
                new T
                {
                    Id = 3,
                    FirstName = "Nikola",
                    LastName = "Cenov",
                    Nationality = "Bgn",
                    Age = 20,
                    IsPartOfACrew = false,
                    VesselId = null
                },
                 new T
                 {
                    Id = 4,
                    FirstName = "Gencho" ,
                    LastName = "Marinov",
                    Age = 54,
                    Nationality = "Bg",
                    IsPartOfACrew = true,
                    VesselId = 5
                 },
                  new T
                  {
                    Id = 5,
                    FirstName = "Nikola",
                    LastName = "Cenov",
                    Nationality = "Bgn",
                    Age = 20,
                    IsPartOfACrew = true,
                    VesselId = 5

                  }
            });
            return model;
        }

        internal static List<T> VesselPopulator<T>(List<T> model)
           where T : Vessel, new()
        {
            model.AddRange(new List<T>
            {
                new T
                {
                    Id = 1,
                    Name="Verila",
                    LengthOverall=160,
                    BreadthMax = 30,
                    CallSign ="xxxx",
                    IsLaden = true,
                    ManningCompanyId=1,
                    VesselImageUrl="",
                    VesselType=0,
                    CargoTypeOnBoard="",
                    ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                },
                new T
                {
                    Id = 2,
                    Name="Lena",
                    LengthOverall=190,
                    BreadthMax = 30,
                    CallSign ="xxxx",
                    IsLaden = false,
                    ManningCompanyId=1,
                    VesselImageUrl="",
                    VesselType=0,
                    CargoTypeOnBoard=null,
                    ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                },
                new T
                {
                    Id = 3,
                    Name="Persian Miracle",
                    LengthOverall=220,
                    BreadthMax = 30,
                    CallSign ="xxxx",
                    IsLaden = true,
                    ManningCompanyId=1,
                    VesselImageUrl="",
                    VesselType=0,
                    CargoTypeOnBoard="",
                    ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                },
                 new T
                 {
                    Id = 4,
                    Name="Seven Ocenas",
                    LengthOverall=260,
                    BreadthMax = 20,
                    CallSign ="xxxx",
                    IsLaden = true,
                    ManningCompanyId=1,
                    VesselImageUrl="",
                    VesselType=0,
                    CargoTypeOnBoard="",
                    ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                 },
                  new T
                  {
                    Id = 5,
                    Name="Maveric",
                    LengthOverall=190,
                    BreadthMax = 20,
                    CallSign ="xxxx",
                    IsLaden = true,
                    ManningCompanyId=1,
                    VesselImageUrl="",
                    VesselType=0,
                    CargoTypeOnBoard="",
                    ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                  }

            });
            return model;
        }

        internal static List<T> CompanyPopulator<T>(List<T> model)
            where T : ManningCompany, new()
        {
            model.AddRange(new List<T>
            {
                new T
                {
                    Id = 1,
                    Name = "Company1",
                    Country = "Bgn",
                    Vessels = new List<Vessel>()
                    {
                        new Vessel()
                        {
                            Id = 12,
                            Name="Seven Ocenas",
                            LengthOverall=260,
                            BreadthMax = 20,
                            CallSign ="xxxx",
                            IsLaden = true,
                            ManningCompanyId=4,
                            VesselImageUrl="",
                            VesselType=0,
                            CargoTypeOnBoard="",
                        }
                    }
                },
                new T
                {
                    Id = 2,
                    Name = "Company2",
                    Country = "Ng",
                    Vessels = new List<Vessel>()
                    {
                        new Vessel()
                        {
                            Id = 9,
                            Name="Seven Ocenas",
                            LengthOverall=260,
                            BreadthMax = 20,
                            CallSign ="xxxx",
                            IsLaden = true,
                            ManningCompanyId=4,
                            VesselImageUrl="",
                            VesselType=0,
                            CargoTypeOnBoard="",
                        }
                    }
                },
                new T
                {
                    Id = 3,
                    Name = "BestCompanyEver",
                    Country = "Bg",
                    Vessels = new List<Vessel>()
                    {
                        new Vessel()
                        {
                            Id = 1,
                            Name="Seven Ocenas",
                            LengthOverall=260,
                            BreadthMax = 20,
                            CallSign ="xxxx",
                            IsLaden = true,
                            ManningCompanyId=3,
                            VesselImageUrl="",
                            VesselType=0,
                            CargoTypeOnBoard="",
                        },
                        new Vessel()
                        {
                            Id = 2,
                            Name="Seven Ocenas",
                            LengthOverall=260,
                            BreadthMax = 20,
                            CallSign ="xxxx",
                            IsLaden = true,
                            ManningCompanyId=3,
                            VesselImageUrl="",
                            VesselType=0,
                            CargoTypeOnBoard="",
                        }
                    }
                },
                 new T
                 {
                    Id = 4,
                    Name = "Company4",
                    Country = "Dn",
                    Vessels = new List<Vessel>()
                    {
                        new Vessel()
                        { Id = 5,
                            Name = "Seven Ocenas",
                            LengthOverall = 260,
                            BreadthMax = 20,
                            CallSign = "xxxx",
                            IsLaden = true,
                            ManningCompanyId = 4,
                            VesselImageUrl = "",
                            VesselType = 0,
                            CargoTypeOnBoard = ""
                    }   }
                 }
            });
            return model;
        }

    }
}
