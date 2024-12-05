using System.Collections.Generic;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.Editions.Dto;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}