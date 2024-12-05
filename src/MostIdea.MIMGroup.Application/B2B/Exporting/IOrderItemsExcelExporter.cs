using System.Collections.Generic;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public interface IOrderItemsExcelExporter
    {
        FileDto ExportToFile(List<GetOrderItemForViewDto> orderItems);
    }
}