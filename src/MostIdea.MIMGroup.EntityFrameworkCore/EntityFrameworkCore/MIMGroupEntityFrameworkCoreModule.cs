using Abp;
using Abp.Dependency;
using Abp.Domain.Entities.Auditing;
using Abp.EntityFrameworkCore.Configuration;
using Abp.IdentityServer4vNext;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using MostIdea.MIMGroup.Configuration;
using MostIdea.MIMGroup.EntityHistory;
using MostIdea.MIMGroup.Migrations.Seed;

namespace MostIdea.MIMGroup.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpZeroCoreEntityFrameworkCoreModule),
        typeof(MIMGroupCoreModule),
        typeof(AbpZeroCoreIdentityServervNextEntityFrameworkCoreModule)
    )]
    public class MIMGroupEntityFrameworkCoreModule : AbpModule
    {
        /* Used it tests to skip DbContext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
                Configuration.Modules.AbpEfCore().AddDbContext<MIMGroupDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                        MIMGroupDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    else
                        MIMGroupDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                });

            // Set this setting to true for enabling entity history.
            Configuration.EntityHistory.IsEnabled = true;

            // Uncomment below line to write change logs for the entities below:
            //Configuration.EntityHistory.Selectors.Add("MIMGroupEntities", EntityHistoryHelper.TrackedTypes);
            //Configuration.CustomConfigProviders.Add(new EntityHistoryConfigProvider(Configuration));
            
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MIMGroupEntityFrameworkCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            var configurationAccessor = IocManager.Resolve<IAppConfigurationAccessor>();

            using (var scope = IocManager.CreateScope())
            {
                if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>()
                    .Exist(configurationAccessor.Configuration["ConnectionStrings:Default"]))
                    SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}