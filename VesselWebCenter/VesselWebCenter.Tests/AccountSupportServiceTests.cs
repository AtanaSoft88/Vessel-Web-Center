using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MockQueryable.Moq;
using NuGet.Packaging;
using NUnit.Framework.Internal;
using System.Security.Principal;
using VesselWebCenter.Data.Models.Accounts;
using VesselWebCenter.Services.ViewModels;
using VesselWebCenter.Tests.DataPopulation;
using VesselWebCenter.Tests.Mocks;

namespace VesselWebCenter.Tests
{
    public class AccountSupportServiceTests : DataPopulator
    {
        private IRepository repo;
        private IAccountSupportService service;
        private UserManager<AppUser> userManager;

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        [TestCase(1)]
        [TestCase(25)]
        [TestCase(26)]
        [TestCase(27)]
        [TestCase(110)]
        [TestCase(1000)]
        [TestCase(999)]
        public async Task Get_All_Users_Which_Are_Available_Should_Return_Their_Count(int countUsers)
        {
            var repoMock = new Mock<IRepository>();
            var appUsers = ApplicationUserPopulator(new List<AppUser>(), countUsers);
            var totalUsersInDb = appUsers.Count();
            repoMock.Setup(r => r.All<AppUser>())
                    .Returns(appUsers.AsQueryable().BuildMock());
            repo = repoMock.Object;
            service = new AccountSupportService(repo, userManager);
            var result = await service.GetAllUsers();
            Assert.That(result.Count(), Is.GreaterThan(0));
            Assert.That(result.Count(), Is.LessThanOrEqualTo(totalUsersInDb));
            if (countUsers != 1)
            {
                Assert.That(totalUsersInDb % 2 == 0 ? result.Count() * 2 : (result.Count() * 2) - 1, Is.EqualTo(totalUsersInDb));
            }
        }

        [Test]
        [TestCase(0, "DeleteIt@abv.bg")]
        [TestCase(0, "fakeEmail")]
        [TestCase(15, "DeleteIt@abv.bg")]
        [TestCase(1, "something")]
        [TestCase(12, "non-ExistedEmail")]
        [TestCase(10, "DeleteIt@abv.bg")]

        public async Task Get_Selected_User_Flagged_As_Deleted(int countUsers, string email)
        {
            var fakeDb = DataBaseMock.Instance;
            await fakeDb.AddRangeAsync(ApplicationUserPopulator(new List<AppUser>(), countUsers));
            fakeDb.Add(new AppUser
            {
                Id = Guid.NewGuid(),
                FirstName = $"Gosho",
                Email = "DeleteIt@abv.bg",
                EmailConfirmed = true,
                IsDeleted = false,
                LastName = $"Petrov",
                NormalizedEmail = "DeleteIt@abv.bg".ToUpper(),
                UserName = "DeleteIt@abv.bg",
            });
            await fakeDb.SaveChangesAsync();
            repo = new Repository(fakeDb);
            service = new AccountSupportService(repo, userManager);

            AccountDeleteViewModel account = new AccountDeleteViewModel()
            {
                DeletedOn = null,
                EmailAddress = email,
                isDeleted = false,                
            };
            var dbUsersBeforeDeletion = fakeDb.Users.Where(x => x.IsDeleted == false).Count();
            await service.DeleteAccountAsync(account);
            var dbUsersAfterDeletion = fakeDb.Users.Where(x => x.IsDeleted == false).Count();
            if (account.EmailAddress == "DeleteIt@abv.bg")
            {
                Assert.That(dbUsersBeforeDeletion, Is.GreaterThan(dbUsersAfterDeletion));
                Assert.That(dbUsersAfterDeletion, Is.GreaterThanOrEqualTo(0));
            }
            else
            {
                Assert.That(dbUsersBeforeDeletion, Is.EqualTo(dbUsersAfterDeletion));
            }

        }

        [Test]
        [TestCase(10)]
        [TestCase(13)]
        [TestCase(45)]
        [TestCase(99)]

        public async Task Get_All_Deleted_Users_Return_Count(int countUsers)
        {
            var repoMock = new Mock<IRepository>();
            var appUsers = ApplicationUserPopulator(new List<AppUser>(), countUsers);
            var totalUsersInDb = appUsers.Count();
            var usersFlaggedAsDeleted = appUsers.Where(x => x.IsDeleted == true).Count();
            repoMock.Setup(r => r.All<AppUser>())
                    .Returns(appUsers.AsQueryable().BuildMock());
            repo = repoMock.Object;
            service = new AccountSupportService(repo, userManager);
            var result = await service.GetAllDeletedUsers();
            Assert.That(result.Count(), Is.GreaterThan(0));
            Assert.That(result.Count(), Is.EqualTo(usersFlaggedAsDeleted));           

        }

        [Test]
        [TestCase(0, "RecoverIt@abv.bg")]
        [TestCase(0, "fakeEmail")]
        [TestCase(15, "RecoverIt@abv.bg")]
        [TestCase(1, "something")]
        [TestCase(12, "non-ExistedEmail")]
        [TestCase(10, "RecoverIt@abv.bg")]
        public async Task Get_User_Account_Recovered(int countUsers, string email)
        {
            var fakeDb = DataBaseMock.Instance;
            await fakeDb.AddRangeAsync(ApplicationUserPopulator(new List<AppUser>(), countUsers));
            fakeDb.Add(new AppUser
            {
                Id = Guid.NewGuid(),
                FirstName = $"Gosho",
                Email = "RecoverIt@abv.bg",
                EmailConfirmed = true,
                IsDeleted = true,
                LastName = $"Petrov",
                NormalizedEmail = "RecoverIt@abv.bg".ToUpper(),
                UserName = "RecoverIt@abv.bg",
            });
            await fakeDb.SaveChangesAsync();
            repo = new Repository(fakeDb);
            service = new AccountSupportService(repo, userManager);

            AccountRecoverViewModel account = new AccountRecoverViewModel()
            {
                DeletedOn = null,
                EmailAddress = email,
                isDeleted = false,                
            };
            var dbUsersBeforeRecover = fakeDb.Users.Where(x => x.IsDeleted == false).Count();
            await service.GetUserAccountRecovered(account);
            var dbUsersAfterRecover = fakeDb.Users.Where(x => x.IsDeleted == false).Count();
            if (account.EmailAddress == "RecoverIt@abv.bg")
            {
                Assert.That(dbUsersBeforeRecover, Is.LessThan(dbUsersAfterRecover));
                Assert.That(dbUsersAfterRecover, Is.GreaterThanOrEqualTo(0));
            }
            else
            {
                Assert.That(dbUsersBeforeRecover, Is.EqualTo(dbUsersAfterRecover));
            }

        }


        [TearDown]
        public void TearDown()
        {

        }

    }
}
