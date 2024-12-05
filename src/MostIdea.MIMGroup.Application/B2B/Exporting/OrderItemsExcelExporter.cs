using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class OrderItemsExcelExporter : NpoiExcelExporterBase, IOrderItemsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrderItemsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderItemForViewDto> orderItems)
        {
            return null;
            //return CreateExcelPackage(
            //    "OrderItems.xlsx",
            //    excelPackage =>
            //    {

            //        var sheet = excelPackage.CreateSheet(L("OrderItems"));

            //        AddHeader(
            //            sheet,
            //            L("Price"),
            //            L("Amount"),
            //            L("Status"),
            //            (L("Product")) + L("Name"),
            //            (L("Order")) + L("OrderNo")
            //            );

            //        AddObjects(
            //            sheet, 2, orderItems,
            //            _ => _.OrderItem.Price,
            //            _ => _.OrderItem.Amount,
            //            _ => _.OrderItem.Status,
            //            _ => _.ProductName,
            //            _ => _.OrderOrderNo
            //            );

            //    });
        }
    }
}