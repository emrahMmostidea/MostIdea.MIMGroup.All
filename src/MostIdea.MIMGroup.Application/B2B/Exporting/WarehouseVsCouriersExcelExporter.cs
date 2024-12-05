using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class WarehouseVsCouriersExcelExporter : NpoiExcelExporterBase, IWarehouseVsCouriersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public WarehouseVsCouriersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetWarehouseVsCourierForViewDto> warehouseVsCouriers)
        {
            return null;
            //return CreateExcelPackage(
            //    "WarehouseVsCouriers.xlsx",
            //    excelPackage =>
            //    {

            //        var sheet = excelPackage.CreateSheet(L("WarehouseVsCouriers"));

            //        AddHeader(
            //            sheet,
            //            (L("User")) + L("Name"),
            //            (L("Warehouse")) + L("Name")
            //            );

            //        AddObjects(
            //            sheet, 2, warehouseVsCouriers,
            //            _ => _.UserName,
            //            _ => _.WarehouseName
            //            );

            //    });
        }
    }
}