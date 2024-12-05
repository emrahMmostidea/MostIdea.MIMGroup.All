using System.Collections.Generic;
using MostIdea.MIMGroup.Authorization.Users.Importing.Dto;
using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
