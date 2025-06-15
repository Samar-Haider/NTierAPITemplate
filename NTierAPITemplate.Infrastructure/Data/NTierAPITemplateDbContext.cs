using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NTierAPITemplate.Domain.Entities;

namespace NTierAPITemplate.Infrastructure.Data
{
    public class NTierAPITemplateDbContext : IdentityDbContext<UserAccount, IdentityRole<Guid>, Guid>
    {
        public NTierAPITemplateDbContext(DbContextOptions<NTierAPITemplateDbContext> options)
            : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // apply your extra configs:
            builder.ApplyConfigurationsFromAssembly(typeof(NTierAPITemplateDbContext).Assembly);
        }
    }
}