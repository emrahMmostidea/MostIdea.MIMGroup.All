using System.Collections.Generic;
using Abp;
using MostIdea.MIMGroup.Chat.Dto;
using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
