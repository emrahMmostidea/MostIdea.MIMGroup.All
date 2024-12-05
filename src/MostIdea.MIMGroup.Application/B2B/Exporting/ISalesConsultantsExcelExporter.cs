using System.Collections.Generic;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public interface ISalesConsultantsExcelExporter
    {
        FileDto ExportToFile(List<GetSalesConsultantForViewDto> salesConsultants);
    }
}