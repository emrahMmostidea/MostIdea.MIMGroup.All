using System.Collections.Generic;
using MostIdea.MIMGroup.Caching.Dto;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}