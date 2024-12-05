using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class ProductPricesForHospitalsExcelExporter : NpoiExcelExporterBase, IProductPricesForHospitalsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductPricesForHospitalsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductPricesForHospitalForViewDto> productPricesForHospitals)
        {
            return null;
            //return CreateExcelPackage(
            //    "ProductPricesForHospitals.xlsx",
            //    excelPackage =>
            //    {

            //        var sheet = excelPackage.CreateSheet(L("ProductPricesForHospitals"));

            //        AddHeader(
            //            sheet,
            //            L("StartDate"),
            //            L("EndDate"),
            //            L("Price"),
            //            (L("Product")) + L("Name"),
            //            (L("ProductCategory")) + L("Name")
            //            );

            //        AddObjects(
            //            sheet, 2, productPricesForHospitals,
            //            _ => _timeZoneConverter.Convert(_.ProductPricesForHospital.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
            //            _ => _timeZoneConverter.Convert(_.ProductPricesForHospital.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
            //            _ => _.ProductPricesForHospital.Price,
            //            _ => _.ProductName,
            //            _ => _.ProductCategoryName
            //            );

            //        for (var i = 1; i <= productPricesForHospitals.Count; i++)
            //        {
            //            SetCellDataFormat(sheet.GetRow(i).Cells[1], "yyyy-mm-dd");
            //        }
            //        sheet.AutoSizeColumn(1); for (var i = 1; i <= productPricesForHospitals.Count; i++)
            //        {
            //            SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
            //        }
            //        sheet.AutoSizeColumn(2);
            //    });
        }
    }
}