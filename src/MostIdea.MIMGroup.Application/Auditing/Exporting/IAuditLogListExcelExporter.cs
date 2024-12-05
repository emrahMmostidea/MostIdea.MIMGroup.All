using System.Collections.Generic;
using MostIdea.MIMGroup.Auditing.Dto;
using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
