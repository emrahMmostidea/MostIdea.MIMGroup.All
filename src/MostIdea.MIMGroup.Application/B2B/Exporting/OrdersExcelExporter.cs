using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class OrdersExcelExporter : NpoiExcelExporterBase, IOrdersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrdersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderForViewDto> orders)
        {
            return null;
            //return CreateExcelPackage(
            //    "Orders.xlsx",
            //    excelPackage =>
            //    {

            //        var sheet = excelPackage.CreateSheet(L("Orders"));

            //        AddHeader(
            //            sheet,
            //            L("Total"),
            //            L("Tax"),
            //            L("GrandTotal"),
            //            L("Status"),
            //            L("OrderNo"),
            //            (L("AddressInformation")) + L("Name"),
            //            (L("User")) + L("Name"),
            //            (L("Hospital")) + L("Name"),
            //            (L("User")) + L("Name"),
            //            (L("Warehouse")) + L("Name")
            //            );

            //        AddObjects(
            //            sheet, 2, orders,
            //            _ => _.Order.Total,
            //            _ => _.Order.Tax,
            //            _ => _.Order.GrandTotal,
            //            _ => _.Order.Status,
            //            _ => _.Order.OrderNo,
            //            _ => _.AddressInformationName,
            //            _ => _.UserName,
            //            _ => _.HospitalName,
            //            _ => _.UserName2,
            //            _ => _.WarehouseName
            //            );

            //    });
        }
    }
}