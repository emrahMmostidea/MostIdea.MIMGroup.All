using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class DynamicEnumItemsExcelExporter : NpoiExcelExporterBase, IDynamicEnumItemsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DynamicEnumItemsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDynamicEnumItemForViewDto> dynamicEnumItems)
        {
            return null;
            //return CreateExcelPackage(
            //    "DynamicEnumItems.xlsx",
            //    excelPackage =>
            //    {

            //        var sheet = excelPackage.CreateSheet(L("DynamicEnumItems"));

            //        AddHeader(
            //            sheet,
            //            L("EnumValue"),
            //            L("ParentId"),
            //            L("IsAuthRestriction"),
            //            L("AuthorizedUsers"),
            //            (L("DynamicEnum")) + L("Name")
            //            );

            //        AddObjects(
            //            sheet, 2, dynamicEnumItems,
            //            _ => _.DynamicEnumItem.EnumValue,
            //            _ => _.DynamicEnumItem.ParentId,
            //            _ => _.DynamicEnumItem.IsAuthRestriction,
            //            _ => _.DynamicEnumItem.AuthorizedUsers,
            //            _ => _.DynamicEnumName
            //            );

            //    });
        }
    }
}