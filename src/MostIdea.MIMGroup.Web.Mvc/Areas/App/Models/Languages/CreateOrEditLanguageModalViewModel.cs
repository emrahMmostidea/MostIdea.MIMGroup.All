﻿using Abp.AutoMapper;
using MostIdea.MIMGroup.Localization.Dto;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Languages
{
    [AutoMapFrom(typeof(GetLanguageForEditOutput))]
    public class CreateOrEditLanguageModalViewModel : GetLanguageForEditOutput
    {
        public bool IsEditMode => Language.Id.HasValue;
    }
}