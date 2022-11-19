using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace VesselWebCenter.Data.Models.Accounts
{
    // No need to be inserted into Dbase as DbSet<> ->> because it only extends the default User Identity
    public class AppUser : IdentityUser<Guid>
    {
        private DateTime? deletedOn = null;

        [Required]
        [StringLength(21)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(21)]
        public string? LastName { get; set; }

        public DateTime CreatedOn => DateTime.Now;

        // to be deleted in a real project , no password in db allowed! Only hashed passwords.
        //public string PasswordPreserved { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn 
        {
            get => this.deletedOn;            
            set 
            {
                if (this.IsDeleted==true)
                {
                    value = DateTime.Now;
                }
                this.deletedOn = value;
            }
        }
    }
}