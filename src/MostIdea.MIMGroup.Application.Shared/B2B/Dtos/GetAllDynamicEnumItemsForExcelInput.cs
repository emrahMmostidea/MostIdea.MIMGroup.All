using Abp.Application.Services.Dto;
using System;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAllDynamicEnumItemsForExcelInput
    {
        public string Filter { get; set; }

        public string EnumValueFilter { get; set; }

        public Guid? ParentIdFilter { get; set; }

        public int? IsAuthRestrictionFilter { get; set; }

        public string AuthorizedUsersFilter { get; set; }

        public string DynamicEnumNameFilter { get; set; }

    }
}