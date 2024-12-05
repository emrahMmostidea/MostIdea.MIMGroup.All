using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class ProductCategoriesExcelExporter : NpoiExcelExporterBase, IProductCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductCategoryForViewDto> productCategories)
        {
            return null;
            //return CreateExcelPackage(
            //    "ProductCategories.xlsx",
            //    excelPackage =>
            //    {

            //        var sheet = excelPackage.CreateSheet(L("ProductCategories"));

            //        AddHeader(
            //            sheet,
            //            L("Name"),
            //            L("Description"),
            //            (L("ProductCategory")) + L("Name"),
            //            (L("Brand")) + L("Name")
            //            );

            //        AddObjects(
            //            sheet, 2, productCategories,
            //            _ => _.ProductCategory.Name,
            //            _ => _.ProductCategory.Description,
            //            _ => _.ProductCategoryName,
            //            _ => _.BrandName
            //            );

            //    });
        }
    }
}