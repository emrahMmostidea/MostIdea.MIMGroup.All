using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace MostIdea.MIMGroup.EntityFrameworkCore
{
    public static class MIMGroupDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<MIMGroupDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<MIMGroupDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}