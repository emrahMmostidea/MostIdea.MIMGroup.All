using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class ProductsExcelExporter : NpoiExcelExporterBase, IProductsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductForViewDto> products)
        {
            return null;
            //return CreateExcelPackage(
            //    "Products.xlsx",
            //    excelPackage =>
            //    {

            //        var sheet = excelPackage.CreateSheet(L("Products"));

            //        AddHeader(
            //            sheet,
            //            L("Name"),
            //            L("Description"),
            //            L("Price"),
            //            L("Quantity"),
            //            (L("ProductCategory")) + L("Name"),
            //            (L("TaxRate")) + L("Name")
            //            );

            //        AddObjects(
            //            sheet, 2, products,
            //            _ => _.Product.Name,
            //            _ => _.Product.Description,
            //            _ => _.Product.Price,
            //            _ => _.Product.Quantity,
            //            _ => _.ProductCategoryName,
            //            _ => _.TaxRateName
            //            );

            //    });
        }
    }
}