using System.Collections.Generic;
using MostIdea.MIMGroup.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace MostIdea.MIMGroup.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
