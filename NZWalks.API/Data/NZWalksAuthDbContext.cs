using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerId = "4F299724-9BC9-4D2D-961C-DB8F5B80BC0A";
            var writerId = "A918D1B0-5365-45AF-A9A2-599545D5AE34";
            var roles = new List<IdentityRole> {
                new IdentityRole
                {
                    Id = readerId,
                    ConcurrencyStamp = readerId,
                    Name= "Reader",
                    NormalizedName = "Reader".ToUpper(),
                },
                new IdentityRole
                {
                    Id = writerId,
                    ConcurrencyStamp = readerId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                }
                };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
