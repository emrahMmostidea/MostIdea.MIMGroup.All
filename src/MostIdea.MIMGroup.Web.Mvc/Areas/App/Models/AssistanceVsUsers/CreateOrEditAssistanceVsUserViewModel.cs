using MostIdea.MIMGroup.B2B.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.AssistanceVsUsers
{
    public class CreateOrEditAssistanceVsUserModalViewModel
    {
        public CreateOrEditAssistanceVsUserDto AssistanceVsUser { get; set; }

        public string UserName { get; set; }

        public string UserName2 { get; set; }

        public List<AssistanceVsUserUserLookupTableDto> AssistanceVsUserUserList { get; set; }

        public bool IsEditMode => AssistanceVsUser.Id.HasValue;
    }
}