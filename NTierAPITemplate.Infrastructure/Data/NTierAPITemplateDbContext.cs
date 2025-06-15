using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierAPITemplate.Infrastructure.Data
{
    public class NTierAPITemplateDbContext : DbContext
    {
        public NTierAPITemplateDbContext(DbContextOptions<NTierAPITemplateDbContext> options)
            : base(options)
        {
        }
    }
}
