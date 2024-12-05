using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class WarehousesExcelExporter : NpoiExcelExporterBase, IWarehousesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public WarehousesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetWarehouseForViewDto> warehouses)
        {
            return null;
            //return CreateExcelPackage(
            //    "Warehouses.xlsx",
            //    excelPackage =>
            //    {

            //        var sheet = excelPackage.CreateSheet(L("Warehouses"));

            //        AddHeader(
            //            sheet,
            //            L("Name"),
            //            L("Coordinate"),
            //            (L("District")) + L("Name")
            //            );

            //        AddObjects(
            //            sheet, 2, warehouses,
            //            _ => _.Warehouse.Name,
            //            _ => _.Warehouse.Coordinate,
            //            _ => _.DistrictName
            //            );

            //    });
        }
    }
}