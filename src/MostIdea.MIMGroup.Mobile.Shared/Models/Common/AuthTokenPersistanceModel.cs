using System;
using Abp.AutoMapper;
using MostIdea.MIMGroup.Sessions.Dto;

namespace MostIdea.MIMGroup.Models.Common
{
    [AutoMapFrom(typeof(ApplicationInfoDto)),
     AutoMapTo(typeof(ApplicationInfoDto))]
    public class ApplicationInfoPersistanceModel
    {
        public string Version { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}