using System.Linq;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.Authorization.Roles;
using MostIdea.MIMGroup.Authorization.Users;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.EntityFrameworkCore;
using MostIdea.MIMGroup.Notifications;

namespace MostIdea.MIMGroup.Migrations.Seed.Tenants
{
    public class MimB2BBuilder
    {
        private readonly MIMGroupDbContext _context;
        private readonly int _tenantId;

        public MimB2BBuilder(MIMGroupDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateMimData();
        }

        private void CreateMimData()
        {
            var doctorRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == "Doktor");
            if (doctorRole == null)
            {
                doctorRole = _context.Roles.Add(new Role(_tenantId, "Doktor", "Doktor") {IsStatic = true}).Entity;
                _context.SaveChanges();
            }

            var dealerRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == "Bayi");
            if (dealerRole == null)
            {
                dealerRole = _context.Roles.Add(new Role(_tenantId, "Bayi", "Bayi") { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            var satisTemRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == "Satış Temsilcisi");
            if (satisTemRole == null)
            {
                satisTemRole = _context.Roles.Add(new Role(_tenantId, "Satış Temsilcisi", "Satış Temsilcisi") { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            var hospitalManagerRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == "Hastane Yöneticisi");
            if (hospitalManagerRole == null)
            {
                hospitalManagerRole = _context.Roles.Add(new Role(_tenantId, "Hastane Yöneticisi", "Hastane Yöneticisi") { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            var courierRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == "Kurye");
            if (courierRole == null)
            {
                courierRole = _context.Roles.Add(new Role(_tenantId, "Kurye", "Kurye") { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            var assistanceRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == "Asistan");
            if (assistanceRole == null)
            {
                assistanceRole = _context.Roles.Add(new Role(_tenantId, "Asistan", "Asistan") { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            var klinikHG = _context.HospitalGroups.IgnoreQueryFilters().FirstOrDefault(x => x.Name == "Klinik");
            if (klinikHG == null)
            {
                klinikHG = _context.HospitalGroups.Add(new HospitalGroup() { Name = "Klinik" }).Entity;
                _context.SaveChanges();
            }

            var bayiHG = _context.HospitalGroups.IgnoreQueryFilters().FirstOrDefault(x => x.Name == "Klinik");
            if (bayiHG == null)
            {
                bayiHG = _context.HospitalGroups.Add(new HospitalGroup() { Name = "Bayi" }).Entity;
                _context.SaveChanges();
            }

        }
    }
}
