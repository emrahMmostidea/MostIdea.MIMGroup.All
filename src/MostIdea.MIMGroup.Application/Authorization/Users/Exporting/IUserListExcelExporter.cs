using System.Collections.Generic;
using MostIdea.MIMGroup.Authorization.Users.Dto;
using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}